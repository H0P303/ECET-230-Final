using Microsoft.Win32;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using Newtonsoft.Json.Schema;

namespace MeadowSolar
{
    internal class FileWindowHandler
    {
        public string file { get; set; }
        private string pFile;
        public string JsonFile { get; set; }
        public bool fileSelected { get; set; }

        //private string schemaJson = @"{
        //    'description': 'A person',
        //    'type': 'object',
        //    'properties':
        //    {
        //        'name': {'type':'string'},
        //        'hobbies': {
        //            'type': 'array',
        //            'items': {'type':'string'}
        //        }
        //    }
        //}";

        //private JSchema schema = JSchema.Parse(@"{{
        //    '$schema': 'http://json-schema.org/draft-04/schema#',
        //    'type': 'array',
        //    'items': [
        //      {
        //        'type': 'object',
        //        'properties': {
        //          'Packet': {
        //            'type': 'object',
        //            'properties': {
        //              'PacketNR': {'type': 'integer'},
        //              'AnalogValue0': {'type': 'number'},
        //              'AnalogValue1': {'type': 'number'},
        //              'AnalogValue2': {'type': 'number'},
        //              'AnalogValue3': {'type': 'number'},
        //              'AnalogValue4': {'type': 'number'},
        //              'AnalogValue5': {'type': 'number'},
        //              'SolarVoltage': {'type': 'string'},
        //              'BatteryVoltage': {'type': 'string'},
        //              'BatteryCurrent': {'type': 'string'},
        //              'LED1_Current': {'type': 'string'},
        //              'LED2_Current': {'type': 'string'},
        //              'LED3_Current': {'type': 'string'}
        //            }
        //        }
        //      }
        //    ]
        //}}");

        public string[] NrPacketsAvailable = new string[100];
        public List<int> N = new List<int>();
        public List<double> C_An0 = new List<double>();
        public List<Packets> V { get; set; }

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
                pFile = openFileDialog1.FileName;
                file = pFile;
            }

            if (File.Exists(file))
            {
                fileSelected = true;
            }
            else
            {
                fileSelected = false;
            }

            deSerializer();
        }

        /// <summary>
        /// DeSerializes the Json File that the User Selected
        /// https://stackoverflow.com/questions/18192357/deserializing-json-object-array-with-json-net
        /// https://www.c-sharpcorner.com/article/c-sharp-list/
        /// https://stackoverflow.com/questions/202813/adding-values-to-a-c-sharp-array
        /// </summary>
        public void deSerializer()
        {
            if (File.Exists(file))
            {
                JsonFile = File.ReadAllText(pFile);
                //System.Diagnostics.Debug.WriteLine(JsonFile);
                V = JsonConvert.DeserializeObject<List<Packets>>(JsonFile); //DeSerializes the Json Object to V

                //System.Diagnostics.Debug.WriteLine(V[0].Packet.PacketNR);
                try
                {
                    for (int i = 0; i < V.Count; i++)
                    {
                        //System.Diagnostics.Debug.Write($"{V[i].Packet.PacketNR} \n");
                        //NrPacketsAvailable[i] = V[i].Packet.PacketNR.ToString();
                        //System.Diagnostics.Debug.Write($"{PacketsNRAvailable[i]} \n");

                        N.Add(V[i].Packet.PacketNR);
                        C_An0.Add(V[i].Packet.AnalogValue0);
                    }
                }
                catch (System.NullReferenceException)
                {
                    MessageBox.Show("Select Valid Json File", "Invalid Json File", MessageBoxButton.OK);
                    SelectFile();
                }

                //fileSelected = true;
            }
            else
            {
                //jsonFileWindow jsonFileWindow = new jsonFileWindow();
                //jsonFileWindow.Close();
                //fileSelected = false;
                MessageBox.Show("Select file to continue", "File not selected", MessageBoxButton.OK);
                SelectFile();
            }
        }
    }

    public class Packets
    {
        [JsonProperty("Packet")]
        public Values Packet { get; set; }
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
        [JsonProperty("PacketNR")]
        public int PacketNR { get; set; }

        [JsonProperty("AnalogValue0")]
        public double AnalogValue0 { get; set; }

        [JsonProperty("AnalogValue1")]
        public double AnalogValue1 { get; set; }

        [JsonProperty("AnalogValue2")]
        public double AnalogValue2 { get; set; }

        [JsonProperty("AnalogValue3")]
        public double AnalogValue3 { get; set; }

        [JsonProperty("AnalogValue4")]
        public double AnalogValue4 { get; set; }

        [JsonProperty("AnalogValue5")]
        public double AnalogValue5 { get; set; }

        [JsonProperty("SolarVoltage")]
        public string SolarVoltage { get; set; }

        [JsonProperty("BatteryVoltage")]
        public string BatteryVoltage { get; set; }

        [JsonProperty("BatteryCurrent")]
        public string BatteryCurrent { get; set; }

        [JsonProperty("LED1_Current")]
        public string LED1_Current { get; set; }

        [JsonProperty("LED2_Current")]
        public string LED2_Current { get; set; }

        [JsonProperty("LED3_Current")]
        public string LED3_Current { get; set; }
    }
}