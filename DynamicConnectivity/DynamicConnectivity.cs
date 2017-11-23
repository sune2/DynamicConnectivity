using System;
using System.Collections.Generic;
using DynamicConnectivity.Lib;

namespace DynamicConnectivity
{
    /// <summary>
    /// Dynamic connectivity
    /// </summary>
    public class DynamicConnectivity
    {
        private int _n;
        private List<DynamicVertex<int>> _vertices;
        private Dictionary<long, DynamicEdge<int>> _edges;

        /// <summary>
        /// Initialize a new instance
        /// </summary>
        /// <param name="n">Number of Vertices</param>
        public DynamicConnectivity(int n)
        {
            _n = n;
            _vertices = new List<DynamicVertex<int>>();
            for (int i = 0; i < n; i++)
            {
                _vertices.Add(DynamicVertex<int>.Create(i));
            }
            _edges = new Dictionary<long, DynamicEdge<int>>();
        }

        /// <summary>
        /// Link an edge between i and j
        /// </summary>
        public void Link(int i, int j)
        {
            if (j > i)
            {
                Swap<int>(ref i, ref j);
            }
            long key = (long)i * _n + j;
            if (_edges.ContainsKey(key))
            {
                throw new ArgumentException("A multiple edge is prohibited");
            }
            var e = _vertices[i].Link(_vertices[j], -1);
            _edges[key] = e;
        }

        /// <summary>
        /// Cut an edge between i and j
        /// </summary>
        public void Cut(int i, int j)
        {
            if (j > i)
            {
                Swap<int>(ref i, ref j);
            }
            long key = (long)i * _n + j;
            DynamicEdge<int> edge;
            if (_edges.TryGetValue(key, out edge))
            {
                edge.Cut();
                _edges.Remove(key);
            }
            else
            {
                throw new ArgumentException("Cutting an edge that does not exist");
            }
        }

        private void Swap<T>(ref T lhs, ref T rhs)
        {
            T temp;
            temp = lhs;
            lhs = rhs;
            rhs = temp;
        }

        /// <summary>
        /// Check if i and j are connected by a path
        /// </summary>
        public bool IsConnected(int i, int j)
        {
            return _vertices[i].IsConnected(_vertices[j]);
        }

        /// <summary>
        /// Connected component size containing i
        /// </summary>
        public int GetConnectedSize(int i)
        {
            int count = _vertices[i].euler[0].node.GetRoot().count;
            // this "count" includes half edges
            // n + (n - 1) * 2 = count
            // n = (count + 2) / 3
            return (count + 2) / 3;
        }

        /// <summary>
        /// Return all the vertices connected to i
        /// </summary>
        public IEnumerable<int> GetConnectedNodes(int i)
        {
            var node = _vertices[i].euler[0].node.GetRoot().leftMost;
            while (node != null)
            {
                int val = node.val.value.value;
                if (val != -1)
                {
                    yield return val;
                }
                node = node.next;
            }
        }
    }
}
