using Microsoft.Win32;
using Newtonsoft.Json;
using System.IO;

namespace MeadowSolar
{
    internal class FileWindowHandler
    {
        public string file { get; set; }
        public string JsonFile { get; set; }

        /// <summary>
        /// Allows user to select desired file to open.
        /// Will later be DeSerialized and contents are displayed in WPF Window
        /// </summary>
        public void SelectFile()
        {
            //bool fileSelected = false;

            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "JSON Files (*.json)|*.json";

            if (openFileDialog1.ShowDialog() == true)
            {
                FileStream fs = new FileStream(openFileDialog1.FileName, FileMode.Open);
                fs.Close();
                file = openFileDialog1.FileName;
            }
        }

        /// <summary>
        /// DeSerializes the Json File that the User Selected
        /// </summary>
        public void deSerializer()
        {
            JsonFile = File.ReadAllText(file);
            //System.Diagnostics.Debug.WriteLine(JsonFile);
            Values V = JsonConvert.DeserializeObject<Values>(JsonFile); //DeSerializes the Json Object to V

            //foreach (var items in V.Packets)
            //{
            //    System.Diagnostics.Debug.WriteLine(items);
            //}

            System.Diagnostics.Debug.WriteLine(V.PacketNR);
        }
    }

    /// <summary>
    /// Json Object Template used for Deserialization.
    /// </summary>
    public class Values
    {
        //"PacketNR": 97,
        //"AnalogValue0": 79.6/*LED3*/,
        //"AnalogValue1": 79.0/*LED2*/,
        //"AnalogValue2": 79.2/*LED1*/,
        //"AnalogValue3": 177.6/*Solar Voltage*/,
        //"AnalogValue4": 78.2/*Battery Voltage*/,
        //"AnalogValue5": 79.6/*"Reference Voltage"*/,
        //"SolarVoltage": "0.2 V",
        //"BatteryVoltage": "0.1 V",
        //"BatteryCurrent": " 0.0 mA",
        //"LED1_Current": " 0.0 mA",
        //"LED2_Current": " 0.0 mA",
        //"LED3_Current": " 0.0 mA"

        //public string[] Packets { get; set; }

        public int PacketNR { get; set; }
        public double AnalogValue0 { get; set; }
        public double AnalogValue1 { get; set; }
        public double AnalogValue2 { get; set; }
        public double AnalogValue3 { get; set; }
        public double AnalogValue4 { get; set; }
        public double AnalogValue5 { get; set; }
        public string SolarVoltage { get; set; }
        public string BatteryVoltage { get; set; }
        public string BatteryCurrent { get; set; }
        public string LED1_Current { get; set; }
        public string LED2_Current { get; set; }
        public string LED3_Current { get; set; }
    }
}