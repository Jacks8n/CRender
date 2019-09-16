﻿using System;
using CRender.Structure;
using CShader;
using CUtility.Collection;
using CUtility.Math;

using static CUtility.Extension.MarshalExtension;

namespace CRender.Pipeline
{
    public unsafe partial class Pipeline
    {
        #region Rasterization

        private void BeginRasterize()
        {
            Rasterizer.StartRasterize(_bufferSizeF);
        }

        private void Rasterize(in Vector2* screenCoords, in GenericVector<float>[] verticesData, int verticesDataCount, in IPrimitive primitive, Fragment* result)
        {
            Vector2* coords = stackalloc Vector2[primitive.VertexCount];
            float** primitiveData = stackalloc float*[primitive.VertexCount];

            //TODO not all data need to be interpolated
            for (int i = 0; i < primitive.VertexCount; i++)
            {
                coords[i] = screenCoords[primitive.Indices[i]];
                primitiveData[i] = verticesData[primitive.Indices[i]].ElementsPtr;
            }
            Rasterize(primitive.VertexCount, result, coords, primitiveData, verticesDataCount);
        }

        private static void Rasterize(int vertexCount, Fragment* result, Vector2* coords, in float** verticesData, int verticesDataCount)
        {
            switch (vertexCount)
            {
                case 2:
                    Rasterizer.Line(coords, verticesData, verticesDataCount);
                    break;
                case 3:
                    Rasterizer.Triangle(coords, verticesData, verticesDataCount);
                    break;
                default:
                    throw new NotImplementedException("Rasterization for this kind of primitive is not supported");
                    break;
            }
            Rasterizer.ContriveResult(result);
        }

        private void EndRasterize()
        {
            Rasterizer.EndRasterize();
        }

        #endregion
    }
}
