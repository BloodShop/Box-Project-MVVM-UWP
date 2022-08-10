using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using static Model.DataStructures.ObservableConcurrentQueue<T>;

namespace Model.DataStructures
{
#if ConcurrentQueue
    /// <summary>
    /// Observable Concurrent queue changed event handler
    /// </summary>
    /// <typeparam name="T">
    /// The concurrent queue elements type
    /// </typeparam>
    /// <param name="sender">
    /// The sender.
    /// </param>
    /// <param name="args">
    /// The <see cref="NotifyConcurrentQueueChangedEventArgs{T}"/> instance containing the event data.
    /// </param>
    public delegate void ConcurrentQueueChangedEventHandler<T>( object sender, NotifyConcurrentQueueChangedEventArgs<T> args);


    /// <summary>
    /// The observable concurrent queue.
    /// </summary>
    /// <typeparam name="T">
    /// The content type
    /// </typeparam>
    public sealed class ObservableConcurrentQueue<T> : ConcurrentQueue<T>, INotifyCollectionChanged
    {
#region Public Events

        /// <summary>
        ///     Occurs when concurrent queue elements [changed].
        /// </summary>
        public event ConcurrentQueueChangedEventHandler<T> ContentChanged;
        public event NotifyCollectionChangedEventHandler CollectionChanged;

#endregion

#region Public Methods and Operators

        /// <summary>
        /// Adds an object to the end of the <see cref="T:System.Collections.Concurrent.ConcurrentQueue`1"/>.
        /// </summary>
        /// <param name="item">
        /// The object to add to the end of the <see cref="T:System.Collections.Concurrent.ConcurrentQueue`1"/>
        ///     . The value can be a null reference (Nothing in Visual Basic) for reference types.
        /// </param>
        public new void Enqueue(T item)
        {
            EnqueueItem(item);
        }


        /// <summary>
        /// Attempts to remove and return the object at the beginning of the
        ///     <see cref="T:System.Collections.Concurrent.ConcurrentQueue`1"/>.
        /// </summary>
        /// <param name="result">
        /// When this method returns, if the operation was successful, <paramref name="result"/> contains the
        ///     object removed. If no object was available to be removed, the value is unspecified.
        /// </param>
        /// <returns>
        /// true if an element was removed and returned from the beginning of the
        ///     <see cref="T:System.Collections.Concurrent.ConcurrentQueue`1"/> successfully; otherwise, false.
        /// </returns>
        public new bool TryDequeue(out T result)
        {
            return TryDequeueItem(out result);
        }

        /// <summary>
        /// Attempts to return an object from the beginning of the
        ///     <see cref="T:System.Collections.Concurrent.ConcurrentQueue`1"/> without removing it.
        /// </summary>
        /// <param name="result">
        /// When this method returns, <paramref name="result"/> contains an object from the beginning of the
        ///     <see cref="T:System.Collections.Concurrent.ConcurrentQueue`1"/> or an unspecified value if the operation failed.
        /// </param>
        /// <returns>
        /// true if and object was returned successfully; otherwise, false.
        /// </returns>
        public new bool TryPeek(out T result)
        {
            var retValue = base.TryPeek(out result);
            if (retValue)
            {
                // Raise item dequeued event
                this.OnContentChanged(
                    new NotifyConcurrentQueueChangedEventArgs<T>(NotifyConcurrentQueueChangedAction.Peek, result));
            }

            return retValue;
        }

#endregion

#region Methods

        /// <summary>
        /// Raises the <see cref="E:Changed"/> event.
        /// </summary>
        /// <param name="args">
        /// The <see cref="NotifyConcurrentQueueChangedEventArgs{T}"/> instance containing the event data.
        /// </param>
        private void OnContentChanged(NotifyConcurrentQueueChangedEventArgs<T> args)
        {
            this.ContentChanged?.Invoke(this, args);
            NotifyCollectionChangedAction? action;
            switch (args.Action)
            {
                case NotifyConcurrentQueueChangedAction.Enqueue:
                    {
                        action = NotifyCollectionChangedAction.Add;
                        break;
                    }
                case NotifyConcurrentQueueChangedAction.Dequeue:
                    {
                        action = NotifyCollectionChangedAction.Remove;
                        break;
                    }
                case NotifyConcurrentQueueChangedAction.Empty:
                    {
                        action = NotifyCollectionChangedAction.Reset;
                        break;
                    }
                default:
                    {
                        action = null;
                        break;
                    }

            }

            // Raise event only when action is defined (Add, Remove or Reset)
            if (action.HasValue)
            {
                this.CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(action.Value, args.Action != NotifyConcurrentQueueChangedAction.Empty
                    ? new List<T> { args.ChangedItem }
                    : null));
            }
        }

        /// <summary>
        /// Enqueues the item.
        /// </summary>
        /// <param name="item">The item.</param>
        private void EnqueueItem(T item)
        {
            base.Enqueue(item);

            // Raise event added event
            this.OnContentChanged(
                new NotifyConcurrentQueueChangedEventArgs<T>(NotifyConcurrentQueueChangedAction.Enqueue, item));
        }

        /// <summary>
        /// Tries the dequeue item.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <returns>true if and object was returned successfully; otherwise, false.</returns>
        private bool TryDequeueItem(out T result)
        {
            if (!base.TryDequeue(out result))
            {
                return false;
            }

            // Raise item dequeued event
            this.OnContentChanged(
                new NotifyConcurrentQueueChangedEventArgs<T>(NotifyConcurrentQueueChangedAction.Dequeue, result));

            if (this.IsEmpty)
            {
                // Raise Queue empty event
                this.OnContentChanged(
                    new NotifyConcurrentQueueChangedEventArgs<T>(NotifyConcurrentQueueChangedAction.Empty));
            }

            return true;
        }
#endregion
        /// <summary>
        /// The notify concurrent queue changed event args.
        /// </summary>
        /// <typeparam name="T">
        /// The item type
        /// </typeparam>
        /// <summary>
        /// The notify concurrent queue changed event args.
        /// </summary>
        /// <typeparam name="T">
        /// The item type
        /// </typeparam>
        public class NotifyConcurrentQueueChangedEventArgs<T> : EventArgs
        {
#region Constructors and Destructors

            /// <summary>
            /// Initializes a new instance of the <see cref="NotifyConcurrentQueueChangedEventArgs{T}"/> class.
            /// </summary>
            /// <param name="action">
            /// The action.
            /// </param>
            /// <param name="changedItem">
            /// The changed item.
            /// </param>
            public NotifyConcurrentQueueChangedEventArgs(NotifyConcurrentQueueChangedAction action, T changedItem)
            {
                this.Action = action;
                this.ChangedItem = changedItem;
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="NotifyConcurrentQueueChangedEventArgs{T}"/> class.
            /// </summary>
            /// <param name="action">
            /// The action.
            /// </param>
            public NotifyConcurrentQueueChangedEventArgs(NotifyConcurrentQueueChangedAction action)
            {
                this.Action = action;
            }

#endregion

#region Public Properties

            /// <summary>
            ///     Gets the action.
            /// </summary>
            /// <value>
            ///     The action.
            /// </value>
            public NotifyConcurrentQueueChangedAction Action { get; private set; }

            /// <summary>
            ///     Gets the changed item.
            /// </summary>
            /// <value>
            ///     The changed item.
            /// </value>
            public T ChangedItem { get; private set; }

#endregion
        }
    }
    /// <summary>
    ///     The notify concurrent queue changed action.
    /// </summary>
    public enum NotifyConcurrentQueueChangedAction
    {
        /// <summary>
        ///     New Item was added to the queue
        /// </summary>
        Enqueue,

        /// <summary>
        ///     Item dequeued from the queue
        /// </summary>
        Dequeue,

        /// <summary>
        ///     Item peeked from the queue without being dequeued.
        /// </summary>
        Peek,

        /// <summary>
        ///     The last item in the queue was dequed and the queue is empty.
        /// </summary>
        Empty
        }
#endif
}

