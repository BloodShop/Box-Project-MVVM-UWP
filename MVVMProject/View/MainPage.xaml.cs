﻿using DAL;
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
        #region Copy to ClipBoard Height and Width
        void Refresh() => Frame.Navigate(typeof(MainPage));
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
        void BoxesLV_Tapped(object sender, TappedRoutedEventArgs e) { }
        void BoxesLV_SelectionChanged(object sender, SelectionChangedEventArgs e) { }
        void BoxesLV_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e) // Copy height to ClipBoard 
        {
            if (BoxesLV.SelectedIndex >= 0)
            {
                WidthTB.Text = ((Box)BoxesLV?.SelectedItem)?.Width.ToString();
                HeightTB.Text = ((Box)BoxesLV?.SelectedItem)?.Height.ToString();
                DataPackage dp = new DataPackage(); // Copy To clipboard the Height of the selected item
                dp.SetText($"{((Box)BoxesLV?.SelectedItem)?.Height}");
                Clipboard.SetContent(dp);
            }
        }
        void QueueDateLV_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            if (QueueDateLV.SelectedIndex >= 0)
            {
                AddWidthTB.Text = ((Box)QueueDateLV?.SelectedItem)?.Width.ToString();
                AddHeightTB.Text = ((Box)QueueDateLV?.SelectedItem)?.Height.ToString();
                DataPackage dp = new DataPackage(); // Copy To clipboard the Height of the selected item
                dp.SetText($"{((Box)QueueDateLV?.SelectedItem)?.Height}");
                Clipboard.SetContent(dp);
            }
        }
        #endregion
        #region Json
        readonly string fileName = "exposure.json";
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
                string jsonString = JsonSerializer.Serialize(DataBase.Instance, options);
                await FileIO.WriteTextAsync(file, jsonString);
                Debug.WriteLine(String.Format($"File is located at {file.Path}"));
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }
        }
        void Exit_Click(object sender, RoutedEventArgs e) { }
        #endregion

        #endregion

    }
}