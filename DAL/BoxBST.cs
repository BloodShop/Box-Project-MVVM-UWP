using Model;
using Model.DataStructures;
using System;
using System.Collections;
using System.Collections.Generic;
using static Model.DataStructures.BSTree<double, Model.DataStructures.BSTree<double, Model.Box>>;

namespace DAL
{
    public class BoxesBST : IEnumerable //, ITraversal
    {
        static bool _init = true;
        static Configuration _config;

        static BSTree<double, BSTree<double, Box>> _main;
        static BSTree<double, BSTree<double, Box>> _cloneTree;
        static DoublyLinkedQueue<Box> _dateQ;
        static DoublyLinkedQueue<Box> _cloneQ;

        public double PERCENTAGES_SEARCH_RANGE { get; private set; }
        public int ADD_EXPIRY_DAYS { get; private set; }
        public int WARNING_QUANTITY { get; private set; }
        public int MAX_AMOUNT_BOXES { get; private set; }

        public Action Retrieve { get => () => { _main = _cloneTree; _dateQ = _cloneQ; }; }
        public DoublyLinkedQueue<Box> DateQ { get => _dateQ; }

        public BoxesBST() => Init();
        void Init() // Initialized once 
        {
            if (_init)
            {
                _config = new Configuration();
                PERCENTAGES_SEARCH_RANGE = _config.Data.PercentagesSearchRange;
                ADD_EXPIRY_DAYS = _config.Data.AddExpiryDate;
                WARNING_QUANTITY = _config.Data.WarningQuantity;
                MAX_AMOUNT_BOXES = _config.Data.MaxAmountOfBoxes;

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
        public void Add(Box box)
        {
            if (box.Amount == 0) return;

            if (_main.FindValue(box.Width, out BSTree<double, Box> yTree))
                if (yTree.FindValue(box.Height, out Box current))
                    current.Amount += box.Amount; // if the box exist already just add the amount
                else
                {
                    yTree.Add(box.Height, box);
                    box.SelfRefrence = _dateQ.Add(box, (x) => x.DateDifference < box.DateDifference);
                }
            else // if width dimension doesn't exist
            {
                yTree = new BSTree<double, Box>(box.Height, box);
                _main.Add(box.Width, yTree);
                box.SelfRefrence = _dateQ.Add(box, (x) => x.DateDifference < box.DateDifference);
            }
        }
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
                        _dateQ.DeleteNode(box.SelfRefrence); // O(1)
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
                    { // Lastly Delete the box quantity to 0 

                        Remove(box, 40);
                    }
                }
                else // (box.Amount > amount)
                {
                    box.AmountBought = amount;
                    box.Amount -= amount;
                    box.LastUsedDate = DateTime.Now;
                    box.ExpirationDate = box.ExpirationDate.AddDays(ADD_EXPIRY_DAYS);
                    amount = 0;
                    yield return box;
                    {
                        _dateQ.DeleteNode(box.SelfRefrence); // O(1)
                        box.SelfRefrence = _dateQ.Add(box, (x) => x.DateDifference < box.DateDifference);
                    }
                }
            }
        }
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
        void CloneTree()
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
        void CloneQueue()
        {
            _cloneQ = new DoublyLinkedQueue<Box>();
            foreach (Box box in _dateQ)
            {
                var clone = (Box)box.Clone();
                _cloneQ.Add(clone, (x) => x.DateDifference < box.DateDifference);
            }
        }
        public IEnumerator GetEnumerator()
        {
            foreach (var item in _main.OrderPick(Traversal.InOrder))
                if (item is BSTree<double, Box> bst)
                    foreach (Box y in bst.OrderPick(Traversal.InOrder))
                        yield return y;
        }
    }
}