using CRender.Math;

namespace CRender.Structure
{
    public class RenderEntity : IRenderObject, IAppliable<RenderEntity>, IOctreeElement<RenderEntity>
    {
        public Transform Transform { get; }

        public Model Model;

        public Material Material;
        
        private readonly RenderEntity _instanceToApply;

        public RenderEntity(Transform transform, Model model, Material material)
        {
            Transform = transform;
            Model = model;
            Material = material;
            _instanceToApply = new RenderEntity(transform.GetInstanceToApply(), model.GetInstanceToApply(), material.GetInstanceToApply());
        }

        public RenderEntity GetInstanceToApply()
        {
            return _instanceToApply;
        }

        int IOctreeElement<RenderEntity>.ChooseBranch(Octree<RenderEntity> tree)
        {
            throw new System.NotImplementedException();
        }
    }
}
