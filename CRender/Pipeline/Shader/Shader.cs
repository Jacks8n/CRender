using CRender.Pipeline.Structure;
using CRender.Structure;

namespace CRender.Pipeline
{
    public abstract class ShaderBase<TApp, TV2F, TFOut> : IShader<TApp, TV2F, TFOut>
        where TApp : unmanaged, IRenderData_App<TApp> where TV2F : unmanaged, IRenderData_VOut, IRenderData_FIn<TV2F>
    {
        public abstract IRenderData_VOut Vertex(TApp appdata);

        public abstract TFOut Fragment(TV2F input);
    }

    public abstract class ShaderBase<TApp, TV2F> : ShaderBase<TApp, TV2F, GenericVector<float>>
        where TApp : unmanaged, IRenderData_App<TApp> where TV2F : unmanaged, IRenderData_VOut, IRenderData_FIn<TV2F>
    { }

    public abstract class ShaderBase<TApp, TV2G, TG2F, TFOut> : IShader<TApp, TV2G, TG2F, TFOut>
        where TApp : unmanaged, IRenderData_App<TApp> where TV2G : unmanaged, IRenderData_VOut, IRenderData_GIn where TG2F : unmanaged, IRenderData_GOut, IRenderData_FIn<TG2F>
    {
        public abstract IRenderData_VOut Vertex(TApp appdata);

        public abstract IRenderData_GOut Geometry(TV2G input);

        public abstract TFOut Fragment(TG2F input);
    }
}
