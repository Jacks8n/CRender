using CUtility.Math;

namespace CRender.Structure
{
    public class CharRenderBuffer<T> : IRenderBuffer<char> where T : unmanaged
    {
        public int Width => _targetBuffer.Width;

        public int Height => _targetBuffer.Height;

        private static readonly char[] COLOR_CHAR = { ' ', '.', '-', '+', '*', '#', '@' };

        private static readonly int COLOR_CHAR_COUNT = COLOR_CHAR.Length;

        private readonly IRenderBuffer<GenericVector<T>> _targetBuffer;

        private readonly GenericVector<T>[] _targetBufferPixels;

        private readonly char[] _colorChars;

        public CharRenderBuffer(IRenderBuffer<GenericVector<T>> targetBuffer)
        {
            _targetBuffer = targetBuffer;
            _targetBufferPixels = targetBuffer.GetRenderBuffer();
            _colorChars = new char[_targetBufferPixels.Length];
        }

        public char[] GetRenderBuffer()
        {
            if (typeof(T) == typeof(float))
                for (int i = 0; i < _targetBufferPixels.Length; i++)
                    _colorChars[i] = GetColorChar((GenericVector<float>)(object)_targetBufferPixels[i]);

            return _colorChars;
        }

        private static char GetColorChar(GenericVector<float> color)
        {
            int index;
            switch (color.Length)
            {
                case 3:
                    index = (int)((color.R * .299f + color.G * .587f + color.B * .114f) * COLOR_CHAR_COUNT);
                    break;
                default:
                    float sum = 0;
                    for (int i = 0; i < color.Length; i++)
                        sum += color[i];
                    index = (int)(sum / color.Length * COLOR_CHAR_COUNT);
                    break;
            }
            return COLOR_CHAR[index == COLOR_CHAR_COUNT ? index - 1 : index];
        }
    }
}
