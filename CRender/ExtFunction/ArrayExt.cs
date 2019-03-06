namespace CRender
{
    public static class ArrayExt
    {
        public static T[] GetEmptyArray<T>(this T[] arr)
        {
            return new T[arr.Length];
        }

        public static T[] GetCopy<T>(this T[] arr)
        {
            T[] copy = arr.GetEmptyArray();
            arr.CopyTo(arr, 0);
            return copy;
        }
    }
}
