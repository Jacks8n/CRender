using static CUtility.Extension.MarshalExt;

namespace CUtility.Collection
{
    public unsafe partial class UnsafeArray<T>
    {
        public void Resize(int length0)
        {
            Resize(length0, lengthPtrLength: 1, dimension: 1);
        }

        public void Resize(int length0, int length1)
        {
            if (length0 < 0 || length1 < 0)
                return;

            if (Resize(length0 * length1, lengthPtrLength: 3, 2))
            {
                _lengthPtr[1] = length0;
                _lengthPtr[2] = length1;
            }
        }

        private bool Resize(int itemPtrLength, int lengthPtrLength, int dimension)
        {
            if (itemPtrLength < 0)
                return false;

            if (_lengthPtr == null)
            {
                _lengthPtr = Alloc<int>(lengthPtrLength);
                Dimension = dimension;
            }
            else if (Dimension != dimension)
            {
                _lengthPtr = ReAlloc(_lengthPtr, lengthPtrLength);
                Dimension = dimension;
            }
            else if (itemPtrLength == Count)
                return false;

            if (itemPtrLength == 0)
            {
                if (_itemsPtr != null)
                {
                    Free(_itemsPtr);
                    _itemsPtr = null;
                }
            }
            else if (_itemsPtr != null)
                _itemsPtr = ReAlloc(_itemsPtr, itemPtrLength);
            else
                _itemsPtr = Alloc<T>(itemPtrLength);

            _lengthPtr[0] = itemPtrLength;
            return true;
        }
    }
}
