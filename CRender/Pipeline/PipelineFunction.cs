using CRender.Structure;
using CRender.Math;
using CRender.Pipeline.Structure;

using static CRender.Pipeline.ShaderValue;

namespace CRender.Pipeline
{
    public partial class PipelineBase<TApp, TV2F>
    {
        protected unsafe void ProcessGeometryStage(RenderEntity entity, TV2F* v2fOutput, Vector2* screenCoordOutput)
        {
            ObjectToWorld = entity.Transform.LocalToWorld;
            ObjectToView = WorldToView * ObjectToWorld;

            Vector3[] vertices = entity.Model.Vertices;
            Material material = entity.Material;

            IRenderData_App<TApp> appdata = new TApp();
            for (int i = 0; i < vertices.Length; i++)
            {
                appdata.AssignAppdata(ref entity.Model, i);
                TV2F v2f = material.Shader.Vertex(material.Shader as IVertexShader<TApp, TV2F>, appdata);

                v2fOutput[i] = v2f;
                screenCoordOutput[i] = ViewToScreen(v2f.Vertex_VOut);
            }
        }

    }
}
