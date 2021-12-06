﻿using Microsoft.Win32;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace MeadowSolar
{
    internal class FileWindowHandler
    {
        public string file { get; set; }
        public string JsonFile { get; set; }

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
                file = openFileDialog1.FileName;
            }
            deSerializer();
        }

        /// <summary>
        /// DeSerializes the Json File that the User Selected
        /// https://stackoverflow.com/questions/18192357/deserializing-json-object-array-with-json-net
        /// https://www.c-sharpcorner.com/article/c-sharp-list/
        /// </summary>
        public void deSerializer()
        {
            JsonFile = File.ReadAllText(file);
            //System.Diagnostics.Debug.WriteLine(JsonFile);
            V = JsonConvert.DeserializeObject<List<Packets>>(JsonFile); //DeSerializes the Json Object to V

            //return V;
            //System.Diagnostics.Debug.WriteLine(V[0].Values.PacketNR);
        }
    }

    public class Packets
    {
        [JsonProperty("Packet")]
        public Values Values { get; set; }
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