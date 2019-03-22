using System;
using CRender.Pipeline.Structure;
using System.Collections.Generic;
using System.Text;

namespace CRender.Pipeline
{
    public interface IVertexShader<TApp, TVOut>
        where TApp : unmanaged, IRenderData_App<TApp> where TVOut : IRenderData_VOut
    {
        TVOut Vertex(TApp input);
    }
}
