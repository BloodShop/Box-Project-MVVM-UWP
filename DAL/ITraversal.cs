using System;

namespace DAL
{
    interface ITraversal
    {
        void InOrder(Action<string> action);
        void PreOrder(Action<string> action);
        void PostOrder(Action<string> action);
        void LevelOrder(Action<string> action);
        void RightInOrder(Action<string> action);
    }
}