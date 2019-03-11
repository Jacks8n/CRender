using System;
using System.Collections.Generic;
using CRender.Math;
using CRender.Pipeline.Structure;
using CRender.Sampler;
using CRender.Structure;

namespace CRender.Pipeline
{
    public class PipelineBase<TApp, TV2F>
        where TApp : struct, IRenderData_App<TApp> where TV2F : unmanaged, IRenderData_VOut, IRenderData_FIn<TV2F>
    {
        public RenderBuffer<float> RenderTarget => _renderTarget;

        #region TODO

        //TODO
        private readonly RenderBuffer<float> _mainTexture;

        private readonly Sampler_Point _sampler = new Sampler_Point(SamplerRepeat_Repeat.Instance, SamplerRepeat_Repeat.Instance);

        #endregion

        private readonly Vector2 _bufferSizeF;

        private readonly Vector2Int _bufferSize;

        private readonly RenderBuffer<float> _renderTarget;

        private Matrix4x4 _matrixObjectToWorld;

        private Matrix4x4 _matrixWorldToView;

        private Matrix4x4 _matrixObjectToView;

        public PipelineBase()
        {
            _bufferSize = CRenderSettings.RenderSize;
            _bufferSizeF = (Vector2)_bufferSize;
            _renderTarget = new RenderBuffer<float>(_bufferSize.X, _bufferSize.Y, channelCount: 3);
        }

        #region Application

        public RenderBuffer<float> Draw(RenderEntity[] entities, ICamera camera)
        {
            _renderTarget.Clear();
            _matrixWorldToView = camera.WorldToView;

            TApp appdata = new TApp();
            TV2F[][] vertexV2FData = new TV2F[entities.Length][];
            IPrimitive[][] primitives = new IPrimitive[entities.Length][];
            Vector2[][] screenCoords = new Vector2[entities.Length][];
            for (int i = 0; i < entities.Length; i++)
            {
                RenderEntity instanceCopy = entities[i].GetInstanceToApply();

                _matrixObjectToWorld = instanceCopy.Transform.LocalToWorld;
                _matrixObjectToView = _matrixWorldToView * _matrixObjectToWorld;

                Vector3[] vertices = instanceCopy.Model.Vertices;
                vertexV2FData[i] = new TV2F[vertices.Length];
                screenCoords[i] = new Vector2[vertices.Length];
                primitives[i] = instanceCopy.Model.Primitives;
                for (int j = 0; j < vertices.Length; j++)
                {
                    appdata.AssignAppdata(ref instanceCopy.Model, j);
                    TV2F v2f = Vertex(ref appdata);

                    vertexV2FData[i][j] = v2f;
                    screenCoords[i][j] = ViewToScreen(v2f.Vertex_VOut);
                }
            }

            //Octree is so annoying
            //Clipping();

            Vector2Int[][][] rasterization = Rasterize(screenCoords, vertexV2FData, primitives);

            GenericVector<float> whiteColor = new GenericVector<float>(3) { 1, 1, 1 };
            //Model
            for (int i = 0; i < rasterization.Length; i++)
                //Primitive
                for (int j = 0; j < rasterization[i].Length; j++)
                    //PixelPos
                    for (int k = 0; k < rasterization[i][j].Length; k++)
                    {
                        _renderTarget.WritePixel(rasterization[i][j][k].X, rasterization[i][j][k].Y, whiteColor);
                    }

            return _renderTarget;
        }

        #endregion

        #region GeometryProcessing

        public virtual TV2F Vertex(ref TApp input)
        {
            return new TV2F
            {
                Vertex_VOut = _matrixObjectToView * input.Vertex_App,
                UV_VOut = input.UV_App
            };
        }

        /// <summary>
        /// TODO
        /// </summary>
        [Obsolete]
        protected virtual void Clipping()
        {
            throw new NotImplementedException("Maybe you need to check your vision");
        }

        #endregion

        #region Rasterization

        protected Vector2 ViewToScreen(Vector4 vpos) => new Vector2(vpos.Y * .5f + .5f, vpos.Z * .5f + .5f);

        protected virtual unsafe Vector2Int[][][] Rasterize(Vector2[][] screenCoords, TV2F[][] modelV2Fs, IPrimitive[][] primitives)
        {
            Rasterizer.StartRasterize(_bufferSizeF);
            Vector2Int[][][] rasterization = new Vector2Int[modelV2Fs.Length][][];

            Vector2* primitiveCoords = stackalloc Vector2[3];
            for (int modelIndex = 0; modelIndex < modelV2Fs.Length; modelIndex++)
            {
                rasterization[modelIndex] = new Vector2Int[primitives[modelIndex].Length][];
                for (int primitiveIndex = 0; primitiveIndex < primitives[modelIndex].Length; primitiveIndex++)
                {
                    IPrimitive primitive = primitives[modelIndex][primitiveIndex];

                    for (int j = 0; j < primitive.Count; j++)
                        primitiveCoords[j] = screenCoords[modelIndex][primitive.Indices[j]];
                    Rasterizer.SetPoints(primitiveCoords);

                    switch (primitive.Count)
                    {
                        case 2:
                            Rasterizer.Line();
                            break;
                        default:
                            throw new NotImplementedException("Rasterization for this kind of primitive is not supported");
                            break;
                    }
                    rasterization[modelIndex][primitiveIndex] = Rasterizer.ContriveResult();
                }
            }

            Rasterizer.EndRasterize();
            return rasterization;
        }

        #endregion

        #region PixelProcessing

        public virtual GenericVector<float> Fragment(TV2F input)
        {
            return _sampler.Sample(_mainTexture, input.UV_VOut);
        }

        #endregion
    }
}