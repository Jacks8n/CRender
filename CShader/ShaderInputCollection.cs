namespace CShader
{
    public class ShaderInputCollection
    {
        public readonly ShaderInOutMap VertexInputMap;

        public readonly ShaderInOutMap GeometryInputMap;

        public readonly ShaderInOutMap FragmentInputMap;

        public ShaderInputCollection(ShaderInOutMap vertexInputMap, ShaderInOutMap geometryInputMap, ShaderInOutMap fragmentInputMap)
        {
            VertexInputMap = vertexInputMap;
            GeometryInputMap = geometryInputMap;
            FragmentInputMap = fragmentInputMap;
        }
    }
}
