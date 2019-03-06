using System;
using System.Collections.Generic;
using System.Text;

namespace CRender.Structure
{
    public interface IPrimitive
    {
        int Count { get; }

        int[] Indices { get; }
    }
}
