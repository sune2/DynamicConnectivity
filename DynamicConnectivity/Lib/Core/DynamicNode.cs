namespace DynamicConnectivity.Lib
{
    public class DynamicNode<T>
    {
        public T value;

        public override string ToString()
        {
            return string.Format("[DN:{0}]", value);
        }
    }
}
