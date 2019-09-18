using System;
using System.Collections.Generic;
using CRender.Sampler;
using CRender.Structure;
using CShader;
using CUtility.Collection;
using CUtility.Math;

using static CUtility.Math.Matrix4x4;

using VertexInvoker = CShader.ShaderInvoker<CShader.IVertexShader>;
using FragmentInvoker = CShader.ShaderInvoker<CShader.IFragmentShader>;

namespace CRender.Pipeline
{
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

        private readonly UnsafeList<Fragment> _rasterizedFragments = new UnsafeList<Fragment>();

        private readonly UnsafeList<Vector4> _renderedColors = new UnsafeList<Vector4>();

        public Pipeline()
        {
            _bufferSize = CRenderSettings.Resolution;
            _bufferSizeF = (Vector2)_bufferSize;
            RenderTarget = new RenderBuffer<float>(_bufferSize.X, _bufferSize.Y, channelCount: 4);
        }

        public unsafe RenderBuffer<float> Draw(RenderEntity[] entities, ICamera camera)
        {
            Clear();

            int entityCount = entities.Length;
            int* primitiveCounts = stackalloc int[entityCount];

            BeginRasterize();
            SetPerRenderValues(camera);
            for (int i = 0; i < entityCount; i++)
            {
                RenderEntity currentEntity = entities[i];
                SetPerObjectValues(currentEntity);

                Material material = currentEntity.Material ?? DEFAULT_MATERIAL;

                #region Vertex Stage

                //Prepare vertex shader
                VertexInvoker.ChangeActiveShader(material.ShaderType, material.Shader);

                //Invoke vertex shader
                Model model = currentEntity.Model;
                int vertexCount = model.Vertices.Length;
                Vector2* coordsOutput = stackalloc Vector2[vertexCount];
                for (int j = 0; j < vertexCount; j++)
                {
                    VertexInvoker.Invoke(model.ReadVertexData(j));
                    coordsOutput[j] = *(Vector2*)VertexInvoker.OutputLayoutPtr->VertexPtr;
                }

                #endregion

                #region Fragment Stage

                FragmentInvoker.ChangeActiveShader(material.ShaderType, material.Shader);

                IPrimitive[] primitives = model.Primitives;
                primitiveCounts[i] = primitives.Length;
                _rasterizedFragments.AddEmpty(primitives.Length);
                for (int j = primitives.Length; j > 0; j--)
                    Rasterize(coordsOutput, model.VerticesData, model.VerticesDataCount, primitives[j - 1], _rasterizedFragments.GetPointer(_rasterizedFragments.Count - j));

                for (int j = _rasterizedFragments.Count - primitives.Length; j < _rasterizedFragments.Count; j++)
                {
                    Fragment* fragment = _rasterizedFragments.GetPointer(j);
                    for (int k = 0; k < fragment->PixelCount; k++)
                    {
                        FragmentInvoker.Invoke(fragment->FragmentData[k]);
                        _renderedColors.Add(*FragmentInvoker.OutputLayoutPtr->ColorPtr);
                    }
                    fragment->FragmentColor = _renderedColors.ArchivePointer();
                }

                #endregion
            }
            EndRasterize();


            //Octree is so annoying
            //TODO: View frustum clip, triangle clip, pixel clip
            //Clipping();

            //This is not the proper way to output, rather for development efficiency
            int fragmentIndex = 0;
            for (int i = 0; i < entityCount; i++)
                for (int j = 0; j < primitiveCounts[i]; j++)
                {
                    Fragment fragment = _rasterizedFragments[fragmentIndex++];
                    RenderTarget.WritePixel(fragment.Rasterization, fragment.PixelCount, fragment.FragmentColor);
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
            ShaderValue.SinTime2 = MathF.Sin(ShaderValue.Time * 2f);
        }

        private void Clear()
        {
            RenderTarget.Clear();
            for (int i = 0; i < _rasterizedFragments.Count; i++)
                _rasterizedFragments[i].Free();
            _rasterizedFragments.Clear();
        }

        public void Dispose()
        {
            for (int i = 0; i < _rasterizedFragments.Count; i++)
                _rasterizedFragments[i].Free();
        }
    }
}