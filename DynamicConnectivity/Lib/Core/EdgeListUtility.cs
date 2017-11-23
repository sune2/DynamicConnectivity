using System.Collections.Generic;

namespace DynamicConnectivity.Lib
{
    public static class EdgeListUtility
    {
        private static int GetUpperBound<T>(List<DynamicEdge<T>> list, DynamicEdge<T> edge)
        {
            int low = -1;
            int high = list.Count;
            while (low + 1 < high)
            {
                int mid = (low + high) >> 1;
                int comp = list[mid].CompareTo(edge);
                if (comp > 0)
                {
                    high = mid;
                }
                else
                {
                    low = mid;
                }
            }
            return high;
        }

        public static void InsertEdge<T>(List<DynamicEdge<T>> list, DynamicEdge<T> edge)
        {
            list.Insert(GetUpperBound(list, edge), edge);
        }

        public static int Index<T>(List<DynamicEdge<T>> list, DynamicEdge<T> edge)
        {
            int idx = GetUpperBound(list, edge);
            return idx - 1;
        }

        public static void RemoveEdge<T>(List<DynamicEdge<T>> list, DynamicEdge<T> edge)
        {
            list.RemoveAt(Index(list, edge));
        }

        public static int LevelIndex<T>(List<DynamicEdge<T>> list, int level)
        {
            int low = -1;
            int high = list.Count;
            while (low + 1 < high)
            {
                int mid = (low + high) >> 1;
                if (list[mid].level >= level)
                {
                    high = mid;
                }
                else
                {
                    low = mid;
                }
            }
            return high;
        }
    }
}
