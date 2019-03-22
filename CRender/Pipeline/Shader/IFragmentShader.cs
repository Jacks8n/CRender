using System;
using CRender.Pipeline.Structure;
using System.Collections.Generic;
using System.Text;

namespace CRender.Pipeline
{
    public interface IFragmentShader<TFin, TFOut>
        where TFin : unmanaged, IRenderData_FIn<TFin>
    {
        TFOut Fragment(TFin input);
    }
}
