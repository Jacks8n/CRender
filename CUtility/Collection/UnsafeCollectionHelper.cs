namespace CUtility.Collection
{
    public static class UnsafeCollectionHelper<TItem> where TItem : unmanaged
    {
        public static void Add<TCollection>(TCollection l, TCollection r, TCollection result) where TCollection : UnsafeCollection<TItem>
        {
            for (int i = 0; i < l.Count; i++)
                if (typeof(TItem) == typeof(int))
                    result[i] = (TItem)(object)((int)(object)l[i] + (int)(object)r[i]);
                else if (typeof(TItem) == typeof(float))
                    result[i] = (TItem)(object)((float)(object)l[i] + (float)(object)r[i]);
        }

        public static void Minus<TCollection>(TCollection l, TCollection r, TCollection result) where TCollection : UnsafeCollection<TItem>
        {
            for (int i = 0; i < l.Count; i++)
                if (typeof(TItem) == typeof(int))
                    result[i] = (TItem)(object)((int)(object)l[i] - (int)(object)r[i]);
                else if (typeof(TItem) == typeof(float))
                    result[i] = (TItem)(object)((float)(object)l[i] - (float)(object)r[i]);
        }

        public static void Multiply<TCollection>(TCollection l, TCollection r, TCollection result) where TCollection : UnsafeCollection<TItem>
        {
            for (int i = 0; i < l.Count; i++)
                if (typeof(TItem) == typeof(int))
                    result[i] = (TItem)(object)((int)(object)l[i] * (int)(object)r[i]);
                else if (typeof(TItem) == typeof(float))
                    result[i] = (TItem)(object)((float)(object)l[i] * (float)(object)r[i]);
        }
        public static void Multiply<TCollection>(TCollection l, TItem r, TCollection result) where TCollection : UnsafeCollection<TItem>
        {
            for (int i = 0; i < l.Count; i++)
                if (typeof(TItem) == typeof(int))
                    result[i] = (TItem)(object)((int)(object)l[i] * (int)(object)r);
                else if (typeof(TItem) == typeof(float))
                    result[i] = (TItem)(object)((float)(object)l[i] * (float)(object)r);
        }

        public static void Divide<TCollection>(TCollection l, TCollection r, TCollection result) where TCollection : UnsafeCollection<TItem>
        {
            for (int i = 0; i < l.Count; i++)
                if (typeof(TItem) == typeof(int))
                    result[i] = (TItem)(object)((int)(object)l[i] / (int)(object)r[i]);
                else if (typeof(TItem) == typeof(float))
                    result[i] = (TItem)(object)((float)(object)l[i] / (float)(object)r[i]);
        }
    }
}
