using System.Collections.Generic;
using CRender.Math;

namespace CRender.Math
{
    public class Octree<T> where T : IOctreeElement<T>
    {
        public readonly Cube Bound;

        private readonly int _maxDepth;

        /// <summary>
        /// Used in Loose Octree
        /// </summary>
        private readonly float _tolerance;

        /// <summary>
        /// Convert XYZ coordinate to int to get index, like x:1 y:1 z:0 -> 6(110)
        /// </summary>
        private Octree<T>[] _branches = null;

        private List<T> _items = null;

        public Octree(Cube bound, int maxDepth = int.MaxValue, float tolerance = 0f)
        {
            Bound = bound;
            _maxDepth = maxDepth;
            _tolerance = tolerance;
        }

        /// <summary>
        /// Returns whether operation is successful
        /// </summary>
        public bool AddItem(T item)
        {
            int branchIndex = item.ChooseBranch(this);
            if (branchIndex < 0)
                return false;

            Octree<T> branch = this;
            for (int i = 1; i < _maxDepth && branchIndex > -1; i++)
            {
                branch = branch.GetBranch(branchIndex);
                branchIndex = item.ChooseBranch(branch);
            }

            if (branchIndex > -1)
                branch.AddItemInternal(item);
            return true;
        }
        
        private void AddItemInternal(T item)
        {
            if (_items == null)
                _items = new List<T>();
            _items.Add(item);
        }

        private Vector3 FlagToVector3(int flag)
        {
            return new Vector3(flag >> 2, (flag & 2) >> 1, flag & 1);
        }

        private void InitializeBranch(int index)
        {
            _branches[index] = new Octree<T>(
                bound: new Cube(
                    size:   (_tolerance + .5f) * Bound.Size,
                    pos:    Bound.Position + FlagToVector3(index) * Bound.Size * (.25f - _tolerance * .5f)),
                maxDepth: _maxDepth - 1, tolerance: _tolerance);
        }

        private Octree<T> GetBranch(int index)
        {
            if (_branches == null)
                _branches = new Octree<T>[8];

            for (int i = 0; i < 8; i++)
                InitializeBranch(i);
            return _branches[index];
        }
    }
}