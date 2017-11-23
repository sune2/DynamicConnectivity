namespace DynamicConnectivity.Lib
{
    public class TreapNode<T>
    {
        public T val;
        public bool flag;
        public bool flagAggregate;
        public TreapNode<T> left;
        public TreapNode<T> right;
        public TreapNode<T> leftMost;
        public TreapNode<T> rightMost;
        public TreapNode<T> prev;
        public TreapNode<T> next;

        public uint priority;
        public int count;
        public TreapNode<T> parent;

        public TreapNode() : this(default(T))
        {
        }

        public TreapNode(T val)
        {
            this.val = val;
            priority = XorShift.Get();
            leftMost = this;
            rightMost = this;
            count = 1;
        }

        private int GetCount(TreapNode<T> node)
        {
            return node != null ? node.count : 0;
        }

        public TreapNode<T> GetRoot()
        {
            var node = this;
            while (node.parent != null)
            {
                node = node.parent;
            }
            return node;
        }

        TreapNode<T> Update()
        {
            count = 1;
            flagAggregate = flag;
            if (left != null)
            {
                count += left.count;
                flagAggregate |= left.flagAggregate;
            }
            if (right != null)
            {
                count += right.count;
                flagAggregate |= right.flagAggregate;
            }
            return this;
        }

        public void SetFlag(bool flag)
        {
            this.flag = flag;
            int counter = 0;
            for (var node = this; node != null; node = node.parent)
            {
                counter++;
                var previous = node.flagAggregate;
                node.Update();
                if (previous == node.flagAggregate)
                {
                    break;
                }
            }
        }

        TreapNode<T> LeftLink(TreapNode<T> child)
        {
            if (child == null)
            {
                left = null;
                leftMost = this;
                prev = null;
            }
            else
            {
                left = child;
                child.parent = this;
                prev = child.rightMost;
                child.rightMost.next = this;
                leftMost = child.leftMost;
            }
            return Update();
        }

        TreapNode<T> RightLink(TreapNode<T> child)
        {
            if (child == null)
            {
                right = null;
                rightMost = this;
                next = null;
            }
            else
            {
                right = child;
                child.parent = this;
                next = child.leftMost;
                child.leftMost.prev = this;
                rightMost = child.rightMost;
            }
            return Update();
        }

        public static TreapNode<T> Merge(TreapNode<T> left, TreapNode<T> right)
        {
            if (left == null || right == null)
            {
                return left ?? right;
            }
            if (left.priority > right.priority)
            {
                return left.RightLink(Merge(left.right, right));
            }
            return right.LeftLink(Merge(left, right.left));
        }

        public TreapNode<T> Merge(TreapNode<T> right)
        {
            var left = this;
            if (left != null)
            {
                left = left.GetRoot();
            }
            if (right != null)
            {
                right = right.GetRoot();
            }
            return Merge(left, right);
        }

        /// <summary>
        /// Cut the left child
        /// </summary>
        private void LeftCut()
        {
            if (left != null)
            {
                left.parent = null;
                left.rightMost.next = null;
                prev = left.leftMost.prev;
                left = null;
                leftMost = this;
                prev = null;
                Update();
            }
        }

        /// <summary>
        /// Cut the right child
        /// </summary>
        private void RightCut()
        {
            if (right != null)
            {
                right.parent = null;
                right.leftMost.prev = null;
                next = right.rightMost.next;
                right = null;
                rightMost = this;
                Update();
            }
        }

        public void Split(out TreapNode<T> left, out TreapNode<T> right)
        {
            var node = this;
            right = node.right;
            // Cut node and right
            RightCut();
            left = node;
            // Climb up
            while (node.parent != null)
            {
                var par = node.parent;
                if (par.left == node)
                {
                    // Climb to upper right
                    par.LeftCut();
                    right = par.LeftLink(right);
                }
                else
                {
                    // Climb to upper left
                    par.RightCut();
                    left = par.RightLink(left);
                }
                node = par;
            }
        }

        public TreapNode<T> Insert(T v)
        {
            TreapNode<T> splitLeft, splitRight;
            Split(out splitLeft, out splitRight);
            var node = new TreapNode<T>(v);
            Merge(Merge(splitLeft, node), splitRight);
            return node;
        }

        public TreapNode<T> Remove()
        {
            TreapNode<T> splitLeft, splitRight;
            Split(out splitLeft, out splitRight);
            if (splitLeft.count == 1)
            {
                return splitRight;
            }
            TreapNode<T> removedLeft, thisNode;
            splitLeft.rightMost.prev.Split(out removedLeft, out thisNode);
            return Merge(removedLeft, splitRight);
        }

        public TreapNode<T> Find(T query)
        {
            if (val.Equals(query))
            {
                return this;
            }
            if (left != null)
            {
                var res = left.Find(query);
                if (res != null)
                {
                    return res;
                }
            }
            if (right != null)
            {
                var res = right.Find(query);
                if (res != null)
                {
                    return res;
                }
            }
            return null;
        }

        public override string ToString()
        {
            string result = val.ToString();
            return result;
        }
    }
}
