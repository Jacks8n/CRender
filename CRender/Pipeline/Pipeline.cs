using System;
using System.Collections.Generic;
using CUtility.Math;
using CRender.Sampler;
using CRender.Structure;

using static CRender.Pipeline.ShaderValue;

namespace CRender.Pipeline
{
    public partial class Pipeline : IPipeline
    {
        public RenderBuffer<float> RenderTarget { get; }

        #region TODO

        //TODO
        private readonly RenderBuffer<float> _mainTexture;

        private readonly Sampler_Point _sampler = new Sampler_Point(SamplerRepeat_Repeat.Instance, SamplerRepeat_Repeat.Instance);

        #endregion

        private readonly Vector2 _bufferSizeF;

        private readonly Vector2Int _bufferSize;

        private readonly Material _currentMaterial;

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
            WorldToView = camera.WorldToView;

            Vector2** screenCoords = stackalloc Vector2*[entities.Length];
            Vector2Int** rasterization = stackalloc Vector2Int*[entities.Length];

            BeginRasterize();
            for (int i = 0; i < entities.Length; i++)
            {
                RenderEntity instanceCopy = entities[i].GetInstanceToApply();

                #region Vertex

                ObjectToWorld = instanceCopy.Transform.LocalToWorld;
                ObjectToView = WorldToView * ObjectToWorld;



                #endregion

                int vertexCount = instanceCopy.Model.Vertices.Length;
                Vector2* coordsOutput = stackalloc Vector2[vertexCount];

                Vector3[] vertices = instanceCopy.Model.Vertices;
                Material material = instanceCopy.Material;

                for (int j = 0; j < vertexCount; j++)
                {
                    screenCoordOutput[j] = ViewToScreen(v2f.Vertex_VOut);
                }
                screenCoords[i] = coordsOutput;
            }
            EndRasterize();

            //Octree is so annoying
            //Clipping();

            GenericVector<float> whiteColor = new GenericVector<float>(3) { 1, 1, 1 };
            //Model
            for (int i = 0; i < rasterization.Length; i++)
                //Primitive
                for (int j = 0; j < rasterization[i].Length; j++)
                    //PixelPos
                    for (int k = 0; k < rasterization[i][j].Length; k++)
                    {
                        RenderTarget.WritePixel(rasterization[i][j][k].X, rasterization[i][j][k].Y, whiteColor);
                    }

            return RenderTarget;
        }

        #endregion
    }
}