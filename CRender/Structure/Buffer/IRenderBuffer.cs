namespace CRender.Structure
{
    public interface IRenderBuffer<T>
    {
        int Width { get; }

        int Height { get; }

        T[] GetRenderBuffer();
    }
}
