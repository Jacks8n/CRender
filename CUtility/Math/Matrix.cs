using CUtility.Extension;

using static CUtility.Extension.MarshalExtension;

namespace CUtility.Math
{
    [System.Obsolete("Under development")]
    public unsafe readonly struct Matrix
    {
        public float this[int x, int y] { get => _columnRowPtr[x][y]; set => _columnRowPtr[x][y] = value; }

        public int Column => *(int*)_elementsPtr;

        public int Row => *(int*)(_elementsPtr + 1);

        private readonly float** _columnRowPtr;

        /// <summary>
        /// First two items are <see cref="Column"/> and <see cref="Row"/>
        /// </summary>
        private readonly float* _elementsPtr;

        public Matrix(int column, int row)
        {
            float* tempPtr = _elementsPtr = Alloc<float>(sizeof(int) * 2 / sizeof(float) + column * row);
            ((int*)_elementsPtr)[0] = column;
            ((int*)_elementsPtr)[1] = row;
            _columnRowPtr = (float**)Alloc<byte>(column);
            for (; --column >= 0; tempPtr += row)
                _columnRowPtr[column] = tempPtr;
        }

        public void Free()
        {
            MarshalExtension.Free(_columnRowPtr);
            MarshalExtension.Free(_elementsPtr);
        }
    }
}
