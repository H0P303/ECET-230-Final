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

        private SolarCalc solarCalc = new SolarCalc();

        public double[] data { get; set; }
        public double[] analogVoltage = new double[6];
        private string[] analogPins = { "LED3", "LED2", "LED1", "Solar Voltage", "Battery Voltage", "\"Reference Voltage\"" };

        /// <summary>
        /// Opens savefile dialog allowing user to select save location.
        /// Must be done before com port can be opened.
        /// </summary>
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

        /// <summary>
        /// Parses analog value from received packet
        /// </summary>
        /// <param name="newPacket"></param>
        public void parser(string newPacket)
        {
            int packetNR = Convert.ToInt32(newPacket.Substring(3, 3));
            //for (int i = 0; i < 6; i++)
            //{
            //    //For index 0 the substring starts at 6. For index 1 the substring starts at 6 + 4 = 10 etc.
            //    analogVoltage[i] = Convert.ToDouble(newPacket.Substring(6 + (i * 4), 4));
            //    analogVoltage[i] = solarCalc.averageVoltage(analogVoltage[i], i); //Adds voltage reading to an incrementing location inside of 2d array
            //}

            saver(solarCalc.ParseSolarData(newPacket), packetNR);
        }

        //https://www.newtonsoft.com/json/help/html/ReadingWritingJSON.htm
        /// <summary>
        /// https://www.newtonsoft.com/json/help/html/ReadingWritingJSON.htm
        /// Writes a new object for each packet that contains the read analog value
        /// for the 6 analog inputs on the meadow board as well as voltages and currents
        /// </summary>
        /// <param name="analogV"></param>  //Value of the analog reading
        /// <param name="ye"></param>   //packetNR
        private void saver(double[] analogV, int packetNR)
        {
            writer.WriteStartObject();
            writer.WritePropertyName($"packet nr: {packetNR}");
            writer.WriteStartArray();
            writer.WriteStartObject();

            //Loops through each analog value inside packet
            for (int i = 0; i < 6; i++)
            {
                //writer.WriteStartObject();
                writer.WritePropertyName($"AnalogValue{i}");
                writer.WriteValue(analogV[i]);
                writer.WriteComment(analogPins[i]);
                //writer.WriteEndObject();
            }
            writer.WritePropertyName("SolarVoltage");
            writer.WriteValue(solarCalc.GetVoltage(solarCalc.analogVoltage[3]));
            writer.WritePropertyName("BatteryVoltage");
            writer.WriteValue(solarCalc.GetVoltage(solarCalc.analogVoltage[4]));
            writer.WritePropertyName("BatteryCurrent");
            writer.WriteValue(solarCalc.GetCurrent(solarCalc.analogVoltage[5], solarCalc.analogVoltage[4]));
            writer.WritePropertyName("LED1-Current");
            writer.WriteValue(solarCalc.LEDCurrent(solarCalc.analogVoltage[5], solarCalc.analogVoltage[2]));
            writer.WritePropertyName("LED2-Current");
            writer.WriteValue(solarCalc.LEDCurrent(solarCalc.analogVoltage[5], solarCalc.analogVoltage[1]));
            writer.WritePropertyName("LED3-Current");
            writer.WriteValue(solarCalc.LEDCurrent(solarCalc.analogVoltage[5], solarCalc.analogVoltage[0]));

            writer.WriteEndObject();
            writer.WriteEndArray();
            writer.WriteEndObject();
        }
    }
}