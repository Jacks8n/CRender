using System;
using CUtility.Math;

using static CUtility.Extension.MarshalExt;

namespace CRender.Pipeline
{
    public partial class Pipeline
    {
        private unsafe struct Fragment : IDisposable
        {
            public Vector2Int* Rasterization;

            public int PixelCount;

            public void Initialize(int size)
            {
                if (PixelCount == size)
                    return;
                if (PixelCount == 0)
                    Rasterization = Alloc<Vector2Int>(size);
                else
                    ReAlloc(Rasterization, size);
                PixelCount = size;
            }

            void IDisposable.Dispose()
            {
                Free(Rasterization);
            }
        }
    }
}
