using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace Model.DataStructures
{
    /// <summary>
    /// Traversal Enum to let the user choose what Order / Print
    /// to get the BSTree<Tkey, Tvalue> <see cref="BSTree{Tkey, Tvalue}"/>
    /// </summary>
    public enum Traversal { InOrder, PreOrder, PostOrder, LevelOrder, RightInOrder }

    /// <summary>
    /// Binary Search Tree Generic Class < Key | Value >
    /// The tree's Complexity is O(n * log (n))
    /// To find and object 
    /// </summary>
    /// <typeparam name="Tkey"></typeparam>
    /// <typeparam name="Tvalue"></typeparam>
    public class BSTree<Tkey, Tvalue> : IEnumerable where Tkey : IComparable
    {
        /// <summary>
        /// Root (top) of the binarySearch tree
        /// </summary>
        protected TreeNode _root;

        //public event NotifyCollectionChangedEventHandler CollectionChanged;
        //public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Ctor initialize root
        /// </summary>
        public BSTree() => _root = null;
        public BSTree<Tkey, Tvalue> CloneBSTree()
        {
            // Create new BinaryTree
            var tree = new BSTree<Tkey, Tvalue>();
            // Get new root
            tree._root = this.Cloning(this._root);
            // Returns a new binary tree
            return tree;
        }
        public TreeNode Cloning(TreeNode node)
        {
            if (node != null)
            {
                var curr = new TreeNode(node.Key, node.Value); // Create new node
                curr.Left = this.Cloning(node.Left); // Construct left subtree
                curr.Right = this.Cloning(node.Right); // Construct right subtree
                return curr;
            }
            return null;
        }
        public BSTree(Tkey key, Tvalue value) => Add(key, value);

        /// <summary>
        /// Nested private node class
        /// </summary>
        public class TreeNode
        {
            public Tkey Key { get; set; }
            public Tvalue Value { get; set; }
            public TreeNode Left { get; set; }
            public TreeNode Right { get; set; }
            public TreeNode(Tkey key, Tvalue value)
            {
                Key = key;
                Value = value;
            }
            public int CompareTo(Tkey key) => Key.CompareTo(key);
#if OPERATORS_CONVERSIONS
            public bool Equals(Tkey other) => base.Equals(other);
            #region OPERATORS
            public static bool operator !=(Tkey k1, Tkey k2) => false;
            public static bool operator <(Tkey c1, Tkey c2) => false;
            public static bool operator <=(Tkey c1, Tkey c2) => false;
            public static bool operator ==(Tkey c1, Tkey c2) => false;
            public static bool operator >=(Tkey c1, Tkey c2) => false;
            public static bool operator >(Tkey c1, Tkey c2) => false;
            public static Tkey operator +(Tkey c1, Tkey c2) => default;
            public static Tkey operator -(Tkey c1, Tkey c2) => default;
            public static Tkey operator *(Tkey c1, Tkey c2) => default;
            public static Tkey operator /(Tkey c1, Tkey c2) => default;
            public static Tkey operator ++(Tkey c1) => default;
            public static Tkey operator --(Tkey c1) => default;
            #endregion
            #region CONVERSIONS
            public static implicit operator Tkey(byte d) => default;
            public static explicit operator Tkey(short d) => default;
            public static implicit operator Tkey(int d) => default;
            public static explicit operator Tkey(long d) => default;
            public static explicit operator Tkey(float d) => default;
            public static explicit operator Tkey(double d) => default;
            #endregion
#endif
        }
        public bool IsEmpty() => _root == null; // O(1)
        public bool IsBST() => IsBST(_root);
        bool IsBST(TreeNode node)
        {
            if (node == null) return true;

            if (node.Left != null && node.CompareTo(node.Left.Key) < 0) // False if Left.Key is > than node.Key
                return false;

            if (node.Right != null && node.CompareTo(node.Right.Key) > 0) // False if Right.Key is < than node.Key
                return false;

            if (!IsBST(node.Left) || !IsBST(node.Right))
                return false;

            return true;
        }
        public int Height() => Height(_root); // O(log n)
        int Height(TreeNode node) // Compute the height of the BSTree
        {
            if (node == null)
                return 0;
            else
            {
                int leftH = Height(node.Left);
                int rightH = Height(node.Right);

                //use the larger one
                if (leftH > rightH) return (leftH + 1);
                else return (rightH + 1);
            }
        }

        //public void Add(TreeNode xNode, Tkey key2/*, Action action = null*/)
        //{
        //    if (xNode.Value is BSTree<Tkey, Tvalue> yBSTree)
        //    {
        //        if (!IsEmpty())
        //        {
        //            _root = new TreeNode(xNode.Key, xNode.Value);
        //            return;
        //        }
        //        TreeNode temp = FindNode(xNode.Key);
        //        if (temp == null)
        //            Add(xNode.Key, xNode.Value, _root);
        //        else
        //        {
        //            TreeNode nodeY = yBSTree.FindNode(key2);
        //            if (nodeY == null)
        //                yBSTree.Add(new TreeNode(key2, ));
        //            //else action?.Invoke();
        //        }
        //    }
        //    else if (xNode.Value is Tvalue)
        //    {
        //        ////
        //    }
        //}
        //public void Add(Tkey keyX, Tkey keyY)
        //{
        //    if (!IsEmpty())
        //    {
        //        var bst = new BSTree<Tkey, Tvalue>();
        //        bst.AddNodeY(keyY); 
        //        _root = new TreeNode(keyX, (Tvalue)Convert.ChangeType(bst, typeof(Tvalue)));
        //        return;
        //    }
        //    TreeNode temp = FindNode(keyX);
        //    if (temp == null)
        //        Add(keyX, temp.Value, _root);
        //    else
        //    {
        //        var yBSTree = temp.Value as BSTree<Tkey, Tvalue>;
        //        TreeNode nodeY = yBSTree.FindNode(keyY);
        //        if (nodeY == null)
        //            yBSTree.AddNodeY(keyY);
        //        //else action?.Invoke();
        //    }
        //}

        public void Add(Tkey key, Tvalue value) // O(log n)
        {
            if (IsEmpty()) _root = new TreeNode(key, value);
            else Add(key, value, _root);
            //CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add/*, */));
        }
        void Add(Tkey key, Tvalue value, TreeNode node) // Recursive add
        { // Can't be qual to the key because was checked at Add FindNode(key) == null
            int comp = node.CompareTo(key);
            if (comp < 0)
            {
                if (node.Right == null) node.Right = new TreeNode(key, value);
                else Add(key, value, node.Right);
            }
            else if (comp > 0)
            {
                if (node.Left == null) node.Left = new TreeNode(key, value);
                else Add(key, value, node.Left);
            }
        }
        public TreeNode Remove(Tkey key)
        {
            if (!IsEmpty()) // If the tree is empty
                return _root = Remove(_root, key);
            return null;
        }
        TreeNode Remove(TreeNode node, Tkey key) // O(log n)
        {
            int comp = node.CompareTo(key);
            if (comp > 0)
                node.Left = Remove(node.Left, key);
            else if (comp < 0)
                node.Right = Remove(node.Right, key);
            else // Otherwise they are Equal ==
            {
                // node with only one child or no child
                if (node.Left == null)
                    return node.Right;
                else if (node.Right == null)
                    return node.Left;

                node.Key = GetMinimum(node.Right); // Node with two children
                node.Right = Remove(node.Right, node.Key); // Delete the inOrder successor
            }
            return node;
        }
        public TreeNode RemoveNode(TreeNode node) // O(1)
        {
            // node with only one child or no child
            if (node.Left == null)
                return node.Right;
            else if (node.Right == null)
                return node.Left;

            node.Key = GetMinimum(node.Right); // Node with two children
            node.Right = Remove(node.Right, node.Key); // Delete the inOrder successor

            //CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, node));
            return node;
        }
        Tkey GetMinimum(TreeNode node) // O(log n)
        {
            Tkey minv = node.Key;
            while (node.Left != null)
            {
                minv = node.Left.Key;
                node = node.Left;
            }
            return minv;
        }

        //public TreeNode FindNode(Tkey key) => FindNode(key, _root);
        //TreeNode FindNode(Tkey key, TreeNode node)
        //{
        //    if (node == null) return default;
        //    int comp = node.CompareTo(key);

        //    if (comp == 0) return node;
        //    else if (comp > 0)
        //        return FindNode(key, node.Right);
        //    else // (comp < 0)
        //        return FindNode(key, node.Left);
        //}

        public bool FindValue(Tkey key, out Tvalue value) // O(log n)
        {
            if (IsEmpty())
            {
                value = default(Tvalue);
                return false;
            }
            value = FindValue(key, _root);
            return value != null;
        }
        Tvalue FindValue(Tkey key, TreeNode node) // O(log n)
        {
            if (node == null) return default;
            int comp = node.CompareTo(key);

            if (comp == 0)
                return node.Value;

            else if (comp < 0)
                return FindValue(key, node.Right);
            else // (comp > 0)
                return FindValue(key, node.Left);
        }
        public bool FindNode(Tkey key, out TreeNode node) // O(log n)
        {
            if (IsEmpty())
            {
                node = null;
                return false;
            }
            node = FindNode(key, _root);
            return node != null;
        }
        TreeNode FindNode(Tkey key, TreeNode node) // O(log n)
        {
            if (node == null) return default;
            int comp = node.CompareTo(key);

            if (comp == 0) return node;
            else if (comp < 0)
                return FindNode(key, node.Right);
            else // (comp > 0)
                return FindNode(key, node.Left);
        }
        public TreeNode FindNode(Tkey key) // O(log n)
        {
            if (IsEmpty())
                return null;
            return FindNode(key, _root);
        }

        public virtual IEnumerable GetRange(Tkey minKey, Tkey maxKey) => GetRange(_root, minKey, maxKey);
        protected virtual IEnumerable GetRange(TreeNode node, Tkey minKey, Tkey maxKey)
        {
            if (node == null) yield break;

            if (minKey.CompareTo(node.Key) < 0)
                foreach (TreeNode leftNode in GetRange(node.Left, minKey, maxKey))
                    yield return leftNode;

            // If NODE's kEY lies in range, then YIELD RETURNS NODE
            if (minKey.CompareTo(node.Key) <= 0 && maxKey.CompareTo(node.Key) >= 0)
                yield return node;

            // Recursively call the right subtree
            foreach (TreeNode rightNode in GetRange(node.Right, minKey, maxKey))
                yield return rightNode;
        }

        //public bool FindLilBiggerValue(Tkey minKey, Tkey maxKey, out Tvalue value) // O(log n)
        //{
        //    if (IsEmpty())
        //    {
        //        value = default(Tvalue);
        //        return false;
        //    }
        //    value = FindLilBiggerValue(minKey, maxKey, _root);
        //    return value != null;
        //}
        //Tvalue FindLilBiggerValue(Tkey minKey, Tkey maxKey, TreeNode node) // O(log n)
        //{
        //    if (node == null) return default;
        //    int compMIN = node.CompareTo(minKey);
        //    int compMAX = node.CompareTo(maxKey);

        //    if (compMIN >= 0 && compMAX <= 0) // If the dimensions match the 
        //        if (node.Left != null && node.Left.Key.CompareTo(minKey) > 0 && !node.Left.Visted)
        //            return FindLilBiggerValue(minKey, maxKey, node.Left);
        //        else
        //        {
        //            node.Visted = true;
        //            return node.Value;
        //        }
        //    else if (compMIN < 0)
        //        return FindLilBiggerValue(minKey, maxKey, node.Right);
        //    else if (compMAX > 0)
        //        return FindLilBiggerValue(minKey, maxKey, node.Left);
        //    return default;
        //}

        public IEnumerable OrderPick(Traversal order)
        {
            switch (order)
            {
                case Traversal.InOrder: return InOrder(_root);
                case Traversal.PreOrder: return PreOrder(_root);
                case Traversal.PostOrder: return PostOrder(_root);
                case Traversal.LevelOrder: return LevelOrder(_root);
                case Traversal.RightInOrder: return RightInOrder(_root);
                default: return null;
            }
        }
        IEnumerable PreOrder(TreeNode node)
        {
            if (node != null) yield return node.Value;
            else yield break;

            foreach (var leftNode in PreOrder(node.Left))
                yield return leftNode;

            foreach (var rightNode in PreOrder(node.Right))
                yield return rightNode;
        }
        IEnumerable PostOrder(TreeNode node)
        {
            if (node == null) yield break;

            if (node.Left != null)
                foreach (var leftNode in PostOrder(node.Left))
                    yield return leftNode;

            if (node.Right != null)
                foreach (var rightNode in PostOrder(node.Right))
                    yield return rightNode;

            yield return node.Value;
        }
        IEnumerable InOrder(TreeNode node)
        {
            if (node == null) yield break;

            if (node.Left != null)
                foreach (var leftNode in InOrder(node.Left))
                    yield return leftNode;

            yield return node.Value;

            if (node.Right != null)
                foreach (var rightNode in InOrder(node.Right))
                    yield return rightNode;
        }
        IEnumerable LevelOrder(TreeNode node)
        {
            DoublyLinkedQueue<TreeNode> queue = new DoublyLinkedQueue<TreeNode>();
            queue.EnQueue(node);
            while (queue.Size != 0)
            {
                TreeNode tempNode = queue.DeQueue();
                yield return tempNode.Value;

                if (tempNode.Left != null) // Enqueue left child
                    queue.EnQueue(tempNode.Left); ;


                if (tempNode.Right != null) // Enqueue right child
                    queue.EnQueue(tempNode.Right);
            }
        }
        IEnumerable RightInOrder(TreeNode node)
        {
            if (node == null) yield break;

            if (node.Right != null)
                foreach (var rightNode in InOrder(node.Right))
                    yield return rightNode;

            yield return node.Value;

            if (node.Left != null)
                foreach (var leftNode in InOrder(node.Left))
                    yield return leftNode;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator(_root);
        IEnumerator GetEnumerator(TreeNode node)
        {
            DoublyLinkedQueue<TreeNode> queue = new DoublyLinkedQueue<TreeNode>();
            queue.EnQueue(node);
            while (queue.Size != 0)
            {
                TreeNode tempNode = queue.DeQueue();
                yield return tempNode;

                if (tempNode.Left != null) // Enqueue left child
                    queue.EnQueue(tempNode.Left); ;


                if (tempNode.Right != null) // Enqueue right child
                    queue.EnQueue(tempNode.Right);
            }
        }

#if Convert_To_Balanced_BST
        public TreeNode ReBuildTree(TreeNode root)
        {
            // Store nodes of given BST in sorted order
            List<TreeNode> nodes = new List<TreeNode>();
            StoreBSTNodes(root, nodes);

            // Constructs BST from nodes[]
            int n = nodes.Count;
            return BuildTreeUtil(nodes, 0, n - 1);
        }
        public TreeNode BuildTreeUtil(List<TreeNode> nodes, int start, int end)
        {
            if (start > end) return null; // base case

            // Get the middle element and make it root
            int mid = (start + end) / 2;
            TreeNode node = nodes[mid];

            // Using index in Inorder traversal, construct left and right subtress 
            node.Left = BuildTreeUtil(nodes, start, mid - 1);
            node.Right = BuildTreeUtil(nodes, mid + 1, end);

            return node;
        }
        public void StoreBSTNodes(TreeNode root, List<TreeNode> nodes)
        {
            if (root == null) return; // Base case
            // Store nodes in Inorder (which is sorted order for BST)
            StoreBSTNodes(root.Left, nodes);
            nodes.Add(root);
            StoreBSTNodes(root.Right, nodes);
        }
#endif
#if test
    public class BST
    {
        Node root;

        public BST()
        {
            root = null;
        }

        public void AddNode(int x)
        {
            if (root == null) root = new Node(x);
            else AddNode(x, root);
        }
        void AddNode(int x, Node t)
        {
            if (x < t.Data)
            {
                if (t.Left == null)
                    t.Left = new Node(x);
                else
                    AddNode(x, t.Left);
            }
            else
            {
                if (t.Right == null)
                    t.Right = new Node(x);
                else
                    AddNode(x, t.Right);
            }
        }
        public void InOrder() => InOrder(root);
        void InOrder(Node t) // Prints the first Left data that has no Left Node, root and then left Node
        {
            if (t != null)
            {
                InOrder(t.Left);
                Console.Write(t.Data + " ");
                InOrder(t.Right);
            }
        }

        public bool Find(int x) => Find(x, root);
        bool Find(int x, Node root)
        {
            if (x == root.Data)
                return true;
            else if (x < root.Data)
                return Find(x, root.Left);
            else if (x > root.Data)
                return Find(x, root.Right);
            return false;
        }

        public int FindMax() => FindMax(root);
        int FindMax(Node root)
        {
            if (root.Right == null) return root.Data;
            return FindMax(root.Right);
        }

        public int FindMin() => FindMin(root);
        int FindMin(Node root)
        {
            if (root.Left == null) return root.Data;
            return FindMax(root.Left);
        }

        public bool WrongisBST() => WrongisBST(root);
        bool WrongisBST(Node t)
        {
            if (t == null) return true;
            if (t.Left != null && t.Left.Data > t.Data)
                return false;
            if (t.Right != null && t.Right.Data < t.Data)
                return false;
            return WrongisBST(t.Left) && WrongisBST(t.Right);
        }

        public bool isBST() => isBST(root, int.MaxValue, int.MaxValue);
        bool isBST(Node t, int max, int min)
        {
            if (t == null) return true;
            if (t.Data < min || t.Data > max) return false;
            return isBST(t.Left, min, t.Data) && isBST(t.Right, t.Data, max);
        }

        public int MaxFullTree()
        {
            return MaxFullTree(root);
        }
        int MaxFullTree(Node t)
        {
            if (t == null) return 0;
            int maxL = MaxFullTree(t.Left);
            int maxR = MaxFullTree(t.Right);

            if (maxL == maxR)
                return maxL + 1;
            if (maxL > maxR)
                return maxL;
            return maxR;
        }

        public void BFS()
        {
            Queue<Node> queue = new Queue<Node>();
            queue.Enqueue(root);
            while (!(queue.Count > 0))
            {
                Node n = queue.Dequeue();
                Console.Write(n.Data + " ");
                if (n.Left != null)
                    queue.Enqueue(n.Left);
                if (n.Right != null)
                    queue.Enqueue(n.Right);
            }
        }

        public Node FindClosest(int x)
        {
            Queue<Node> queue = new Queue<Node>();
            queue.Enqueue(root);
            while (!(queue.Count > 0))
            {
                Node n = queue.Dequeue();
                if (n.Data == x) return n;
                if (n.Left != null)
                    queue.Enqueue(n.Left);
                if (n.Right != null)
                    queue.Enqueue(n.Right);
            }
            return null;
        }
    }
#endif
    }
}