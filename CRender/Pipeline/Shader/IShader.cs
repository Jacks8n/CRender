using CRender.Pipeline.Structure;
using CRender.Structure;

namespace CRender.Pipeline
{
    public interface IShader<TApp, TFIn, TFOut>
        where TApp : unmanaged, IRenderData_App<TApp> where TFIn : unmanaged, IRenderData_FIn<TFIn>
    {
        IRenderData_VOut Vertex(TApp appdata);

        TFOut Fragment(TFIn input);
    }

    public interface IShader<TApp, TGIn, TFIn, TFOut> : IShader<TApp, TFIn, TFOut>
        where TApp : unmanaged, IRenderData_App<TApp> where TGIn : unmanaged, IRenderData_GIn where TFIn : unmanaged, IRenderData_FIn<TFIn>
    {
        IRenderData_GOut Geometry(TGIn input);
    }
}
