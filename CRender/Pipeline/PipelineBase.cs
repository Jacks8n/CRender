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
        private int[] _triangleIndices;

        private RenderBuffer<float> _mainTexture;

        private Sampler_Point _sampler = new Sampler_Point(SamplerRepeat_Repeat.Instance, SamplerRepeat_Repeat.Instance);

        private Matrix4x4 _matrixObjectToWorld;

        private Matrix4x4 _matrixWorldToView;

        private Matrix4x4 _matrixObjectToView;

        private Vector2 _bufferSizeF;

        private Vector2Int _bufferSize;

        private RenderBuffer<float> _renderTarget = new RenderBuffer<float>();

        #region Application

        public RenderBuffer<float> Draw(RenderEntity[] entities, ICamera camera)
        {
            _matrixWorldToView = camera.WorldToView;
            _bufferSize = CRenderSettings.RenderSize;
            _bufferSizeF = (Vector2)_bufferSize;
            _renderTarget.Initialize(_bufferSize.X, _bufferSize.Y, channelCount: 4);

            TApp appdata = new TApp();
            TV2F[][] vertexV2FData = new TV2F[entities.Length][];
            IPrimitive[][] primitives = new IPrimitive[entities.Length][];
            for (int i = 0; i < entities.Length; i++)
            {
                RenderEntity instanceCopy = entities[i].GetInstanceToApply();

                _matrixObjectToWorld = instanceCopy.Transform.LocalToWorld;
                _matrixObjectToView = _matrixWorldToView * _matrixObjectToWorld;

                Vector3[] vertices = instanceCopy.Model.Vertices;
                vertexV2FData[i] = new TV2F[vertices.Length];
                primitives[i] = instanceCopy.Model.Primitives;
                for (int j = 0; j < vertices.Length; j++)
                {
                    appdata.AssignAppdata(ref instanceCopy.Model, j);
                    vertexV2FData[i][j] = Vertex(ref appdata);
                }
            }

            //Octree is so annoying
            //Clipping();

            Vector2Int[][] rasterization = Rasterize(vertexV2FData, primitives);

            for (int i = 0; i < rasterization.Length; i++)
                for (int j = 0; j < rasterization[i].Length; j++)
                {
                    Fragment(vertexV2FData[i][j]);
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

        protected Vector2 ViewToScreen(Vector4 vpos) => new Vector2((1 - vpos.Y) * .5f, vpos.Z * .5f);

        protected virtual Vector2Int[][] Rasterize(TV2F[][] vertices, IPrimitive[][] primitives)
        {
            Rasterizer.StartRasterize(_bufferSizeF);
            Vector2Int[][] rasterization = new Vector2Int[vertices.Length][];

            Vector2[] screenCoords = new Vector2[3];
            for (int modelIndex = 0; modelIndex < vertices.Length; modelIndex++)
            {
                for (int i = 0; i < primitives[modelIndex].Length; i++)
                {
                    IPrimitive primitive = primitives[modelIndex][i];

                    for (int j = 0; j < primitive.Count; j++)
                        screenCoords[j] = ViewToScreen(vertices[modelIndex][primitive.Indices[j]].Vertex_VOut);
                    Rasterizer.SetPoints(screenCoords);

                    switch (primitive.Count)
                    {
                        case 2:
                            Rasterizer.Line();
                            break;
                        default:
                            throw new NotImplementedException("Rasterization for this kind of primitive is not supported");
                            break;
                    }
                    rasterization[modelIndex] = Rasterizer.ContriveResult();
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