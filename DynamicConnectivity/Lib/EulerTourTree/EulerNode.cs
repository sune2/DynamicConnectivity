namespace DynamicConnectivity.Lib
{
    /// <summary>
    /// Base calss of euler tour tree node
    /// </summary>
    public class EulerNode<T>
    {
        public T value;
        public TreapNode<EulerNode<T>> node;

        public override string ToString()
        {
            return string.Format("{0}", value);
        }
    }
}
