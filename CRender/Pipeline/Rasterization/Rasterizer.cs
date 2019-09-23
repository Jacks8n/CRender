using System;
using CUtility;
using CUtility.StupidWrapper;
using CUtility.Collection;
using CUtility.Math;
using CRender.Structure;
using System.Runtime.CompilerServices;

using static CUtility.Extension.MarshalExtension;

namespace CRender.Pipeline.Rasterization
{
    /// <summary>
    /// Rasterize primitives, based on the resolution passed in <see cref="StartRasterize(Vector2Int)"/>
    /// </summary>
    public static unsafe class Rasterizer
    {
        public struct RasterizerEntry
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void OutputRasterization(Vector2Int pixelCoord)
            {
                Rasterizer.OutputRasterization(pixelCoord);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void OutputRasterization(Vector2Int pixelCoord, UnsafeList<float> fragmentData)
            {
                Rasterizer.OutputRasterization(pixelCoord, fragmentData);
            }
        }

        public static int RasterizedPixelCount => _pixelCoords.Count;

        private static bool _rasterizing = false;

        private static Vector2 _resolution = Vector2.Zero;

        private static Vector2 _pixelSize = Vector2.One;

        private static readonly UnsafeList<Vector2Int> _pixelCoords = new UnsafeList<Vector2Int>();

        private static readonly UnsafeList<FloatPointer> _fragmentData = new UnsafeList<FloatPointer>();

        static Rasterizer()
        {
            AppDomain.CurrentDomain.DomainUnload += (sender, args) =>
            {
                if (_rasterizing)
                    EndRasterize();
            };
        }

        public static void StartRasterize(Vector2 resolution)
        {
            if (_rasterizing)
                throw new Exception("Rasterization has begun");
            _rasterizing = true;

            _resolution = resolution;
            _pixelSize.X = 1f / _resolution.X;
            _pixelSize.Y = 1f / _resolution.Y;
        }

        public static void Rasterize<TRasterizer, TPrimitive>(TPrimitive* primitivePtr) where TRasterizer : JSingleton<TRasterizer>, IPrimitiveRasterizer<TPrimitive>, new() where TPrimitive : unmanaged, IPrimitive
        {
            TRasterizer rasterizer = JSingleton<TRasterizer>.Instance;
            rasterizer.Resolution = _resolution;
            rasterizer.Rasterize(primitivePtr);
        }

        public static void EndRasterize()
        {
            if (!_rasterizing)
                throw new Exception("Rasterization hasn't begun");
            _rasterizing = false;

            Clear();
        }

        public static void ContriveResult(Fragment* result)
        {
            result->PixelCount = RasterizedPixelCount;
            result->Rasterization = _pixelCoords.ArchivePointer();
            result->FragmentData = _fragmentData.Count > 0 ? (float**)_fragmentData.ArchivePointer() : null;
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

        private static void OutputRasterization(Vector2Int pixelCoord)
        {
            _pixelCoords.Add(pixelCoord);
        }

        private static void OutputRasterization(Vector2Int pixelCoord, UnsafeList<float> fragmentData)
        {
            _pixelCoords.Add(pixelCoord);
            _fragmentData.Add(fragmentData.ArchivePointer());
        }
    }
}
