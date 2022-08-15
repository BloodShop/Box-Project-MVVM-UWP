# Box-Project-MVVM-UWP
DataStructures and Algorithms Complexity Box WareHouse. Most searches Time complexity of `O(log n)`.
The main WareHouse stores a BSTree of generic type <double, BSTree<double, Box>`, TKey and TValue - an inner Tree.

## MVVM devided to three parts
#### 1. Model (Boxes WareHouse, Configuration, DataStructures...)
#### 2. View (XAML)
![image](https://user-images.githubusercontent.com/23366804/184636276-e32ed137-96e0-4887-a846-85141d8dc265.png)
#### 3. View-Model (MainView model base)
```javascript
public class MainViewModelBase : INotifyPropertyChanged, INotifyCollectionChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public event NotifyCollectionChangedEventHandler CollectionChanged;
        event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
        {
            add { this.PropertyChanged += value; }
            remove { this.PropertyChanged -= value; }
        }
        protected virtual void RaisePropertyChanged([CallerMemberName] string propertyName = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        public virtual void RaiseCollectionChanged(NotifyCollectionChangedEventArgs e) => this.CollectionChanged?.Invoke(this, e);
        public virtual void RaisePropertyChanged(PropertyChangedEventArgs e) => this.PropertyChanged?.Invoke(this, e);
    }
```

## Binray Search Tree
Binary Search Tree is a node-based binary tree data structure which has the following properties:
+ The left subtree of a node contains only nodes with keys lesser than the node’s key.
+ The right subtree of a node contains only nodes with keys greater than the node’s key.
+ The left and right subtree each must also be a binary search tree.

![AlgorithmsProject-Page-3](https://user-images.githubusercontent.com/23366804/184636675-ca692483-461c-44dc-92eb-c539aeec47f0.jpg)

## Advantages of BST over Hash Table
+ We can get all keys in sorted order by just doing Inorder Traversal of BST. This is not a natural operation in Hash Tables and requires extra efforts.
+ Doing order statistics, finding closest lower and greater elements, doing range queries are easy to do with BSTs. Like sorting, these operations are not a natural operation with Hash Tables.
+ BSTs are easy to implement compared to hashing, we can easily implement our own customized BST. To implement Hashing, we generally rely on libraries provided by programming languages.
+ With Self-Balancing BSTs, all operations are guaranteed to work in O(Logn) time. But with Hashing, Θ(1) is average time and some particular operations may be costly i.e, O(n2 ), especially when table resizing happens.
+ In BST we can do range searches efficiently but in Hash Table we cannot do range search efficienly.
+ BST are memory efficient but Hash table is not.
