using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace Model.DataStructures
{
    public class QNode<T>
    {
        public T Data { get; private set; }
        public QNode<T> Next { get; internal set; }
        public QNode<T> Prev { get; internal set; }
        public QNode(T data, QNode<T> prev)
        {
            Data = data;
            Next = null;
            Prev = prev;
        }
    }
    public class DoublyLinkedQueue<T> : ObservableCollection<T>, IEnumerable 
        where T : class
    {
        //class QNode
        //{
        //    public T Data;
        //    public QNode<T> Next;
        //    public QNode<T> Prev;
        //    public QNode(T data, QNode<T> prev)
        //    {
        //        Data = data;
        //        Next = null;
        //        Prev = prev;
        //    }
        //}
        public QNode<T> Front { get; private set; }
        public QNode<T> Last { get; private set; }
        public int Size { get; private set; } = 0;

        public void Clear()
        {
            Front = null;
            Last = null;
            Size = 0;
        }
        public QNode<T> Element(T element) // O(n)
        {
            if (IsEmpty()) return null;
            for (QNode<T> p = Front; p != null; p = p.Next)
                if (p.Data == element) return p;
            return null;
        }
        public QNode<T> Add(T data, Predicate<T> predicate) // O(n) 
        {
            if (Last != null && predicate(Last.Data)) return EnQueue(data);

            for (QNode<T> p1 = Front; p1 != null; p1 = p1.Next)
                if (!predicate(p1.Data))
                    return AddBefore(data, p1);
                
            return EnQueue(data);
        }
        QNode<T> AddBefore(T data, QNode<T> before) // O(1)
        {
            var @new = new QNode<T>(data, null);
            if (before.Prev == null)
            {
                before.Prev = @new;
                @new.Next = before;
                @new.Prev = null;
                Front = @new;
                Size++;
            }
            else
            {
                @new.Prev = before.Prev;
                @new.Next = before;
                before.Prev.Next = @new;
                before.Prev = @new;
                Size++;
            }
            return @new;
        }
        public QNode<T> EnQueue(T data) // Add a node into queue O(1) 
        {
            if (data == null) return null;
            var node = new QNode<T>(data, Last); // Create a new node
            if (IsEmpty())
            {
                Front = node; // When adding a first node of queue
                Size = 1;
            }
            else
            {
                Last.Next = node;
                Size++;
            }
            Last = node;
            return Last;
        }
        public bool IsEmpty() => Size == 0;
        public T Peek() // Get a front element of queue O(1) 
        {
            if (IsEmpty()) return default;
            else return Front.Data;
        }
        public T DeQueue() // Remove a front node of a queue O(1) 
        {
            if (IsEmpty()) 
                return default;
            else
            {
                var data = Peek();
                if (Front == Last) // When queue contains only one node
                {
                    Last = null;
                    Front = null;
                }
                else
                {
                    Front = Front.Next;
                    Front.Prev = null;
                }
                Size--;
                return data; 
            }
        }
        public void PrintQdata(Action<string> action) // Print elements of queue
        {
            var node = Front;
            action("\nQueue Element");
            while (node != null)
            {
                action($" {node.Data}");
                node = node.Next;
            }
            action("\n");
        }
        public void DeleteNode(T data) // O(n) 
        {
            if (IsEmpty()) return; // Base case
            for (QNode<T> p = Front; p != null; p = p.Next)
                if (p.Data == data)
                {
                    DeleteNode(p);
                    return;
                }
        }
        public void DeleteNode(QNode<T> del) // Deletes the given node if exists O(1) 
        {
            if (IsEmpty()) return; // Base case

            if (del.Prev == null)
            {
                Front = del.Next;
                Front.Prev = null;
            }
            else if (del.Next == null)
            {
                Last = del.Prev;
                Last.Next = null;
            }
            else
            {
                del.Prev.Next = del.Next;
                del.Next.Prev = del.Prev;
            }
            Size--;
            return;
            #region One-way LinkedList queue
            //// If node to be deleted is Front node
            //if (Front == del)
            //    Front = del.Next;

            //// Change next only if node to be deleted is NOT the last node
            //if (del.Next != null)
            //    del.Next.Prev = del.Prev;

            //// Change prev only if node to be deleted is NOT the first node
            //if (del.Prev != null)
            //    del.Prev.Next = del.Next;

            //// Finally, free the memory occupied by del
            #endregion
        }
        public IEnumerator GetEnumerator() // Enumerates through all the elements in queue 
        {
            var node = Front;
            while (node != null)
            {
                yield return node.Data;
                node = node.Next;
            }
        }
    }
}