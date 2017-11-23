using System.Collections.Generic;

namespace DynamicConnectivity.Lib
{
    /// <summary>
    /// vertex of graph
    /// </summary>
    public class DynamicVertex<T> : DynamicNode<T>
    {
        private static int KeyCounter;

        public List<EulerVertex<DynamicNode<T>>> euler;
        public List<DynamicEdge<T>> adjacent;

        public bool IsConnected(DynamicVertex<T> other)
        {
            return euler[0].IsConnected(other.euler[0]);
        }

        public DynamicEdge<T> Link(DynamicVertex<T> other, T value)
        {
            var e = new DynamicEdge<T>(value, KeyCounter, this, other);
            KeyCounter++;
            if (!euler[0].IsConnected(other.euler[0]))
            {
                e.LinkSpanningForests();
            }
            euler[0].SetFlag(true);
            other.euler[0].SetFlag(true);
            EdgeListUtility.InsertEdge(adjacent, e);
            EdgeListUtility.InsertEdge(other.adjacent, e);
            return e;
        }

        /// <summary>
        /// Remove edge from adjacent list
        /// </summary>
        public void RemoveEdge(DynamicEdge<T> edge)
        {
            var adj = adjacent;
            int idx = EdgeListUtility.Index(adj, edge);
            adj.RemoveAt(idx);
            // Check if flag needs to be updated
            if (!((idx < adj.Count && adj[idx].level == edge.level) ||
                (idx > 0 && adj[idx - 1].level == edge.level)))
            {
                euler[edge.level].SetFlag(false);
            }
        }

        public static DynamicVertex<T> Create(T value)
        {
            var v = new DynamicVertex<T>()
            {
                value = value,
                adjacent = new List<DynamicEdge<T>>()
            };
            var euler = new List<EulerVertex<DynamicNode<T>>>();
            euler.Add(EulerVertex<DynamicNode<T>>.Create(v));
            v.euler = euler;
            return v;
        }
    }
}
