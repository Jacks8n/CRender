﻿using System;
using CRender.IO;
using CUtility.Math;

namespace CRender.Structure
{
    public class RenderBuffer<T> : IRenderBuffer<GenericVector<T>>, IPPMImage where T : unmanaged
    {
        public int ChannelCount { get; private set; }

        public int Width { get; private set; }

        public int Height { get; private set; }

        private GenericVector<T>[] _pixels = null;

        private bool _initialized = false;

        public RenderBuffer() { }

        public RenderBuffer(Vector2Int size, int channelCount) : this(size.X, size.Y, channelCount) { }

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

        public void WritePixel(int u, int v, GenericVector<T> pixel)
        {
            _pixels[UVToIndex(u, v)].Write(pixel);
        }

        public void WritePixel(Vector2Int[] uvs, GenericVector<T> pixel)
        {
            for (int i = 0; i < uvs.Length; i++)
                _pixels[UVToIndex(uvs[i].X, uvs[i].Y)].Write(pixel);
        }

        public unsafe void WritePixel(Vector2Int* uvs, int pixelCount, GenericVector<T> color)
        {
            for (int i = 0; i < pixelCount; i++)
                _pixels[UVToIndex(uvs[i].X, uvs[i].Y)].Write(color);
        }

        public unsafe void WritePixel(Vector2Int* uvs, int pixelCount, Vector4* colors)
        {
            for (int i = 0; i < pixelCount; i++)
                _pixels[UVToIndex(uvs[i].X, uvs[i].Y)].Write(colors[i]);
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
            return (Height - v - 1) * Width + u;
        }

        public GenericVector<float> GetColorAt(int u, int v)
        {
            if (typeof(T) == typeof(float))
                return (GenericVector<float>)(object)_pixels[UVToIndex(u, v)];
            throw new Exception();
        }
    }
}
