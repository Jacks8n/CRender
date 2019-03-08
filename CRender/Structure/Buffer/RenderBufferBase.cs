using System;

namespace CRender.Structure
{
    public class RenderBuffer<T> where T : unmanaged
    {
        public int ChannelCount => _pixels[0][0].Length;

        public int Width => _pixels.Length;

        public int Height => _pixels[0].Length;

        public GenericVector<T>[] this[int u] => _pixels[u];

        private GenericVector<T>[][] _pixels = null;

        public RenderBuffer() { }

        public RenderBuffer(int width, int height, int channelCount) => Initialize(width, height, channelCount);

        public void Initialize(int width, int height, int channelCount)
        {
            if (_pixels == null || Width != width || Height != height || ChannelCount != channelCount)
            {
                _pixels = new GenericVector<T>[width][];

                for (int i = 0; i < width; i++)
                {
                    _pixels[i] = new GenericVector<T>[height];
                    for (int j = 0; j < height; j++)
                        _pixels[i][j] = new GenericVector<T>(channelCount);
                }
            }
            else
                for (int i = 0; i < width; i++)
                    for (int j = 0; j < height; j++)
                        for (int k = 0; k < channelCount; k++)
                            _pixels[i][j][k] = default;
        }
        public virtual void WritePixel(int u, int v, GenericVector<T> pixel)
        {
            _pixels[u][v].Write(pixel);
        }
    }
}
