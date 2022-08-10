using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DataStructures.Observable
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
    }
}