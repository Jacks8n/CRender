using System;

namespace CRender.Structure
{
    public class RenderBuffer<T> : IRenderBuffer<GenericVector<T>> where T : unmanaged
    {
        public int ChannelCount { get; private set; }

        public int Width { get; private set; }

        public int Height { get; private set; }

        private GenericVector<T>[] _pixels = null;

        private bool _initialized = false;

        public RenderBuffer() { }

        public RenderBuffer(int width, int height, int channelCount) => Initialize(width, height, channelCount);

        public void Initialize(int width, int height, int channelCount)
        {
            if (_initialized)
                throw new Exception("RenderBuffer has initialized");

            Width = width;
            Height = height;
            _pixels = new GenericVector<T>[width * height];

            for (int i = 0; i < _pixels.Length; i++)
                _pixels[i] = new GenericVector<T>(channelCount);

            _initialized = true;
        }

        public ref GenericVector<T> GetPixel(int u, int v)
        {
            return ref _pixels[UVToIndex(u, v)];
        }

        public virtual void WritePixel(int u, int v, GenericVector<T> pixel)
        {
            _pixels[UVToIndex(u, v)].Write(pixel);
        }

        public GenericVector<T>[] GetRenderBuffer()
        {
            return _pixels;
        }

        public void Clear()
        {
            for (int i = 0; i < _pixels.Length; i++)
                _pixels[i].Clear();
        }

        private int UVToIndex(int u, int v)
        {
            return u + v * Width;
        }
    }
}
