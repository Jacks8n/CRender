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
    public partial class Pipeline : IPipeline
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

        #region Application

        public unsafe RenderBuffer<float> Draw(RenderEntity[] entities, ICamera camera)
        {
            RenderTarget.Clear();
            _rasterizedFragments.Clear();

            int entityCount = entities.Length;
            int* primitiveCounts = stackalloc int[entityCount];

            BeginRasterize();
            *ShaderValue.WorldToView = *camera.WorldToView;
            ShaderValue.Time = CRenderer.CurrentSecond;
            ShaderValue.SinTime = MathF.Sin(ShaderValue.Time);
            for (int i = 0; i < entityCount; i++)
            {
                RenderEntity currentEntity = entities[i];
                Material material = currentEntity.Material ?? DEFAULT_MATERIAL;
                Vector4[] vertices = currentEntity.Model.Vertices;
                IPrimitive[] primitives = currentEntity.Model.Primitives;

                *ShaderValue.ObjectToWorld = *currentEntity.Transform.LocalToWorld;
                Mul(ShaderValue.WorldToView, ShaderValue.ObjectToWorld, ShaderValue.ObjectToView);

                ShaderInvoker<IVertexShader>.ChangeActiveShader(material.ShaderType, material.Shader);

                int vertexCount = vertices.Length;
                Vector2* coordsOutput = stackalloc Vector2[vertexCount];
                Vector4* vertexOutput = stackalloc Vector4[vertexCount];

                for (int j = 0; j < vertexCount; j++)
                {
                    ShaderInOutMap outputMap = ShaderInvoker<IVertexShader>.Invoke(j, vertices);
                    vertexOutput[j] = *outputMap.VertexPtr;
                    coordsOutput[j] = ViewToScreen(*outputMap.VertexPtr);
                }

                primitiveCounts[i] = primitives.Length;
                for (int j = 0; j < primitives.Length; j++)
                    Rasterize(coordsOutput, primitives[j], _rasterizedFragments.GetPtr(_rasterizedFragments.Count + j));
                _rasterizedFragments.AddEmpty(primitives.Length);
            }
            EndRasterize();

            //Octree is so annoying
            //TODO: View frustum clip, triangle clip, pixel clip
            //Clipping();

            //Interpolation

            //This is not the proper way to output, just to check result as soon as possible
            GenericVector<float> white = new GenericVector<float>(3) { 1, 1, 1 };
            int fragmentIndex = 0;
            for (int i = 0; i < entityCount; i++)
                for (int j = 0; j < primitiveCounts[i]; j++)
                {
                    RenderTarget.WritePixel(_rasterizedFragments[fragmentIndex].Rasterization, _rasterizedFragments[fragmentIndex].PixelCount, white);
                    fragmentIndex++;
                }

            return RenderTarget;
        }

        #endregion
    }
}