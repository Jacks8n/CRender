using System;
using CUtility.Extension;
using CUtility.Math;

namespace CRender.Pipeline
{
    public unsafe struct Fragment
    {
        public int PixelCount;

        public Vector2Int* Rasterization;

        public float* FragmentData;

        public void Free()
        {
            MarshalExt.Free(Rasterization);
            MarshalExt.Free(FragmentData);
        }
    }
}
