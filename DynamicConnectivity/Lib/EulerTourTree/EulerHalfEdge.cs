namespace DynamicConnectivity.Lib
{
    /// <summary>
    /// half edge node of euler tour tree
    /// </summary>
    public class EulerHalfEdge<T> : EulerNode<T>
    {
        public EulerVertex<T> source;
        public EulerVertex<T> dest;

        public EulerHalfEdge<T> opposite;

        public EulerHalfEdge(T value, EulerVertex<T> source, EulerVertex<T> dest)
        {
            this.value = value;
            this.source = source;
            this.dest = dest;
        }

        public void CleanUp()
        {
            node.Remove();
            node.val = null;
            node = null;
            opposite = null;
            source = null;
            dest = null;
        }

        public void Cut()
        {
            // Split into parts
            TreapNode<EulerNode<T>> a, b, c, d;
            node.Split(out a, out b);
            opposite.node.Split(out c, out d);

            if (b != null && (b.GetRoot() == c || b.GetRoot() == d))
            {
                // [a, bc, d] or [a, c, bd]
                var res = a.rightMost.Remove();
                if (res != null)
                {
                    res.Merge(d);
                }
            }
            else
            {
                // [c, ad, b] or [ca, d, b]
                var res = c.rightMost.Remove();
                if (res != null)
                {
                    res.Merge(b);
                }
            }

            opposite.CleanUp();
            CleanUp();
        }
    }
}
