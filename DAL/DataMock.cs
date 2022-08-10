using Model;
using System;

namespace DAL
{
    public class DataBase
    {
        private static readonly object _lock = new object();
        public static Box[] Boxes { get; private set; } = new Box[20];
        static DataBase _instance;
        public static DataBase Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance == null)
                        _instance = new DataBase();
                }
                return _instance;
            }
        }
        public DataBase(DataBase instance) => _instance = instance; // Json
        DataBase()
        {
            Random r = new Random();
            double m = r.NextDouble() * 100;
            for (int i = 0; i < Boxes.Length; i++)
            {
                if (i % 7 == 0 || i % 5 == 0) m = r.NextDouble() * 100;
                Boxes[i] = new Box(m, r.Next(5, 40), r.Next(1, 20));
            }
        }
    }
}