using Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage;

namespace DAL
{
    public class DataBase
    {
        private static readonly object _lock = new object();
        public static Box[] Boxes { get; private set; } = new Box[100];
        static DataBase _instance;
        public static DataBase Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance == null)
                        _instance = new DataBase();
                    //InitDataBaseJson();
                }
                return _instance;
            }
        }
        DataBase()
        {
            Random r = new Random();
            double m = r.NextDouble() * 100;
            for (int i = 0; i < Boxes.Length; i++)
            {
                if (i % 7 == 0 || i % 5 == 0) m = r.NextDouble() * 100;
                Boxes[i] = new Box(m, r.Next(5, 40), r.Next(1, 20));
                Boxes[i].LastUsedDate = new DateTime(2022, 7, 02);
            }
        }
        public static void SaveDataBaseJson(ObservableCollection<Box> collection)
        {
            try
            {
                //string json = JsonConvert.SerializeObject(collection);
                //var path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + ".json";

                //if (File.Exists(path))
                //    File.WriteAllText(path, json);

                //    using (StreamWriter file = File.CreateText(path))
                //    {
                //        JsonSerializer serializer = new JsonSerializer();
                //        //serialize object directly into file stream
                //        serializer.Serialize(file, json);
                //    }

                // Track where the file is located
                //Debug.WriteLine(String.Format($"File is located at {file.Path}"));

                //var configPath = Path.Combine(Environment.CurrentDirectory, "TextFiles\\DataBase.json"); // ms-appx:///
                //var json = JsonConvert.SerializeObject(collection);

                //using (StreamWriter file = File.CreateText(configPath))
                //{
                //    JsonSerializer serializer = new JsonSerializer();
                //    //serialize object directly into file stream
                //    serializer.Serialize(file, json);
                //}

                //File.WriteAllText(configPath, json);
                // G:\SeLa\ShayTavor\1111MVVMProject\MVVMProject\DAL\TextFiles\DataBase.json
                // G:\SeLa\ShayTavor\1111MVVMProject\MVVMProject\MVVMProject\bin\x86\Debug\AppX\TextFiles\DataBase.json
            }
            catch (Exception) { }
        }
        static void InitDataBaseJson()
        {
            try
            {
                string configPath = Path.Combine(Environment.CurrentDirectory, "TextFiles\\DataBase.json");
                string raw = File.ReadAllText(configPath);
                if (raw != null) _instance = JsonConvert.DeserializeObject<DataBase>(raw);
            }
            catch (Exception) { _instance = new DataBase(); }

            //try
            //{  // Read File path
            //    StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            //    StorageFile sampleFile = await storageFolder.GetFileAsync("DataBase.json");
            //    string jsonFromFile = await FileIO.ReadTextAsync(sampleFile);
            //    Boxes = JsonConvert.DeserializeObject<Box[]>(jsonFromFile);
            //}
            //catch (Exception ex) {  }
        }
    }
}