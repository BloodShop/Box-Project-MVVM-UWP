# Box-Project-MVVM-UWP
DataStructures and Algorithms Complexity Box WareHouse. Most searches Time complexity of `Θ(log n)`.
The main WareHouse stores a BSTree of generic type `<double, BSTree<double, Box>`, *TKey* and *TValue* - an inner Tree.
The WareHouse class uses it's own function and condition in order to implement the Binary Search Tree Time comlexity of Insertion, Deletion, Add, Search and etc.

## MVVM Project devided to three parts
#### 1. Model (Boxes WareHouse, Configuration, DataStructures...)
> The BSTree has an efficient function over finding several item between certain range dimension of the requested box. This IEnumrable function yield return all the values with-in the requested range dimensions.
https://github.com/BloodShop/Box-Project-MVVM-UWP/blob/30c148271fee37ccf24a986242c59cc763cfe5ca/Model/DataStructures/BSTree.cs#L299-L315
#### 2. View (XAML)

<p align="center"> Running application - AllBoxes on the left, dateQueue on the right and the search offer</p>
<p align="center">
  <img height="600"  src="https://user-images.githubusercontent.com/23366804/184640776-957bfe34-efb1-45eb-baf8-4703021278d2.png">
</p>

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

<p align="center"> Draw.IO Diagram - Deletion and Insertion operations</p>
<p align="center">
  <img height="600" src="https://user-images.githubusercontent.com/23366804/184636675-ca692483-461c-44dc-92eb-c539aeec47f0.jpg">
</p>

## Advantages of BST over Hash Table
+ We can get all keys in sorted order by just doing Inorder Traversal of BST. This is not a natural operation in Hash Tables and requires extra efforts.
+ Doing order statistics, finding closest lower and greater elements, doing range queries are easy to do with BSTs. Like sorting, these operations are not a natural operation with Hash Tables.
+ BSTs are easy to implement compared to hashing, we can easily implement our own customized BST. To implement Hashing, we generally rely on libraries provided by programming languages.
+ With Self-Balancing BSTs, all operations are guaranteed to work in `Θ(log n)` time. But with Hashing, `Θ(1)` is average time and some particular operations may be costly i.e, `Θ(n2)`, especially when table resizing happens.
+ In BST we can do range searches efficiently but in Hash Table we cannot do range search efficienly.
+ BST are memory efficient but Hash table is not.

## Doubly Linked List Queue
Queue are basically 3 operations such as insertion (called EnQueue), deletion (called DeQueue), size (number of element). This operation are `Θ(1)` time complexity. Those processes can be easily implemented in doubly linked list.
My DoublyLinkedListQueue also implements - Node Deletion (called DeleteNode), Get front data (called Peek), Get the wanted element (called Element), clear function (called clear) and Get Enumerator to get all elements in queue.

https://github.com/BloodShop/Box-Project-MVVM-UWP/blob/30c148271fee37ccf24a986242c59cc763cfe5ca/Model/DataStructures/DoublyLinkedQueue.cs#L160-L185

## Observable Collections
ObservableCollection is a collection that allows code outside the collection be aware of when changes to the collection (add, move, remove) occur. It is used heavily in WPF and Silverlight but its use is not limited to there. Code can add event handlers to see when the collection has changed and then react through the event handler to do some additional processing. This may be changing a UI or performing some other operation.

The code below doesn't really do anything but demonstrates how you'd attach a handler in a class and then use the event args to react in some way to the changes. UWP already has many operations like refreshing the UI built in so you get them for free when using Observable-Collections
https://github.com/BloodShop/Box-Project-MVVM-UWP/blob/30c148271fee37ccf24a986242c59cc763cfe5ca/Model/DataStructures/Observable/ObservableBSTree.cs#L12-L41
An ObservableCollection works essentially like a regular collection except that it implements the interfaces:
+ `INotifyCollectionChanged`
+ `INotifyPropertyChanged`

As such it is very useful when you want to know when the collection has changed. An event is triggered that will tell the user what entries have been added/removed or moved.
*More importantly they are very useful when using databinding.*

## Explanation without Code
#### Normal Collections - No Notifications
Every now and then I go to TLV and my gf asks me to buy stuff. So I take a shopping list with me. The list has a lot of things on there like:
+ Louis Vuitton handbag ($5000)
+ Clive Christian’s Imperial Majesty Perfume ($215,000)
+ Gucci Sunglasses ($2000)

hahaha well I'm not buying that stuff. So I cross them off and remove them from the list and I add instead:
- 12 dozen Titleist golf balls.
- 12 lb bowling ball.

So I usually come home without the goods and she's never pleased. The thing is that she doesn't know about what i take off the list and what I add onto it; she gets no notifications.

#### The ObservableCollection - notifications when changes made
Now, whenever I remove something from the list: she get's a notification on her phone (i.e. sms / email etc)!

The observable collection works just the same way. If you add or remove something to or from it: someone is notified. And when they are notified, well then they call you and you'll get a ear-full. Of course the consequences are customisable via the event handler.

> [EDIT: Here's some sample code from MSDN](https://docs.microsoft.com/en-us/dotnet/desktop/wpf/data/how-to-create-and-bind-to-an-observablecollection?redirectedfrom=MSDN&view=netframeworkdesktop-4.8 "Microsoft Docs.")
