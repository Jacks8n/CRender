using System;
using CUtility.Math;

using static CUtility.Extension.MarshalExt;

namespace CRender.Pipeline
{
    public partial class Pipeline
    {
        private unsafe struct Fragment
        {
            public Vector2Int* Rasterization;

            public byte** PixelData;

            public int PixelCount;

            private byte* _totalPixelData;

            public void Initialize(int size, int dataSizePerPixel)
            {
                if (PixelCount == size)
                    return;
                if (Rasterization == null)
                    Rasterization = Alloc<Vector2Int>(size);
                else
                    Rasterization = ReAlloc(Rasterization, size);
                if (PixelData == null)
                {
                    _totalPixelData = AllocBytes<byte>(dataSizePerPixel * size);
                    //Byte** can't be used as unmanged type argument in .net core 2.2
                    //Same reason for the above one
                    PixelData = (byte**)AllocBytes<byte>(size);
                }
                else
                {
                    _totalPixelData = ReAlloc(_totalPixelData, dataSizePerPixel * size);
                    PixelData = (byte**)ReAlloc((byte*)PixelData, size);
                }

                PixelCount = size;
                byte* current = _totalPixelData;
                for (int i = 0; i < size; i++)
                {
                    PixelData[i] = current;
                    current += dataSizePerPixel;
                }
            }

            public void FreeData()
            {
                Free(Rasterization);
                Free(_totalPixelData);
                Free(PixelData);
            }
        }
    }
}
