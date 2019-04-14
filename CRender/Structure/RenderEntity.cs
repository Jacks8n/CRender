using CUtility.Math;

namespace CRender.Structure
{
    public class RenderEntity : IRenderObject, IAppliable<RenderEntity>, IOctreeElement<RenderEntity>
    {
        public Transform Transform { get; private set; }

        public Model Model;

        /// <summary>
        /// Optional
        /// </summary>
        public IMaterial Material;

        private RenderEntity _instanceToApply;

        private RenderEntity() { }

        public RenderEntity(Transform transform, Model model, IMaterial material)
        {
            Transform = transform;
            Model = model;
            Material = material;
        }

        public RenderEntity GetInstanceToApply()
        {
            if (_instanceToApply == null)
                _instanceToApply = new RenderEntity();
            _instanceToApply.Transform = Transform.GetInstanceToApply();
            _instanceToApply.Model = Model.GetInstanceToApply();
            _instanceToApply.Material = Material;
            return _instanceToApply;
        }

        int IOctreeElement<RenderEntity>.ChooseBranch(Octree<RenderEntity> tree)
        {
            throw new System.NotImplementedException();
        }
    }
}
