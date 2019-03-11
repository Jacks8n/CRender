namespace CRender.Structure
{
    public class CharRenderBuffer<T> where T : unmanaged
    {
        public int Width => _renderBuffer.Width;

        public int Height => _renderBuffer.Height;

        private static readonly char[] COLOR_CHAR = { ' ', '.', '-', '+', '*', '#', '@' };

        private static readonly int COLOR_CHAR_COUNT = COLOR_CHAR.Length;

        private readonly RenderBuffer<T> _renderBuffer;

        private readonly char[] _colorChars;

        public CharRenderBuffer(RenderBuffer<T> renderBuffer)
        {
            _renderBuffer = renderBuffer;
            _colorChars = new char[renderBuffer.Width * renderBuffer.Height];
        }

        public char[] CalculateColorChars()
        {
            if (typeof(T) == typeof(float))
            {
                int charIndex = 0;
                for (int v = 0; v < _renderBuffer.Height; v++)
                    for (int u = 0; u < _renderBuffer.Width; u++)
                        _colorChars[charIndex++] = GetColorChar((GenericVector<float>)(object)_renderBuffer[u][v]);
            }
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
