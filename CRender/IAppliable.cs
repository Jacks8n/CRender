namespace CRender
{
    public interface IAppliable<T> where T : IAppliable<T>
    {
        T GetInstanceToApply();
    }
}
