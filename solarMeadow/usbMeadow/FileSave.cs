using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Diagnostics;

namespace MeadowSolar
{
    internal class FileSave
    {
        public JsonWriter writer;

        public double[] data { get; set; }
        public double[] analogVoltage = new double[6];

        public void saveToJson()
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "JSON File (*.json)|*.json";

            if (saveFileDialog1.ShowDialog() == true)
            {
                //https://docs.microsoft.com/en-us/dotnet/api/system.io.streamwriter?view=net-6.0
                StreamWriter logFile = File.CreateText(saveFileDialog1.FileName); //Creates LogFile
                logFile.AutoFlush = true;

                writer = new JsonTextWriter(logFile);
                writer.Formatting = Formatting.Indented;
                writer.WriteStartObject();
                writer.WritePropertyName("Packets");
                writer.WriteStartArray();
            }
        }

        public void parser(string newPacket)
        {
            for (int i = 0; i < 6; i++)
            {
                //For index 0 the substring starts at 6. For index 1 the substring starts at 6 + 4 = 10 etc.
                analogVoltage[i] = Convert.ToDouble(newPacket.Substring(6 + (i * 4), 4));
                saver(analogVoltage);
            }
        }

        private void saver(double[] analogV)
        {
            for (int i = 0; i < 6; i++)
            {
                writer.WriteStartObject();
                writer.WritePropertyName($"AnalogValue{i}");
                writer.WriteValue(analogV[i]);
                writer.WriteEndObject();
            }
        }
    }
}