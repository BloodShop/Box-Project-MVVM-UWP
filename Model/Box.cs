using Model.DataStructures;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Model
{
    public class Box : IFormattable, ICloneable/*, IComparable<Box>*/
    {
        public Func<string> BackSupply { get; private set; }
        public Func<int, bool> WarningQnt { get => (x) => _amount <= x; }

        int _amount;
        int _amountBought;
        DateTime _lastUsedDate = DateTime.Now;
        double _width;
        double _height;

        public QNode<Box> SelfRefrence;

        public int Amount
        {
            get => _amount;
            set
            {
                if (value > 0) _amount = value;
                else _amount = 0; // Possiblly be 1 - ignore non-scence
            }
        }
        public int AmountBought { get => _amountBought; set => _amountBought = value; }
        public double Width { get => _width; set => _width = value; }
        public double Height { get => _height; set => _height = value; }
        public DateTime LastUsedDate { get => _lastUsedDate; set => _lastUsedDate = value; }
        public int DateDifference { get => (DateTime.Now - _lastUsedDate).Days;  }
        /// <summary>
        /// Initialize Box with three parameters
        /// </summary>
        /// <param name="width">Box's Width</param>
        /// <param name="height">Box's Height</param>
        /// <param name="amount">Box's amount</param>
        public Box(double width, double height, int amount)
        {
            _width = Math.Round(width, 2);
            _height = Math.Round(height, 2);
            Amount = amount;
        }
        public object Clone() => MemberwiseClone();
        public override string ToString() => $"Box's Dimensions:\nWidth - {_width:0.00}\nHeight - {_height:0.00}." +
            $"\nAmount Left: {_amount}\nDate: {_lastUsedDate:d}\nExpiry Date: {_lastUsedDate.AddDays(30):d}\n";
        public string ToString(string format, IFormatProvider formatProvider)
        {
            switch (format)
            {
                case "s": return $"Box's Dimensions:\nWidth - {_width}\nHeight - {_height}.";
                default: return ToString();
            }
        }
    }
}