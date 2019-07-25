using System;
using CUtility.Math;

using static CUtility.Extension.MarshalExt;

namespace CRender.Pipeline
{
    public unsafe struct Fragment
    {
        public int PixelCount;

        public Vector2Int* Rasterization;

        public float* FragmentData;

        public void FreeUnmanaged()
        {
            Free(Rasterization);
            Free(FragmentData);
        }
    }
}
