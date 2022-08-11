using Model;
using Model.DataStructures;
using System;
using System.Collections;
using System.Collections.Generic;
using static Model.DataStructures.BSTree<double, Model.DataStructures.BSTree<double, Model.Box>>;

namespace DAL
{ // Encapsulate the Concept that Varies 
    public class BoxesBST : IEnumerable //, ITraversal
    {
        static Configuration _config;
        static bool _init = true;

        // Store's Repository
        static BSTree<double, BSTree<double, Box>> _main;
        static BSTree<double, BSTree<double, Box>> _cloneTree;
        static DoublyLinkedQueue<Box> _dateQ;
        static DoublyLinkedQueue<Box> _cloneQ;

        // Constants which are declared at the configuration
        public readonly double PERCENTAGES_SEARCH_RANGE;
        public readonly int ADD_EXPIRY_DAYS;
        public readonly int WARNING_QUANTITY;
        public readonly int MAX_AMOUNT_BOXES;
        public readonly int DAYS_TO_EXPIRE;

        public Action Retrieve => () => { _main = _cloneTree; _dateQ = _cloneQ; }; // Retrieve function to the original repo
        public DoublyLinkedQueue<Box> DateQ { get => _dateQ; }

        public BoxesBST()
        {
            if (_init)
            {
                _config = new Configuration();
                PERCENTAGES_SEARCH_RANGE = _config.Data.PercentagesSearchRange;
                ADD_EXPIRY_DAYS = _config.Data.AddExpiryDate;
                WARNING_QUANTITY = _config.Data.WarningQuantity;
                MAX_AMOUNT_BOXES = _config.Data.MaxAmountOfBoxes;
                DAYS_TO_EXPIRE = _config.Data.DaysToExpire;

                _ = DataBase.Instance;
                _main = new BSTree<double, BSTree<double, Box>>();
                _dateQ = new DoublyLinkedQueue<Box>();
                foreach (var box in DataBase.Boxes)
                    Add(box);
                _init = false;
            }
        }
#if Traversals
        public void InOrder(Action<string> action)
        {
            foreach (var item in _main.OrderPick(Traversal.InOrder))
                if (item is BSTree<double, Box> bst)
                    foreach (Box x in bst.OrderPick(Traversal.InOrder))
                        action(x + "\n");
        }
        public void PreOrder(Action<string> action)
        {
            foreach (var item in _main.OrderPick(Traversal.PreOrder))
                if (item is BSTree<double, Box> bst)
                    foreach (Box x in bst.OrderPick(Traversal.PreOrder))
                        action(x + "\n");
        }
        public void PostOrder(Action<string> action)
        {
            foreach (var item in _main.OrderPick(Traversal.PostOrder))
                if (item is BSTree<double, Box> bst)
                    foreach (Box x in bst.OrderPick(Traversal.PostOrder))
                        action(x + "\n");
        }
        public void LevelOrder(Action<string> action)
        {
            foreach (var item in _main.OrderPick(Traversal.LevelOrder))
                if (item is BSTree<double, Box> bst)
                    foreach (Box x in bst.OrderPick(Traversal.LevelOrder))
                        action(x + "\n");
        }
        public void RightInOrder(Action<string> action)
        {
            foreach (var item in _main.OrderPick(Traversal.RightInOrder))
                if (item is BSTree<double, Box> bst)
                    foreach (Box x in bst.OrderPick(Traversal.RightInOrder))
                        action(x + "\n");
        }
#endif
        /// <summary>
        /// Contains function which return boolean if the Box exicts at the repository or not
        /// </summary>
        /// <param name="Width">The given Width of the box <see cref="Box.Width"/></param>
        /// <param name="Height">The given Height of the box <see cref="Box.Height"/></param>
        /// <param name="box">Out parameter if the box exicts <see cref="Box"/></param>
        /// <returns></returns>
        public bool Contains(double Width, double Height, out Box box)
        {
            if (_main.FindValue(Width, out BSTree<double, Box> yTree))
                if (yTree.FindValue(Height, out box))
                    return true;
            box = null;
            return false;
        }
        /// <summary>
        /// If Box Exicts Add to the box the given amount
        /// </summary>
        /// <param name="box">The box that exicts and you want to add to it curtain amount</param>
        /// <param name="amount">The amount to add to the given box</param>
        public void AddToExicting(Box box, int amount)
        {
            if(box.Amount + amount <= MAX_AMOUNT_BOXES) box.Amount += amount;
            else box.Amount = MAX_AMOUNT_BOXES;
        }
        /// <summary>
        /// Add's a box to the store repository
        /// </summary>
        /// <param name="box">The given box to add to the store <see cref="Box"/></param>
        public void Add(Box box)
        {
            if (box.Amount == 0) return;

            if (_main.FindValue(box.Width, out BSTree<double, Box> yTree))
                if (yTree.FindValue(box.Height, out Box current))
                    AddToExicting(current, box.Amount);
                else
                {
                    yTree.Add(box.Height, box);
                    box.SelfRefrence = _dateQ.EnQueue(box);
                }
            else // if width dimension doesn't exist
            {
                yTree = new BSTree<double, Box>(box.Height, box);
                _main.Add(box.Width, yTree);
                box.SelfRefrence = _dateQ.EnQueue(box);
            }
        }
        /// <summary>
        /// Remove's a box out of the repository of the store if exicts
        /// </summary>
        /// <param name="box">The required box to remove from store <see cref="Box"/></param>
        /// <param name="amount">The amount to remove from the store repository</param>
        /// <returns></returns>
        public Box Remove(Box box, int amount)
        {
            if (box == null) return null;

            if (_main.FindValue( /*Tkey*/ box.Width, /*Tvalue*/ out BSTree<double, Box> yTree))
                if (yTree.FindValue(box.Height, out Box current))
                {
                    if (amount >= box.Amount)
                    {
                        box.Amount = 0;
                        yTree.Remove(box.Height);
                        if (amount != int.MaxValue) _dateQ.DeleteNode(box.SelfRefrence); // O(1)
                        current.Amount -= 0;
                    }
                    else
                    {
                        current.Amount -= amount;
                        return current;
                    }
                }
            return null;
        }
        /// <summary>
        /// Get all Regueired Boxes with-in the asked dimensions
        /// </summary>
        /// <param name="width">The Width dimension of the box <see cref="Box.Width"/> </param>
        /// <param name="height">The Height dimension of the box <see cref="Box.Height"/></param>
        /// <param name="amount">The amount of this specific box you want to get <see cref="Box.Amount"/></param>
        /// <returns></returns>
        /// <exception cref="Exception">Expected exception which thrown while there are no appropriate Box Dimensions</exception>
        public IEnumerable<Box> Get(double width, double height, int amount)
        {
            CloneTree();
            CloneQueue();
            DoublyLinkedQueue<Box> rangeQueue = GetRange(width, height); // Get the best matches - Front best, Last less match
            while (amount != 0)
            {
                Box box = rangeQueue.DeQueue();
                if (box == null) throw new Exception("Box's Dimensions Doesn't Exist / Doesn't fit the amount you asked\nTry another Dimensions");

                if (amount >= box.Amount)
                {
                    box.AmountBought = box.Amount;
                    amount -= box.Amount;
                    yield return box;
                    Remove(box, MAX_AMOUNT_BOXES);
                }
                else // (amount < box.Amount)
                {
                    box.AmountBought = amount;
                    box.Amount -= amount;
                    box.LastUsedDate = DateTime.Now;
                    amount = 0;
                    yield return box;
                    {
                        _dateQ.DeleteNode(box.SelfRefrence); // O(1)
                        box.SelfRefrence = _dateQ.EnQueue(box);
                    }
                }
            }
        }
        /// <summary>
        /// Get all boxes that fit with-in the Min and Max asked range
        /// </summary>
        /// <param name="width">The Width dimension of the box <see cref="Box.Width"/> </param>
        /// <param name="height">The Height dimension of the box <see cref="Box.Height"/></param>
        /// <returns></returns>
        DoublyLinkedQueue<Box> GetRange(double width, double height)
        {
            double newWidth = width + width * (PERCENTAGES_SEARCH_RANGE / 100);
            double newHeight = height + height * (PERCENTAGES_SEARCH_RANGE / 100);

            DoublyLinkedQueue<TreeNode> mainQueue = new DoublyLinkedQueue<TreeNode>();
            DoublyLinkedQueue<Box> rangedBoxes = new DoublyLinkedQueue<Box>();
            foreach (TreeNode item in _main.GetRange(width, newWidth))
                mainQueue.EnQueue(item);

            foreach (TreeNode item in mainQueue)
                foreach (BSTree<double, Box>.TreeNode b in item.Value.GetRange(height, newHeight))
                    rangedBoxes.EnQueue(b.Value);

            return rangedBoxes;
        }
        void CloneTree() // Creates a clone tree of the main existing 
        {
            _cloneTree = new BSTree<double, BSTree<double, Box>>();
            foreach (Box box in this)
            {
                var copy = (Box)box.Clone();
                if (_cloneTree.FindValue( /*Tkey*/ copy.Width, /*Tvalue*/ out BSTree<double, Box> yTree))
                    if (yTree.FindValue(copy.Height, out Box current))
                        current.Amount += copy.Amount; // if the box exist already just add the amount
                    else
                        yTree.Add(box.Height, copy);
                else // if width dimension doesn't exist
                {
                    yTree = new BSTree<double, Box>(copy.Height, copy);
                    _cloneTree.Add(box.Width, yTree);
                }
            }
            #region Two foreach loop that run at the same time
            //var t1 = Task.Factory.StartNew(() =>
            //{
            //    foreach (TreeNode node in _main)
            //        temp = node.Value;

            //});

            //var t2 = Task.Factory.StartNew(() =>
            //{
            //    foreach (TreeNode node in _cloneTree)
            //        node.Value = temp.CloneBSTree();
            //});

            //// This will block the thread until both tasks have completed
            //Task.WaitAll(t1, t2);
            #endregion
        }
        void CloneQueue() // Creates a clone queue of the dateQueue existing 
        {
            _cloneQ = new DoublyLinkedQueue<Box>();
            foreach (Box box in _dateQ)
            {
                var clone = (Box)box.Clone();
                _cloneQ.EnQueue(clone);
            }
        }
        /// <summary>
        /// IEnumrable function which retrieves all boxes in the store repository from the Tree search InOrder way
        /// </summary>
        /// <returns></returns>
        public IEnumerator GetEnumerator()
        {
            foreach (var item in _main.OrderPick(Traversal.InOrder))
                if (item is BSTree<double, Box> bst)
                    foreach (Box y in bst.OrderPick(Traversal.InOrder))
                        yield return y;
        }
    }
}