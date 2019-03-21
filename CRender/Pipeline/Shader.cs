using CRender.Pipeline.Structure;
using CRender.Structure;

namespace CRender.Pipeline
{
    /// <summary>
    /// A flag interface only used in marking shader classes
    /// </summary>
    public interface IShader { }

    public abstract class ShaderBase<TApp, TV2F, TFOut> : IShader, IVertexShader<TApp, TV2F>, IFragmentShader<TV2F, TFOut>
        where TApp : unmanaged, IRenderData_App<TApp> where TV2F : unmanaged, IRenderData_VOut, IRenderData_FIn<TV2F>
    {
        private interface IShader { }

        public abstract TV2F Vertex(TApp input);

        public abstract TFOut Fragment(TV2F input);
    }

    public abstract class ShaderBase<TApp, TV2F> : ShaderBase<TApp, TV2F, GenericVector<float>>
        where TApp : unmanaged, IRenderData_App<TApp> where TV2F : unmanaged, IRenderData_VOut, IRenderData_FIn<TV2F>
    {
    }
}
