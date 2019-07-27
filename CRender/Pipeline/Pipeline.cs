using System;
using System.Collections.Generic;
using CRender.Sampler;
using CRender.Structure;
using CShader;
using CUtility.Math;
using CUtility.Collection;

using static CUtility.Math.Matrix4x4;

namespace CRender.Pipeline
{
    public partial class Pipeline : IPipeline, IDisposable
    {
        private static readonly Material DEFAULT_MATERIAL = Material.NewMaterial(ShaderDefault.Instance);

        public RenderBuffer<float> RenderTarget { get; }

        private readonly Vector2 _bufferSizeF;

        private readonly Vector2Int _bufferSize;

        private readonly UnsafeList<Fragment> _rasterizedFragments = new UnsafeList<Fragment>();

        public Pipeline()
        {
            _bufferSize = CRenderSettings.Resolution;
            _bufferSizeF = (Vector2)_bufferSize;
            RenderTarget = new RenderBuffer<float>(_bufferSize.X, _bufferSize.Y, channelCount: 3);
        }

        public unsafe RenderBuffer<float> Draw(RenderEntity[] entities, ICamera camera)
        {
            Clear();

            int entityCount = entities.Length;
            int* primitiveCounts = stackalloc int[entityCount];

            BeginRasterize();
            SetGlobalValues(camera);
            for (int i = 0; i < entityCount; i++)
            {
                RenderEntity currentEntity = entities[i];
                SetObjectValues(currentEntity);

                Material material = currentEntity.Material ?? DEFAULT_MATERIAL;

                #region Vertex Stage

                ShaderInvoker<IVertexShader>.ChangeActiveShader(material.ShaderType, material.Shader);
                ShaderInOutMap vertexInput = ShaderInvoker<IVertexShader>.ActiveInputMap;
                ShaderInOutMap vertexOutput = ShaderInvoker<IVertexShader>.ActiveOutputMap;

                Vector4[] vertices = currentEntity.Model.Vertices;
                Vector3[] normals = currentEntity.Model.Normals;
                int vertexCount = vertices.Length;
                Vector2* coordsOutput = stackalloc Vector2[vertexCount];
                for (int j = 0; j < vertexCount; j++)
                {
                    *vertexInput.VertexPtr = vertices[j];
                    if (vertexInput.NormalPtr != null)
                        *vertexInput.NormalPtr = normals[j];
                    ShaderInvoker<IVertexShader>.Invoke();
                    coordsOutput[j] = ViewToScreen(*vertexOutput.VertexPtr);
                }

                #endregion

                ShaderInvoker<IFragmentShader>.ChangeActiveShader(material.ShaderType, material.Shader);
                ShaderInOutMap fragmentInput = ShaderInvoker<IFragmentShader>.ActiveInputMap;
                ShaderInOutMap fragmentOutput = ShaderInvoker<IFragmentShader>.ActiveOutputMap;

                IPrimitive[] primitives = currentEntity.Model.Primitives;
                primitiveCounts[i] = primitives.Length;
                _rasterizedFragments.EnsureVacant(primitives.Length);
                if (fragmentInput.RequireInterpolation)
                    for (int j = 0; j < primitives.Length; j++)
                        Rasterize(coordsOutput, currentEntity.Model.VerticesData, currentEntity.Model.VerticesDataCount, primitives[j], _rasterizedFragments.GetPointer(_rasterizedFragments.Count + j));
                else
                    for (int j = 0; j < primitives.Length; j++)
                        Rasterize(coordsOutput, primitives[j], _rasterizedFragments.GetPointer(_rasterizedFragments.Count + j));
            }
            EndRasterize();

            //Octree is so annoying
            //TODO: View frustum clip, triangle clip, pixel clip
            //Clipping();

            //TODO: Interpolation

            //This is not the proper way to output, rather for development efficiency
            int fragmentIndex = 0;
            GenericVector<float> white = new GenericVector<float>(3) { 1f, 1f, 1f };
            for (int i = 0; i < entityCount; i++)
                for (int j = 0; j < primitiveCounts[i]; j++)
                {
                    RenderTarget.WritePixel(_rasterizedFragments[fragmentIndex].Rasterization, _rasterizedFragments[fragmentIndex].PixelCount, white);
                    fragmentIndex++;
                }

            return RenderTarget;
        }

        private static unsafe void SetObjectValues(RenderEntity currentEntity)
        {
            *ShaderValue.ObjectToWorld = *currentEntity.Transform.LocalToWorld;
            Mul(ShaderValue.WorldToView, ShaderValue.ObjectToWorld, ShaderValue.ObjectToView);
        }

        private static unsafe void SetGlobalValues(ICamera camera)
        {
            *ShaderValue.WorldToView = *camera.WorldToView;
            ShaderValue.Time = CRenderer.CurrentSecond;
            ShaderValue.SinTime = MathF.Sin(ShaderValue.Time);
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