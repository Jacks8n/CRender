using System;
using CRender.Pipeline.Structure;
using System.Collections.Generic;
using System.Text;

namespace CRender.Pipeline
{
    public interface IGeometryShader<TGIn, TGOut>
        where TGIn : unmanaged, IRenderData_GIn where TGOut : IRenderData_GOut
    {
        TGOut Geometry(TGIn input);
    }
}
