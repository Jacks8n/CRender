using System;
using System.Collections.Generic;
using System.Text;

namespace CRender.Structure
{
    public static class GenericVectorArithmetic
    {
        public static ref GenericVector<int> AddUp(this ref GenericVector<int> origin, ref GenericVector<int> other)
        {
            for (int i = 0; i < origin.Length; i++)
                origin[i] += other[i];
            return ref origin;
        }

        public static ref GenericVector<float> AddUp(this ref GenericVector<float> origin, ref GenericVector<float> other)
        {
            for (int i = 0; i < origin.Length; i++)
                origin[i] += other[i];
            return ref origin;
        }
    }
}
