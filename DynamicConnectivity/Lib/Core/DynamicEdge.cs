using System.Collections.Generic;

namespace DynamicConnectivity.Lib
{
    public class DynamicEdge<T> : DynamicNode<T>
    {
        public int key;
        public DynamicVertex<T> source;
        public DynamicVertex<T> dest;
        public int level;
        public List<EulerHalfEdge<DynamicNode<T>>> euler;

        public DynamicEdge(T value, int key, DynamicVertex<T> source, DynamicVertex<T> dest)
        {
            this.value = value;
            this.key = key;
            this.source = source;
            this.dest = dest;
        }

        public void Cut()
        {
            // Don't double cut an edge
            if (source == null)
            {
                return;
            }

            // remove edge from adjacent list of each node
            source.RemoveEdge(this);
            dest.RemoveEdge(this);

            if (euler != null)
            {
                // if euler is not null, the edge is in forests.
                // Cut edge from tree
                for (int i = 0; i < euler.Count; i++)
                {
                    euler[i].Cut();
                }

                // Find replacement, looping over levels
                for (int i = level; i >= 0; i--)
                {
                    var tv = source.euler[i].node.GetRoot();
                    var tw = dest.euler[i].node.GetRoot();
                    if (tv.count > tw.count)
                    {
                        RaiseLevelOfTree(tw, i);
                        if (Visit(tw, i))
                        {
                            break;
                        }
                    }
                    else
                    {
                        RaiseLevelOfTree(tv, i);
                        if (Visit(tv, i))
                        {
                            break;
                        }
                    }
                }
            }
            source = null;
            dest = null;
            euler = null;
            level = 32;
        }

        private void RaiseLevelOfTree(TreapNode<EulerNode<DynamicNode<T>>> node, int level)
        {
            if (node == null)
            {
                return;
            }
            var e = node.val.value as DynamicEdge<T>;
            if (e != null && e.level == level) // second predicate is for avoiding duplicate raise
            {
                e.RaiseLevel();
            }
            RaiseLevelOfTree(node.left, level);
            RaiseLevelOfTree(node.right, level);
        }

        /// <summary>
        /// Search over Tv for edge connecting to Tw
        /// </summary>
        private bool Visit(TreapNode<EulerNode<DynamicNode<T>>> node, int level)
        {
            if (node.flag)
            {
                var v = node.val.value as DynamicVertex<T>;
                var adj = v.adjacent;
                int ptr = EdgeListUtility.LevelIndex(adj, level);
                for (; ptr < adj.Count && adj[ptr].level == level; ptr++)
                {
                    var e = adj[ptr];
                    var es = e.source;
                    var et = e.dest;

                    if (es.euler[level].IsConnected(et.euler[level]))
                    {
                        // Raise edge level if dest is in current tree
                        e.RaiseLevel();
                        ptr--;
                    }
                    else
                    {
                        // Otherwise, connect trees and finish, because dest is in target tree
                        e.LinkSpanningForests();
                        return true;
                    }
                }
            }

            if (node.left != null && node.left.flagAggregate)
            {
                if (Visit(node.left, level))
                {
                    return true;
                }
            }
            if (node.right != null && node.right.flagAggregate)
            {
                if (Visit(node.right, level))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Raise the level of the edge, optionally inserting into higher level trees
        /// </summary>
        public void RaiseLevel()
        {
            // Update position in edge lists
            source.RemoveEdge(this);
            dest.RemoveEdge(this);
            level++;
            EdgeListUtility.InsertEdge(source.adjacent, this);
            EdgeListUtility.InsertEdge(dest.adjacent, this);

            // Update flags for s
            if (source.euler.Count <= level)
            {
                source.euler.Add(EulerVertex<DynamicNode<T>>.Create(source));
            }
            var es = source.euler[level];
            es.SetFlag(true);

            // Update flags for t
            if (dest.euler.Count <= level)
            {
                dest.euler.Add(EulerVertex<DynamicNode<T>>.Create(dest));
            }
            var et = dest.euler[level];
            et.SetFlag(true);

            // If the edge is already part of a spanning tree, relink them in the next level
            if (euler != null)
            {
                euler.Add(es.Link(et, this));
            }
        }

        /// <summary>
        /// Add an edge to all spanning forests with level <= this.level
        /// </summary>
        public void LinkSpanningForests()
        {
            var es = source.euler;
            var et = dest.euler;
            euler = new List<EulerHalfEdge<DynamicNode<T>>>();
            for (int i = 0; i <= level; i++)
            {
                if (es.Count <= i)
                {
                    es.Add(EulerVertex<DynamicNode<T>>.Create(source));
                }
                if (et.Count <= i)
                {
                    et.Add(EulerVertex<DynamicNode<T>>.Create(dest));
                }
                euler.Add(es[i].Link(et[i], this));
            }
        }

        public override string ToString()
        {
            return string.Format("{0}[{1}]:{2}", value, level, key);
        }

        public int CompareTo(DynamicEdge<T> other)
        {
            if (level != other.level)
            {
                return level - other.level;
            }
            return key - other.key;
        }
    }
}
