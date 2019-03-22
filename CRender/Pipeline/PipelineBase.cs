using System;
using System.Collections.Generic;
using CRender.Math;
using CRender.Pipeline.Structure;
using CRender.Sampler;
using CRender.Structure;

using static CRender.Pipeline.ShaderValue;

namespace CRender.Pipeline
{
    public class PipelineBase<TApp, TV2F> : IPipeline where TApp : unmanaged, IRenderData_App<TApp> where TV2F : unmanaged, IRenderData_VOut, IRenderData_FIn<TV2F>
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

            IPrimitive[][] primitives = new IPrimitive[entities.Length][];
            TV2F** v2fData = stackalloc TV2F*[entities.Length];
            Vector2** screenCoords = stackalloc Vector2*[entities.Length];
            for (int i = 0; i < entities.Length; i++)
            {
                RenderEntity instanceCopy = entities[i].GetInstanceToApply();

                int vertexCount = instanceCopy.Model.Vertices.Length;
                TV2F* v2fOutput = stackalloc TV2F[vertexCount];
                Vector2* coordsOutput = stackalloc Vector2[vertexCount];

                primitives[i] = ProcessGeometryStage(instanceCopy, v2fOutput, coordsOutput);
                v2fData[i] = v2fOutput;
                screenCoords[i] = coordsOutput;
            }

            //Octree is so annoying
            //Clipping();

            Vector2Int[][][] rasterization = Rasterize(screenCoords, v2fData, primitives);

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

        private unsafe IPrimitive[] ProcessGeometryStage(RenderEntity entity, TV2F* v2fOutput, Vector2* screenCoordOutput)
        {
            ObjectToWorld = entity.Transform.LocalToWorld;
            ObjectToView = WorldToView * ObjectToWorld;

            Vector3[] vertices = entity.Model.Vertices;
            Material material = entity.Material;

            IRenderData_App<TApp> appdata = new TApp();
            for (int i = 0; i < vertices.Length; i++)
            {
                appdata.AssignAppdata(ref entity.Model, i);
                TV2F v2f = material.Shader.Vertex(material.Shader as IVertexShader<TApp, TV2F>, appdata);

                v2fOutput[i] = v2f;
                screenCoordOutput[i] = ViewToScreen(v2f.Vertex_VOut);
            }

            return entity.Model.Primitives;
        }

        #endregion

        #region GeometryProcessing

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

        protected virtual unsafe int Rasterize(in TV2F* modelV2Fs, in Vector2* screenCoords, in IPrimitive[] primitives)
        {
            Rasterizer.StartRasterize(_bufferSizeF);
            Vector2* primitiveCoords = stackalloc Vector2[3];

            for (int primitiveIndex = 0; primitiveIndex < primitives.Length; primitiveIndex++)
            {
                IPrimitive primitive = primitives[primitiveIndex];

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
                Rasterizer.ContriveResult();
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