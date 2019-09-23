using System;
using CRender.Pipeline.Rasterization;
using CRender.Structure;
using CShader;
using CUtility;
using CUtility.Collection;
using CUtility.Math;

using static CUtility.Extension.MarshalExtension;
using VertexInvoker = CShader.ShaderInvoker<CShader.IVertexShader, CShader.ShaderInOutPatternDefault>;
using FragmentInvoker = CShader.ShaderInvoker<CShader.IFragmentShader, CShader.ShaderInOutPatternDefault>;

namespace CRender.Pipeline
{
    public unsafe partial class Pipeline
    {
        #region Assemble primitve

        private int AssemblePrimitive(Model model, Vector2* screenCoords)
        {
            int[] indices = model.Indices;
            int i, j;
            switch (_assembleMode)
            {
                case PrimitiveAssembleMode.Line:
                    for (i = indices.Length - 2, j = i / 2, _linePrimitives.AddEmpty(j + 1); i > -1; i -= 2, j--)
                        AssemblePrimitve(model, screenCoords, i, _linePrimitives.GetPointer(j));
                    return indices.Length / 2;
                case PrimitiveAssembleMode.Triangle:
                    for (i = indices.Length - 3, j = i / 3, _trianglePrimitives.AddEmpty(j + 1); i > -1; i -= 3, j--)
                        AssemblePrimitve(model, screenCoords, i, _trianglePrimitives.GetPointer(j));
                    return indices.Length / 3;
                case PrimitiveAssembleMode.LineTriangle:
                    for (i = indices.Length - 3, j = i, _linePrimitives.AddEmpty(j + 3); i > -1; i -= 3, j -= 3)
                    {
                        AssemblePrimitve(model, screenCoords, i, _linePrimitives.GetPointer(j));
                        AssemblePrimitve(model, screenCoords, i + 1, _linePrimitives.GetPointer(j + 1));
                        AssemblePrimitve(model, screenCoords, i + 2, _linePrimitives.GetPointer(j + 2));
                    }
                    return indices.Length;
            }
            return 0;
        }

        private void AssemblePrimitve<T>(Model model, Vector2* screenCoords, int startIndex, T* result) where T : unmanaged, IPrimitive
        {
            int vertexCount = result->VertexCount;
            Vector2* coords = Alloc<Vector2>(vertexCount);
            float** primitiveData = (float**)Alloc<IntPtr>(vertexCount);
            int index;
            for (int i = 0; i < vertexCount; i++)
            {
                index = model.Indices[startIndex + i];
                coords[i] = screenCoords[index];
                primitiveData[i] = AllocBytes<float>(FragmentInvoker.ActiveInputMap.InstanceSize);
                FragmentInvoker.ActiveInputMap.MoveFields(model.ReadVerticesDataAsPattern(index), (byte*)primitiveData[i]);
            }
            result->CoordsPtr = coords;
            result->VerticesDataPtr = primitiveData;
            result->VerticesDataCount = FragmentInvoker.ActiveInputMap.SizeInFloatCount;
        }

        #endregion

        #region Rasterization

        private void BeginRasterize()
        {
            Rasterizer.StartRasterize(_bufferSizeF);
        }

        private void Rasterize<TPrimitive, TRasterizer>(in TPrimitive* primitivePtr, int primitiveCount, Fragment* result)
            where TRasterizer : JSingleton<TRasterizer>, IPrimitiveRasterizer<TPrimitive>, new() where TPrimitive : unmanaged, IPrimitive
        {
            while (--primitiveCount >= 0)
            {
                Rasterizer.Rasterize<TRasterizer, TPrimitive>(primitivePtr + primitiveCount);
                Rasterizer.ContriveResult(result + primitiveCount);
            }
        }

        private void EndRasterize()
        {
            Rasterizer.EndRasterize();
        }

        #endregion
    }
}
