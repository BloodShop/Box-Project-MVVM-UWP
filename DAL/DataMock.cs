using Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;

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
                        //InitDataBaseJson();
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
                Boxes[i].LastUsedDate = new DateTime(2022,7,01);
            }
        }
        public static void SaveJson(ObservableCollection<Box> collection)
        {
            try
            {
                var configPath = Path.Combine(Environment.CurrentDirectory, "TextFiles/DataBase.json"); // ms-appx:///
                var json = JsonConvert.SerializeObject(collection, Formatting.Indented);
                File.WriteAllText(configPath, json);
            }
            catch (Exception) { }
        }
        static void InitDataBaseJson()
        {
            try
            {
                var configPath = Path.Combine(Environment.CurrentDirectory, "TextFiles\\DataBase.json");
                var raw = File.ReadAllText(configPath);
                if (raw != null) _instance = JsonConvert.DeserializeObject<DataBase>(raw);
            }
            catch (Exception) { _instance = new DataBase(); }
        }
    }
}