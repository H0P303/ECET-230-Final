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

            System.Diagnostics.Debug.WriteLine(V.PacketNR);
        }
    }

    /// <summary>
    /// Json Object Template used for Deserialization.
    /// </summary>
    public class Values
    {
        //"packet nr:": 311,
        //"AnalogValue0": 351.0/*LED3*/,
        //"AnalogValue1": 350.8/*LED2*/,
        //"AnalogValue2": 351.0/*LED1*/,
        //"AnalogValue3": 246.6/*Solar Voltage*/,
        //"AnalogValue4": 351.4/*Battery Voltage*/,
        //"AnalogValue5": 350.4/*"Reference Voltage"*/,
        //"SolarVoltage": "0.2 V",
        //"BatteryVoltage": "0.4 V",
        //"BatteryCurrent": " 0.0 mA",
        //"LED1_Current": " 0.0 mA",
        //"LED2_Current": " 0.0 mA",
        //"LED3_Current": " 0.0 mA"

        public int PacketNR { get; set; }
        public int AnalogValue0 { get; set; }
        public int AnalogValue1 { get; set; }
        public int AnalogValue2 { get; set; }
        public int AnalogValue3 { get; set; }
        public int AnalogValue4 { get; set; }
        public int AnalogValue5 { get; set; }
        public int SolarVoltage { get; set; }
        public int BatteryVoltage { get; set; }
        public int BatteryCurrent { get; set; }
        public int LED1_Current { get; set; }
        public int LED2_Current { get; set; }
        public int LED3_Current { get; set; }
    }
}