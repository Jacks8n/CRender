using System;
using CUtility.Extension;
using CUtility.Math;

namespace CRender.Pipeline
{
    public unsafe struct Fragment
    {
        public int PixelCount;

        public Vector2Int* Rasterization;

        public float** FragmentData;

        public Vector4* FragmentColor;

        public void Free()
        {
            MarshalExtension.Free(Rasterization);
            Rasterization = null;
            MarshalExtension.Free(FragmentColor);
            FragmentColor = null;
            if (FragmentData != null)
            {
                for (int i = 0; i < PixelCount; i++)
                    MarshalExtension.Free(FragmentData[i]);
                MarshalExtension.Free(FragmentData);
                FragmentData = null;
            }
        }
    }
}
