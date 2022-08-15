using System;
using System.Diagnostics;
using DAL;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model;
using Model.DataStructures;

namespace Unit_Test
{
    [TestClass]
    public class BSTree
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
        public DoublyLinkedQueue<Box> DateQ => _dateQ;

        public BSTree()
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
        /// <summary>
        /// If Box Exicts Add to the box the given amount
        /// </summary>
        /// <param name="box">The box that exicts and you want to add to it curtain amount</param>
        /// <param name="amount">The amount to add to the given box</param>
        public void AddToExicting(Box box, int amount)
        {
            if (box.Amount + amount <= MAX_AMOUNT_BOXES)
                box.Amount += amount;
            else box.Amount = MAX_AMOUNT_BOXES;
        }
        /// <summary>
        /// Add's a box to the store repository
        /// </summary>
        /// <param name="box">The given box to add to the store <see cref="Box"/></param>
        public void Add(Box box)  // Time complexity O (log n)
        {
            if (box.Amount == 0) return;

            if (_main.FindValue(box.Width, out BSTree<double, Box> yTree))
                if (yTree.FindValue(box.Height, out Box current))
                {
                    AddToExicting(current, current.Amount);
                    _dateQ.EnQueue(current);
                }
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


        [TestMethod] public void IsNotEmpty() => Assert.IsFalse(_main.IsEmpty());
        [TestMethod] public void IsBst() => Assert.IsTrue(_main.IsBST());
        [TestMethod] public void IsInOrder()
        {
            foreach (BSTree<double, Box> bst in _main.OrderPick(Traversal.InOrder))
                foreach (Box box in bst.OrderPick(Traversal.InOrder))
                    Debug.WriteLine(box.Width + " " + box.Height);
        }
        [TestMethod] public void IsPreOrder()
        {
            foreach (BSTree<double, Box> bst in _main.OrderPick(Traversal.PostOrder))
                foreach (Box box in bst.OrderPick(Traversal.PostOrder))
                    Debug.WriteLine(box.Width + " " + box.Height);
        }
        [TestMethod] public void IsPostOrder()
        {
            foreach (BSTree<double, Box> bst in _main.OrderPick(Traversal.PostOrder))
                foreach (Box box in bst.OrderPick(Traversal.PostOrder))
                    Debug.WriteLine(box.Width + " " + box.Height);
        }
        [TestMethod] public void IsLevelOrder()
        {
            foreach (BSTree<double, Box> bst in _main.OrderPick(Traversal.LevelOrder))
                foreach (Box box in bst.OrderPick(Traversal.LevelOrder))
                    Debug.WriteLine(box.Width + " " + box.Height);
        }
        [TestMethod] public void IsRightInOrder()
        {
            foreach (BSTree<double, Box> bst in _main.OrderPick(Traversal.RightInOrder))
                foreach (Box box in bst.OrderPick(Traversal.RightInOrder))
                    Debug.WriteLine(box.Width + " " + box.Height);
        }
    }
}
