using Model.DataStructures;
using Newtonsoft.Json;
using System;

namespace Model
{
    public class Box : IFormattable, ICloneable, IComparable<Box>
    {
        [JsonIgnore] public Func<int, bool> WarningQnt => (x) => _amount <= x; 
        [JsonIgnore] public QNode<Box> SelfRefrence;

        int _amount;
        int _amountBought;
        DateTime _lastUsedDate = DateTime.Now;
        double _width;
        double _height;

        public int Amount
        {
            get => _amount;
            set
            {
                if (value > 0) _amount = value;
                else _amount = 0;
            }
        }
        public int AmountBought { get => _amountBought; set => _amountBought = value; }
        public double Width { get => _width; set => _width = value; }
        public double Height { get => _height; set => _height = value; }
        public DateTime LastUsedDate { get => _lastUsedDate; set => _lastUsedDate = value; }
        public int DateDifference  => (DateTime.Now - _lastUsedDate).Days;  
        /// <summary>
        /// Initialize Box with three parameters
        /// </summary>
        /// <param name="width">Box's Width</param>
        /// <param name="height">Box's Height</param>
        /// <param name="amount">Box's amount</param>
        public Box(double width, double height, int amount)
        {
            _width = Math.Round(width, 2); // Rounds the double after two digits after the dot.
            _height = Math.Round(height, 2);
            Amount = amount;
        }
        public object Clone() => MemberwiseClone();
        public override string ToString() => $"Box's Dimensions: \nWidth - {_width:0.00} \nHeight - {_height:0.00}. " +
            $"\nAmount Left: {_amount} \nDate: {_lastUsedDate:d}\nExpiry Date: {_lastUsedDate.AddDays(30):d} \n";
        public string ToString(string format, IFormatProvider formatProvider)
        {
            switch (format)
            {
                case "s": return $"Box's Dimensions: \nWidth - {_width} \nHeight - {_height}.";
                default: return ToString();
            }
        }
        public int CompareTo(Box other) // Compare boxes by there Dimensions first width then height
        {
            int compWidth = _width.CompareTo(other._width);
            if (compWidth > 0) return 1;
            else if (compWidth < 0) return -1;
            else
            {
                int compHeight = _height.CompareTo(other._height);
                if (compHeight > 0) return 1;
                else if (compHeight < 0) return -1;
                else return 0;
            }
        }
    }
}