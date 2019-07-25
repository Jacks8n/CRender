using System;
using CRender.Structure;
using CShader;
using CUtility.Collection;
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
        private unsafe void Rasterize(in Vector2* screenCoords, in GenericVector<float>[] verticesData, int verticesDataCount, in IPrimitive primitive, Fragment* result)
        {
            Vector2* coords = stackalloc Vector2[primitive.VertexCount];
            float** primitiveData = stackalloc float*[3];
            for (int i = 0; i < primitive.VertexCount; i++)
            {
                coords[i] = screenCoords[primitive.Indices[i]];
                primitiveData[i] = verticesData[primitive.Indices[i]].ElementsPtr;
            }
            switch (primitive.VertexCount)
            {
                case 2:
                    Rasterizer.Line(coords, primitiveData, verticesDataCount);
                    break;
                case 3:
                    Rasterizer.Triangle(coords, primitiveData, verticesDataCount);
                    break;
                default:
                    throw new NotImplementedException("Rasterization for this kind of primitive is not supported");
                    break;
            }
            Rasterizer.ContriveResult(result);
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
