using System;
using CRender.Pipeline.Rasterization;
using System.Collections.Generic;
using CRender.Sampler;
using CRender.Structure;
using CShader;
using CUtility.Collection;
using CUtility.Math;

using static CUtility.Math.Matrix4x4;

using VertexInvoker = CShader.ShaderInvoker<CShader.IVertexShader, CShader.ShaderInOutPatternDefault>;
using FragmentInvoker = CShader.ShaderInvoker<CShader.IFragmentShader, CShader.ShaderInOutPatternDefault>;

namespace CRender.Pipeline
{
    [Flags]
    public enum PrimitiveAssembleMode { Line = 1, Triangle = 2, LineTriangle = Line | Triangle }

    public partial class Pipeline : IPipeline, IDisposable
    {
        private static readonly Material DEFAULT_MATERIAL = Material.NewMaterial(ShaderDefault.Instance);

        public RenderBuffer<float> RenderTarget { get; }

        private static readonly unsafe Matrix4x4* ViewToScreen = AllocMatrix(0f, .5f, 0f, .5f,
                                                                             0f, 0f, .5f, .5f,
                                                                             0f, 0f, 0f, 0f,
                                                                             0f, 0f, 0f, 1f);

        private readonly Vector2 _bufferSizeF;

        private readonly Vector2Int _bufferSize;

        private readonly UnsafeList<LinePrimitive> _linePrimitives = new UnsafeList<LinePrimitive>();

        private readonly UnsafeList<TrianglePrimitive> _trianglePrimitives = new UnsafeList<TrianglePrimitive>();

        private readonly UnsafeList<Fragment> _rasterizedFragments = new UnsafeList<Fragment>();

        private readonly UnsafeList<Vector4> _renderedColors = new UnsafeList<Vector4>();

        private PrimitiveAssembleMode _assembleMode = PrimitiveAssembleMode.Triangle;

        public Pipeline()
        {
            _bufferSize = CRenderSettings.Resolution;
            _bufferSizeF = (Vector2)_bufferSize;
            RenderTarget = new RenderBuffer<float>(_bufferSize.X, _bufferSize.Y, channelCount: 4);
        }

        public unsafe RenderBuffer<float> Draw<T>(RenderEntity[] entities, T camera) where T : ICamera
        {
            Clear();

            int entityCount = entities.Length;

            BeginRasterize();
            SetPerRenderValues(camera);
            for (int i = 0; i < entityCount; i++)
            {
                RenderEntity currentEntity = entities[i];
                SetPerObjectValues(currentEntity);

                Material material = currentEntity.Material ?? DEFAULT_MATERIAL;
                Model model = currentEntity.Model;

                #region Vertex Stage

                //Prepare vertex shader
                VertexInvoker.ChangeActiveShader(material.ShaderType, material.Shader);

                //Invoke vertex shader
                int vertexCount = model.Vertices.Length;
                Vector2* coordsOutput = stackalloc Vector2[vertexCount];
                ShaderInOutPatternDefault* inoutPtr = stackalloc ShaderInOutPatternDefault[1];
                for (int j = 0; j < vertexCount; j++)
                {
                    VertexInvoker.Invoke(model.ReadVerticesDataAsPattern(j));
                    VertexInvoker.ActiveOutputMap.Write(inoutPtr);
                    coordsOutput[j] = *(Vector2*)&inoutPtr->Vertex;
                }

                #endregion

                #region Fragment Stage

                FragmentInvoker.ChangeActiveShader(material.ShaderType, material.Shader);

                //Primitive assemble
                int newPrimitiveCount = AssemblePrimitive(model, coordsOutput);
                _rasterizedFragments.AddEmpty(newPrimitiveCount);
                switch (_assembleMode)
                {
                    case PrimitiveAssembleMode.Line:
                    case PrimitiveAssembleMode.LineTriangle:
                        Rasterize<LinePrimitive, Line>(_linePrimitives.GetPointer(_linePrimitives.Count - newPrimitiveCount), newPrimitiveCount, _rasterizedFragments.GetPointer(_rasterizedFragments.Count - newPrimitiveCount));
                        break;
                    case PrimitiveAssembleMode.Triangle:
                        Rasterize<TrianglePrimitive, Triangle>(_trianglePrimitives.GetPointer(_trianglePrimitives.Count - newPrimitiveCount), newPrimitiveCount, _rasterizedFragments.GetPointer(_rasterizedFragments.Count - newPrimitiveCount));
                        break;
                }

                for (int j = _rasterizedFragments.Count - newPrimitiveCount; j < _rasterizedFragments.Count; j++)
                {
                    Fragment* fragment = _rasterizedFragments.GetPointer(j);
                    for (int k = 0; k < fragment->PixelCount; k++)
                    {
                        //Fragment.FragmentData has been tailored to a proper size
                        FragmentInvoker.Invoke((byte*)fragment->FragmentData[k]);
                        FragmentInvoker.ActiveOutputMap.Write(inoutPtr);
                        _renderedColors.Add(inoutPtr->Color);
                    }
                    fragment->FragmentColor = _renderedColors.ArchivePointer();
                }

                #endregion
            }

            EndRasterize();


            //Octree is so annoying
            //TODO: View frustum clip, triangle clip, pixel clip
            //Clipping();

            //This is not the proper way to output, rather for debugging
            for (int i = 0; i < _rasterizedFragments.Count; i++)
            {
                Fragment* fragmentPtr = _rasterizedFragments.GetPointer(i);
                RenderTarget.WritePixel(fragmentPtr->Rasterization, fragmentPtr->PixelCount, fragmentPtr->FragmentColor);
            }

            return RenderTarget;
        }

        private unsafe void SetPerObjectValues(RenderEntity currentEntity)
        {
            *ShaderValue.ObjectToWorld = *currentEntity.Transform.LocalToWorld;
            Mul(ShaderValue.WorldToView, ShaderValue.ObjectToWorld, ShaderValue.ObjectToView);
            Mul(ViewToScreen, ShaderValue.ObjectToView, ShaderValue.ObjectToScreen);

            //TODO Normalize
            //if (!JMath.Approx(ShaderValue.ObjectToView->M44, 1f))
            //    Divide(ShaderValue.ObjectToView, ShaderValue.ObjectToView->M44, ShaderValue.ObjectToView);
        }

        private unsafe void SetPerRenderValues<T>(T camera) where T : ICamera
        {
            *ShaderValue.WorldToView = *camera.WorldToView;
            ShaderValue.Time = CRenderer.CurrentSecond;
            ShaderValue.SinTime = MathF.Sin(ShaderValue.Time);
            ShaderValue.CosTime = MathF.Cos(ShaderValue.Time);
            ShaderValue.SinTime2 += ShaderValue.SinTime2 = ShaderValue.SinTime * ShaderValue.CosTime;
            ShaderValue.CosTime2 = (ShaderValue.CosTime2 += ShaderValue.CosTime2 = ShaderValue.CosTime * ShaderValue.CosTime) - 1;
        }

        private unsafe void Clear()
        {
            RenderTarget.Clear();
            for (int i = 0; i < _linePrimitives.Count; i++)
                PrimitiveHelper.Free(_linePrimitives.GetPointer(i));
            _linePrimitives.Clear();
            for (int i = 0; i < _trianglePrimitives.Count; i++)
                PrimitiveHelper.Free(_trianglePrimitives.GetPointer(i));
            _trianglePrimitives.Clear();
            for (int i = 0; i < _rasterizedFragments.Count; i++)
                _rasterizedFragments[i].Free();
            _rasterizedFragments.Clear();
            _renderedColors.Clear();
        }

        public void Dispose()
        {
            Clear();
            _linePrimitives.Dispose();
            _trianglePrimitives.Dispose();
            _trianglePrimitives.Dispose();
            _rasterizedFragments.Dispose();
            _renderedColors.Dispose();
        }
    }
}