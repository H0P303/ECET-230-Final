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
                writer.WriteStartArray();
                //writer.WriteStartObject();
                //writer.WritePropertyName("Packets");
                //writer.WriteStartArray();
            }
        }

        /// <summary>
        /// Parses analog value from received packet
        /// </summary>
        /// <param name="newPacket"></param>
        public void parser(string newPacket)
        {
            int packetNR = Convert.ToInt32(newPacket.Substring(3, 3));

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
            //writer.WritePropertyName($"Packet {packetNR}");
            writer.WriteStartObject();
            writer.WritePropertyName("Packet");
            //writer.WriteStartArray();
            writer.WriteStartObject();
            writer.WritePropertyName("PacketNR");
            writer.WriteValue(packetNR);

            //Loops through each analog value inside packet
            for (int i = 0; i < 6; i++)
            {
                writer.WritePropertyName($"AnalogValue{i}");
                writer.WriteValue(analogV[i]);
                writer.WriteComment(analogPins[i]);
            }
            writer.WritePropertyName("SolarVoltage");
            writer.WriteValue(solarCalc.GetVoltage(solarCalc.analogVoltage[3]));
            writer.WritePropertyName("BatteryVoltage");
            writer.WriteValue(solarCalc.GetVoltage(solarCalc.analogVoltage[4]));
            writer.WritePropertyName("BatteryCurrent");
            writer.WriteValue(solarCalc.GetCurrent(solarCalc.analogVoltage[5], solarCalc.analogVoltage[4]));
            writer.WritePropertyName("LED1_Current");
            writer.WriteValue(solarCalc.LEDCurrent(solarCalc.analogVoltage[5], solarCalc.analogVoltage[2]));
            writer.WritePropertyName("LED2_Current");
            writer.WriteValue(solarCalc.LEDCurrent(solarCalc.analogVoltage[5], solarCalc.analogVoltage[1]));
            writer.WritePropertyName("LED3_Current");
            writer.WriteValue(solarCalc.LEDCurrent(solarCalc.analogVoltage[5], solarCalc.analogVoltage[0]));

            writer.WriteEndObject();
            writer.WriteEndObject();
            //writer.WriteEndArray();
        }

        /// <summary>
        /// Terminates the Json Doc.
        /// Run when COM Port is closed
        /// </summary>
        public void finish()
        {
            writer.WriteEndArray();
            //writer.WriteEndObject();
            writer.Close();
        }
    }
}