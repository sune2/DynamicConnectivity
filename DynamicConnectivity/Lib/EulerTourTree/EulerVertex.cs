namespace DynamicConnectivity.Lib
{
    /// <summary>
    /// vertex node of euler tour tree
    /// </summary>
    public class EulerVertex<T> : EulerNode<T>
    {
        public void SetFlag(bool flag)
        {
            node.SetFlag(flag);
        }

        public bool IsConnected(EulerVertex<T> other)
        {
            return node.GetRoot() == other.node.GetRoot();
        }

        public void MakeRoot()
        {
            TreapNode<EulerNode<T>> left, right;
            node.Split(out left, out right);
            if (right != null)
            {
                right.Merge(left);
            }
        }

        public EulerHalfEdge<T> Link(EulerVertex<T> other, T value)
        {
            // Move both vertices to root
            MakeRoot();
            other.MakeRoot();

            // Create half edges and link them to each other
            var st = new EulerHalfEdge<T>(value, this, other);
            var ts = new EulerHalfEdge<T>(value, other, this);
            ts.opposite = st;
            st.opposite = ts;

            // Insert entries in Euler tours
            st.node = node.Insert(st);
            ts.node = other.node.Insert(ts);

            // Link tours together
            node.Merge(other.node);

            // Return half edge
            return st;
        }

        public int GetCount()
        {
            return node.GetRoot().count;
        }

        public void CleanUp()
        {
            node.Remove();
            node.val = null;
            node = null;
        }

        public static EulerVertex<T> Create(T value)
        {
            var v = new EulerVertex<T>()
            {
                value = value
            };
            v.node = new TreapNode<EulerNode<T>>(v);
            return v;
        }
    }
}
