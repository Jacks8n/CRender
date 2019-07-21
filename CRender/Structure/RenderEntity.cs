using CUtility.Math;

namespace CRender.Structure
{
    public class RenderEntity : IRenderObject
    {
        public Transform Transform { get; private set; }

        public Model Model;

        /// <summary>
        /// Optional
        /// </summary>
        public Material Material;

        public RenderEntity(Transform transform, Model model, Material material)
        {
            Transform = transform;
            Model = model;
            Material = material;
        }
    }
}
