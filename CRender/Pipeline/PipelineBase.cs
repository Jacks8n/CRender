using System;
using System.Collections.Generic;
using CRender.Math;
using CRender.Sampler;
using CRender.Structure;

using static CRender.Pipeline.ShaderValue;

namespace CRender.Pipeline
{
    public partial class PipelineBase
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

        public PipelineBase()
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

            TV2F** v2fData = stackalloc TV2F*[entities.Length];
            Vector2** screenCoords = stackalloc Vector2*[entities.Length];
            Vector2Int** rasterization = stackalloc Vector2Int*[entities.Length];

            BeginRasterize();
            for (int i = 0; i < entities.Length; i++)
            {
                RenderEntity instanceCopy = entities[i].GetInstanceToApply();

                int vertexCount = instanceCopy.Model.Vertices.Length;
                TV2F* v2fOutput = stackalloc TV2F[vertexCount];
                Vector2* coordsOutput = stackalloc Vector2[vertexCount];

                ProcessGeometryStage(instanceCopy, v2fOutput, coordsOutput);
                v2fData[i] = v2fOutput;
                screenCoords[i] = coordsOutput;
            }
            EndRasterize();

            //Octree is so annoying
            //Clipping();

            Vector2Int[][][] rasterization = Rasterize(v2fData, screenCoords, primitives);

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

        #region GeometryProcessing

        /// <summary>
        /// TODO
        /// </summary>
        [Obsolete]
        private void Clipping()
        {
            throw new NotImplementedException("Maybe you need to check your vision");
        }

        #endregion

        #region Rasterization

        private Vector2 ViewToScreen(Vector4 vpos) => new Vector2(vpos.Y * .5f + .5f, vpos.Z * .5f + .5f);

        private void BeginRasterize()
        {
            Rasterizer.StartRasterize(_bufferSizeF);
        }

        /// <summary>
        /// Returns the number of result pixels
        /// </summary>
        /// <param name="modelV2Fs"><typeparamref name="TV2F"/> of every vertex</param>
        /// <param name="screenCoords">Screen coordinate of every vertex</param>
        /// <param name="result">Array to store rasterization result</param>
        private unsafe int Rasterize(in TV2F* modelV2Fs, in Vector2* screenCoords, in IPrimitive primitive, Vector2Int* result)
        {
            Vector2* primitiveCoords = stackalloc Vector2[primitive.Count];

            for (int i = 0; i < primitive.Count; i++)
                primitiveCoords[i] = screenCoords[primitive.Indices[i]];
            Rasterizer.SetPoints(primitiveCoords);

            switch (primitive.Count)
            {
                case 2:
                    Rasterizer.Line();
                    break;
                case 3:
                    Rasterizer.Triangle();
                    break;
                default:
                    throw new NotImplementedException("Rasterization for this kind of primitive is not supported");
                    break;
            }
            return Rasterizer.ContriveResultPtr(ref result);
        }

        private void EndRasterize()
        {
            Rasterizer.EndRasterize();
        }

        #endregion
    }
}