namespace CShader
{
    public class ShaderInputCollection
    {
        public readonly ShaderInputMap VertexInputMap;

        public readonly ShaderInputMap GeometryInputMap;

        public readonly ShaderInputMap FragmentInputMap;

        public ShaderInputCollection(ShaderInputMap vertexInputMap, ShaderInputMap geometryInputMap, ShaderInputMap fragmentInputMap)
        {
            VertexInputMap = vertexInputMap;
            GeometryInputMap = geometryInputMap;
            FragmentInputMap = fragmentInputMap;
        }
    }
}
