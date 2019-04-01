namespace CRender.Pipeline
{
    /// <summary>
    /// Indicates that a series of values need to be interpolated during rasterization
    /// </summary>
    public unsafe interface IInterpolatable<T> where T : unmanaged
    {
        /// <summary>
        /// Pointer to values to be interpolated
        /// </summary>
        T* ValuesPtr { get; }

        /// <summary>
        /// The number of values
        /// </summary>
        int Length { get; }
    }
}
