using System;
using System.Collections.Generic;
using CRender.Sampler;
using CRender.Structure;
using CShader;
using CUtility.Math;

namespace CRender.Pipeline
{
    public partial class Pipeline : IPipeline
    {
        public RenderBuffer<float> RenderTarget { get; }

        private static readonly IMaterial DEFAULT_MATERIAL = new Material<ShaderDefault>(ShaderDefault.Instance);

        private readonly Vector2 _bufferSizeF;

        private readonly Vector2Int _bufferSize;

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

            int entityCount = entities.Length;
            Fragment** rasterizedFragments = stackalloc Fragment*[entityCount];
            int* primitiveCounts = stackalloc int[entityCount];

            BeginRasterize();
            ShaderValue.WorldToView = camera.WorldToView;
            for (int i = 0; i < entityCount; i++)
            {
                RenderEntity instanceCopy = entities[i].GetInstanceToApply();
                IMaterial material = instanceCopy.Material ?? DEFAULT_MATERIAL;
                Vector4[] vertices = instanceCopy.Model.Vertices;
                IPrimitive[] primitives = instanceCopy.Model.Primitives;
                int vertexCount = vertices.Length;

                ShaderValue.ObjectToWorld = instanceCopy.Transform.LocalToWorld;
                ShaderValue.ObjectToView = ShaderValue.WorldToView * ShaderValue.ObjectToWorld;

                ShaderInvoker<IVertexShader>.ChangeActiveShader(material.ShaderType, material.Shader);

                Vector2* coordsOutput = stackalloc Vector2[vertexCount];
                Vector4* vertexOutput = stackalloc Vector4[vertexCount];

                for (int j = 0; j < vertexCount; j++)
                {
                    ShaderInOutMap outputMap = ShaderInvoker<IVertexShader>.Invoke(j, vertices);
                    vertexOutput[j] = *outputMap.VertexPtr;
                    coordsOutput[j] = ViewToScreen(*outputMap.VertexPtr);
                }

                Fragment* fragment = stackalloc Fragment[primitives.Length];
                primitiveCounts[i] = primitives.Length;
                for (int j = 0; j < primitives.Length; j++)
                {
                    fragment[j].PixelCount = Rasterize(coordsOutput, primitives[j], ref fragment[j].Rasterization);
                }
                rasterizedFragments[i] = fragment;
            }
            //Octree is so annoying
            //TODO: View frustum clip, triangle clip, pixel clip
            //Clipping();

            //This is not the proper way to output, just to check result as soon as possible
            GenericVector<float> white = new GenericVector<float>(3) { 1, 1, 1 };
            for (int i = 0; i < entityCount; i++)
                for (int j = 0; j < primitiveCounts[i]; j++)
                {
                    RenderTarget.WritePixel(rasterizedFragments[i][j].Rasterization, rasterizedFragments[i][j].PixelCount, white);
                }

            EndRasterize();
            return RenderTarget;
        }

        #endregion
    }
}