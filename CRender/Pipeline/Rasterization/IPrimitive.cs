using CUtility.Math;
using CUtility.Extension;

namespace CRender.Pipeline.Rasterization
{
    public unsafe interface IPrimitive
    {
        int VertexCount { get; }

        Vector2* CoordsPtr { get; set; }

        float** VerticesDataPtr { get; set; }

        int VerticesDataCount { get; set; }
    }

    public unsafe static class PrimitiveHelper
    {
        public static void Free<T>(T* primitivePtr) where T : unmanaged, IPrimitive
        {
            MarshalExtension.Free(primitivePtr->CoordsPtr);
            for (int i = 0; i < primitivePtr->VertexCount; i++)
                MarshalExtension.Free(primitivePtr->VerticesDataPtr[i]);
            MarshalExtension.Free(primitivePtr->VerticesDataPtr);
        }
    }
}
