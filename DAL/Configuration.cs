using Newtonsoft.Json;
using System;
using System.IO;

namespace DAL
{
    public class Configuration
    {
        public class Config
        {
            public double PercentagesSearchRange { get; set; }
            public int AddExpiryDate { get; set; }
            public int WarningQuantity { get; set; }
            public int MaxAmountOfBoxes { get; set; }
            public int DaysToExpire { get; set; }
        }
        public Config Data { get; set; } = new Config();
        public Configuration()
        {
            try
            {
                var configPath = Path.Combine(Environment.CurrentDirectory, "TextFiles\\Config.json");
                var raw = File.ReadAllText(configPath);
                Data = JsonConvert.DeserializeObject<Config>(raw);
            }
           catch (Exception)
            {
                Data.WarningQuantity = 5;
                Data.AddExpiryDate = 32;
                Data.MaxAmountOfBoxes = 40;
                Data.PercentagesSearchRange = 20;
                Data.DaysToExpire = 30;
            }
        }
    }
}