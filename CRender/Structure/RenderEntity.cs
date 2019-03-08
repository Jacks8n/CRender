using CRender.Math;

namespace CRender.Structure
{
    public class RenderEntity : IRenderObject, IAppliable<RenderEntity>, IOctreeElement<RenderEntity>
    {
        public Transform Transform { get; }

        public Model Model;

        /// <summary>
        /// Optional
        /// </summary>
        public Material Material;

        private RenderEntity _instanceToApply;

        public RenderEntity(Transform transform, Model model, Material material)
        {
            Transform = transform;
            Model = model;
            Material = material;
        }

        public RenderEntity GetInstanceToApply()
        {
            if (_instanceToApply == null)
                _instanceToApply = new RenderEntity(Transform.GetInstanceToApply(), Model.GetInstanceToApply(), Material?.GetInstanceToApply());
            return _instanceToApply;
        }

        int IOctreeElement<RenderEntity>.ChooseBranch(Octree<RenderEntity> tree)
        {
            throw new System.NotImplementedException();
        }
    }
}
