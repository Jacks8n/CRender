namespace CShader
{
    public static class SemanticLayoutHelper
    {
        public static void RegisterSemantic(this SemanticLayout layout, bool hasVertex = false, bool hasNormal = false, bool hasUV = false, bool hasColor = false)
        {
            layout.BeginRegister();
            if (hasVertex)
                layout.RegisterVertex();
            if (hasNormal)
                layout.RegisterNormal();
            if (hasUV)
                layout.RegisterUV();
            if (hasColor)
                layout.RegisterColor();
            layout.EndRegister();
        }
    }
}
