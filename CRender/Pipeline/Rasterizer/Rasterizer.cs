using System;
using CUtility.StupidWrapper;
using CUtility.Collection;
using CUtility.Math;
using CRender.Structure;

using static CUtility.Extension.MarshalExt;

namespace CRender.Pipeline
{
    /// <summary>
    /// Rasterize primitives, based on the resolution passed in <see cref="StartRasterize(Vector2Int)"/>
    /// </summary>
    public sealed unsafe partial class Rasterizer : IDisposable
    {
        public static int RasterizedPixelCount => _pixelCoords.Count;

        private static bool _initialized = false;

        private static Vector2 _resolution = Vector2.Zero;

        private static Vector2 _pixelSize = Vector2.One;

        private static readonly UnsafeList<Vector2Int> _pixelCoords = new UnsafeList<Vector2Int>();

        private static readonly UnsafeList<float> _fragmentData = new UnsafeList<float>();

        public static void StartRasterize(Vector2 resolution)
        {
            if (_initialized)
                throw new Exception("Rasterization has begun");
            _initialized = true;

            _resolution = resolution;
            _pixelSize.X = 1f / _resolution.X;
            _pixelSize.Y = 1f / _resolution.Y;
        }

        public static void EndRasterize()
        {
            if (!_initialized)
                throw new Exception("Rasterization hasn't begun");
            _initialized = false;

            Clear();
        }

        public static void ContriveResult(Fragment* result)
        {
            result->PixelCount = RasterizedPixelCount;
            result->Rasterization = _pixelCoords.ArchivePointer();
            result->FragmentData = _fragmentData.ArchivePointer();
        }

        public static void ContriveResult(Vector2Int[] output, int start)
        {
            for (int i = 0; i < RasterizedPixelCount; i++)
                output[start + i] = _pixelCoords[i];
            Clear();
        }

        public static Vector2Int[] ContriveResult()
        {
            Vector2Int[] result = new Vector2Int[RasterizedPixelCount];
            ContriveResult(result, 0);
            return result;
        }

        private static void Clear()
        {
            _pixelCoords.Clear();
            _fragmentData.Clear();
        }

        /// <summary>
        /// Store result and shift <see cref="RasterizedPixelCount"/>
        /// </summary>
        private static void OutputRasterization(Vector2Int pixelCoord)
        {
            _pixelCoords.Add(pixelCoord);
        }

        /// <summary>
        /// Store result and shift <see cref="RasterizedPixelCount"/>
        /// </summary>
        private static void OutputRasterization(Vector2Int pixelCoord, UnsafeList<float> fragmentData)
        {
            _pixelCoords.Add(pixelCoord);
            _fragmentData.AddRange(fragmentData);
        }

        void IDisposable.Dispose()
        {
            if (_initialized)
                EndRasterize();
        }
    }
}
