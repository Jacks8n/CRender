using System;
using CRender.Structure;
using CShader;
using CUtility.Math;

using static CUtility.Extension.MarshalExt;

namespace CRender.Pipeline
{
    public partial class Pipeline
    {
        #region Rasterization

        private Vector2 ViewToScreen(Vector4 vpos) => new Vector2(vpos.Y * .5f + .5f, vpos.Z * .5f + .5f);

        private void BeginRasterize()
        {
            Rasterizer.StartRasterize(_bufferSizeF);
        }

        /// <summary>
        /// Rasterize <paramref name="primitive"/> and stores the result in <paramref name="result"/>
        /// </summary>
        /// <param name="screenCoords">Screen coordinate of every vertex</param>
        private unsafe void Rasterize(in Vector2* screenCoords, in IPrimitive primitive, Fragment* result)
        {
            Vector2* coords = stackalloc Vector2[primitive.Count];
            for (int i = 0; i < primitive.Count; i++)
                coords[i] = screenCoords[primitive.Indices[i]];
            Rasterizer.SetVertices(coords);

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
            result->Initialize(Rasterizer.RasterizeResultLength);
            Rasterizer.ContriveResult(result->Rasterization);
        }

        /// <summary>
        /// This function will also dispose contrived results
        /// </summary>
        private void EndRasterize()
        {
            Rasterizer.EndRasterize();
        }

        #endregion
    }
}
