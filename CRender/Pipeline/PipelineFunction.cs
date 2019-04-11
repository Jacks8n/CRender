using System;
using CRender.Structure;
using CUtility.Math;

using static CRender.Pipeline.ShaderValue;

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
        /// Returns the number of result pixels
        /// </summary>
        /// <param name="modelV2Fs"><typeparamref name="TV2F"/> of every vertex</param>
        /// <param name="screenCoords">Screen coordinate of every vertex</param>
        /// <param name="result">Array to store rasterization result</param>
        private unsafe int Rasterize(in Vector2* screenCoords, in IPrimitive primitive, Vector2Int* result)
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
