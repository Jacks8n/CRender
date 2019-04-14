using CUtility.Math;

namespace CRender.Pipeline
{
    public partial class Pipeline
    {
		private unsafe struct Fragment
        {
            public Vector2Int* Rasterization;

            public int PixelCount;
        }
    }
}
