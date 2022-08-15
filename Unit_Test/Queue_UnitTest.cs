using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model;
using Model.DataStructures;
using System.Diagnostics;

namespace Unit_Test
{
    [TestClass]
    public class Queue_UnitTest
    {
        readonly DoublyLinkedQueue<Box> queue = new DoublyLinkedQueue<Box>();

        [TestMethod] public void En_De_queue_Check()
        {
            queue.EnQueue(new Box(1,2,3));
            queue.EnQueue(new Box(2,73,32));
            var b1 = queue.DeQueue();
            var b2 = queue.DeQueue();
            var b3 = queue.DeQueue();
            Assert.IsTrue(b1.Width == 1 && b1.Height == 2 && b1.Amount == 3);
            Assert.IsTrue(b2.Width == 2 && b2.Height == 73 && b2.Amount == 32);
            Assert.IsNull(b3);
        }
        [TestMethod] public void EmptyQueueCheck()
        {
            queue.EnQueue(new Box(1, 2, 3));
            queue.EnQueue(new Box(2, 73, 32));
            queue.DeQueue();
            queue.DeQueue();
            Assert.IsTrue(queue.IsEmpty());
        }
        [TestMethod] public void PeekFront()
        {
            queue.EnQueue(new Box(1, 2, 3));
            queue.EnQueue(new Box(2, 73, 32));
            var b1 = queue.Peek();
            var b2 = queue.DeQueue();
            Assert.IsTrue(b1 == b2);
        }
        [TestMethod] public void DeleteNode_O_1()
        {
            var b1 = new Box(1, 2, 3);
            b1.SelfRefrence = queue.EnQueue(b1);
            var b2 = new Box(2, 73, 91);
            b2.SelfRefrence = queue.EnQueue(b2);
            var b3 = new Box(5, 4, 1);
            b3.SelfRefrence = queue.EnQueue(b3);

            queue.DeleteNode(b1.SelfRefrence);
            var b22 = queue.DeQueue();
            Assert.IsTrue(b22 == b2);

            var b33 = queue.DeQueue();
            Assert.IsTrue(b33 == b3);
            Assert.IsTrue(queue.IsEmpty());
        }
        [TestMethod] public void DebugEnumerator()
        {
            queue.EnQueue(new Box(1, 2, 3));
            queue.EnQueue(new Box(2, 73, 32));
            queue.EnQueue(new Box(5, 100, 32));
            queue.EnQueue(new Box(6, 22, 21));
            queue.EnQueue(new Box(7, 3, 32));
            queue.EnQueue(new Box(2, 1, 32));
            foreach (Box box in queue)
                Debug.WriteLine(box.Width + " " + box.Height);
        }
        [TestMethod] public void ClearQueueCheck()
        {
            queue.EnQueue(new Box(1, 2, 3));
            queue.EnQueue(new Box(2, 73, 32));
            queue.EnQueue(new Box(5, 100, 32));
            queue.EnQueue(new Box(6, 22, 21));
            queue.EnQueue(new Box(7, 3, 32));
            queue.EnQueue(new Box(2, 1, 32));
            Assert.IsFalse(queue.IsEmpty());
            queue.Clear();
            Assert.IsTrue(queue.IsEmpty());
            foreach (Box box in queue)
                Debug.WriteLine(box.Width + " " + box.Height);
        }

    }
}
