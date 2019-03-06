namespace CRender.Pipeline.Structure
{
    public unsafe interface IInterpolatable<T> where T : unmanaged, IInterpolatable<T>
    {
        void Minus(T* other);

        void Add(T* other);

        void Multiply(float scale);
        
        /// <param name="leftOrRight">-1 for left, 1 for right</param>
        T GetHorizontalComponent(int leftOrRight);
    }
}