using CRender.Pipeline.Structure;

namespace CRender.Pipeline
{
    public static class ShaderInvoker
    {
        public static TVOut Vertex<TApp, TVOut>(this IVertexShader<TApp, TVOut> shader, TApp input) where TApp : unmanaged, IRenderData_App<TApp> where TVOut : IRenderData_VOut
        {
            return shader.Vertex(input);
        }

        public static TGOut Geometry<TGIn, TGOut>(this IGeometryShader<TGIn, TGOut> shader, TGIn input) where TGIn : unmanaged, IRenderData_GIn where TGOut : IRenderData_GOut
        {
            return shader.Geometry(input);
        }

        public static TFOut Fragment<TFIn, TFOut>(this IFragmentShader<TFIn, TFOut> shader, TFIn input) where TFIn : unmanaged, IRenderData_FIn<TFIn>
        {
            return shader.Fragment(input);
        }
    }
}
