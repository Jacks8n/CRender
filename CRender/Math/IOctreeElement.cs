namespace CRender.Math
{
    public interface IOctreeElement<T> where T : IOctreeElement<T>
    {
        /// <summary>
        /// <para> Convert XYZ coordinate to int to get index, like 010 -> 2 </para>
        /// <para> Return negative int to stop choosing </para>
        /// </summary>
        int ChooseBranch(Octree<T> tree);
    }
}
