namespace CRender.Pipeline
{
    public enum InterpolateMode { Linear }

    public unsafe interface IInterpolatable<T> where T : unmanaged
    {
        InterpolateMode Mode { get; }

        T* ValuesPtr { get; }

        int Length { get; }
    }
}
