namespace CRender.Structure
{
    public class Material : IAppliable<Material>
    {
        public RenderBuffer<float> MainTexture { get; private set; }

        private Material _instanceToApply;
        
        public Material GetInstanceToApply()
        {
            return this;
        }
    }
}
