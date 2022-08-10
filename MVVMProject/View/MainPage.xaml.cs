using DAL;
using Model;
using System;
using System.Diagnostics;
using System.Text.Json;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace MVVMProject
{
    public sealed partial class MainPage : Page
    {
        public MainPage() => this.InitializeComponent();

        #region MyComfort
        readonly static DispatcherTimer timer = timer = new DispatcherTimer();
        static DateTime date = DateTime.Today;
        void Init_Timer() // Initialize Timer when app loaded
        {
            timer.Interval = new TimeSpan(0, 0, 0, 5);
            timer.Tick += ManageTmr_Tick;
            timer.Start();
        }
        void ManageTmr_Tick(object sender, object e) // Deletes front box if DateDiffernce is 0 - every 24 hours
        {
            if (!(date < DateTime.Today)) return;
            //while (mv.EnumQu.Front.Data.DateDifference <= 0)
            //    mv.BoxBST.Remove(mv.EnumQu.Front.Data);
        }
        void ClearBtn_Click(object sender, RoutedEventArgs e) /* Refresh(); */
        {
            WidthTB.Text = string.Empty;
            HeightTB.Text = string.Empty;
            AmountTB.Text = string.Empty;
            AddAmountTB.Text = string.Empty;
            AddWidthTB.Text = string.Empty;
            AddHeightTB.Text = string.Empty;
            AmountRemoveTB.Text = string.Empty;
        }
        #region Copy to ClipBoard Height and Width
        void BoxesLV_Tapped(object sender, TappedRoutedEventArgs e) // Copy width to ClipBoard
        {
            if (BoxesLV.SelectedIndex >= 0)
            {
                DataPackage dp = new DataPackage(); // Copy To clipboard the guid of the selected item
                dp.SetText($"{((Box)BoxesLV?.SelectedItem)?.Width}");
                Clipboard.SetContent(dp);
            }
        }
        void BoxesLV_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e) // Copy height to ClipBoard 
        {
            if (BoxesLV.SelectedIndex >= 0)
            {
                WidthTB.Text = ((Box)BoxesLV?.SelectedItem)?.Width.ToString();
                HeightTB.Text = ((Box)BoxesLV?.SelectedItem)?.Height.ToString();
                DataPackage dp = new DataPackage(); // Copy To clipboard the guid of the selected item
                dp.SetText($"{((Box)BoxesLV?.SelectedItem)?.Height}");
                Clipboard.SetContent(dp);
            }
        }
        #endregion
        #region Json
        readonly string fileName = "exposure.json";
        //async void SaveData(string path, object data)
        //{
        //    using (StreamWriter file = File.CreateText(path))
        //    {
        //        JsonSerializer serializer = new JsonSerializer();
        //        //serialize object directly into file stream
        //        serializer.Serialize(file, data);
        //    }
        //}
        async void LoadDataBaseJson()
        {
            try
            {
                StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
                StorageFile sampleFile = await storageFolder.GetFileAsync(fileName);
                string jsonFromFile = await FileIO.ReadTextAsync(sampleFile);
                DataBase db = JsonSerializer.Deserialize<DataBase>(jsonFromFile);
            }
            catch (NotSupportedException ex) when (ex.Message.Equals("Deserialization of types without a parameterless constructor, a singular parameterized constructor, or a parameterized constructor annotated with 'JsonConstructorAttribute' is not supported. Type 'Library.DAL.DataBase'. Path: $ | LineNumber: 0 | BytePositionInLine: 1.")) { }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }
        }
        async void Save_Click(object sender, TappedRoutedEventArgs e)
        {
            try
            {
                var options = new JsonSerializerOptions(JsonSerializerDefaults.Web);
                var appFolder = ApplicationData.Current.LocalFolder;
                var file = await appFolder.CreateFileAsync(fileName, CreationCollisionOption.OpenIfExists);
                string jsonString = JsonSerializer.Serialize<DataBase>(DataBase.Instance, options);
                await FileIO.WriteTextAsync(file, jsonString);

                Debug.WriteLine(String.Format($"File is located at {file.Path}"));
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }
        }
        void Exit_Click(object sender, RoutedEventArgs e)
        {
            Save_Click(this, null);
            Application.Current.Exit();
        }
        #endregion
        private void BoxesLV_SelectionChanged(object sender, SelectionChangedEventArgs e) { }
        #endregion
    }
}