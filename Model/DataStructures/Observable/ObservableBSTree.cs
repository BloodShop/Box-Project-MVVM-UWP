using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DataStructures
{
    public class ObservableBSTree<TKey, TValue> : BSTree<TKey, TValue>, INotifyCollectionChanged, INotifyPropertyChanged
        where TKey : IComparable
    {
        public int CountOfHandlers { get; private set; }
        public event NotifyCollectionChangedEventHandler CollectionChanged;
        public event PropertyChangedEventHandler PropertyChanged;
        void OnCollectionChanged(NotifyCollectionChangedEventArgs e) => CollectionChanged?.Invoke(this, e);

        public ObservableBSTree(TKey key, TValue value) : base(key, value) { }
        public ObservableBSTree() : base() { }

        public new void Add(TKey key, TValue value)
        {
            base.Add(key, value);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add/*, */));
        }

        public new void Remove(TKey key)
        {
            base.Remove(key);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove/*, */));
        }

        public new TreeNode RemoveNode(TreeNode node)
        {
            var x = base.RemoveNode(node);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, x/*, */));
            return x;
        }

        public override IEnumerable GetRange(TKey minKey, TKey maxKey) => GetRange(_root, minKey, maxKey);
        protected override IEnumerable GetRange(TreeNode node, TKey minKey, TKey maxKey)
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
    }
}