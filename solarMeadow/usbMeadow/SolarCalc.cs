using System;

namespace MeadowSolar
{
    internal class SolarCalc
    {
        //field

        private static double ResitorValue;
        public double[] analogVoltage = new double[6];
        private const int numberOfSamples = 5;
        private static int currentIndex = 0;

        //https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/arrays/multidimensional-arrays
        private double[,] slidingWindowVoltage = new double[6, numberOfSamples];

        //Constructor takes no arguments
        public SolarCalc()
        {
            ResitorValue = 100.0;
        }

        //Method

        public double[] ParseSolarData(string newPacket)
        {
            for (int i = 0; i < 6; i++)
            {
                //For index 0 the substring starts at 6. For index 1 the substring starts at 6 + 4 = 10 etc.
                analogVoltage[i] = Convert.ToDouble(newPacket.Substring(6 + (i * 4), 4));
                analogVoltage[i] = averageVoltage(analogVoltage[i], i); //Adds voltage reading to an incrementing location inside of 2d array
            }

            return analogVoltage;
        }

        /// <summary>
        /// Passes voltage from solar parse method.
        /// Calculates the average voltage using each data point inside the 2d array.
        /// </summary>
        /// <param name="voltageToAverage"></param>
        /// <param name="indexOfAnalog"></param>
        /// <returns></returns>
        public double averageVoltage(double voltageToAverage, int indexOfAnalog)
        {
            double sum;
            if (currentIndex >= numberOfSamples)
            {
                currentIndex = 0;
            }

            slidingWindowVoltage[indexOfAnalog, currentIndex] = voltageToAverage;
            sum = 0;

            for (int i = 0; i < numberOfSamples; i++)
            {
                sum += slidingWindowVoltage[indexOfAnalog, i];
            }

            if (indexOfAnalog == numberOfSamples)
            {
                currentIndex++;
            }

            return sum / numberOfSamples;
        }

        public string GetVoltage(double analogValue)
        {
            double dAnalogValue = analogValue / 1000.0;
            return dAnalogValue.ToString("0.0 V");
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="analogValue5"></param> //Tied to diode , resistor node.
        /// <param name="shuntResistorAnalog"></param>
        /// <returns></returns>
        public string GetCurrent(double analogValue5, double shuntResistorAnalog)
        {
            double shuntAnalog = analogValue5 - shuntResistorAnalog;
            double dAnalogValue = (shuntAnalog / ResitorValue);
            //https://msdn.microsoft.com/en-us/library/0c899ak8.aspx
            return dAnalogValue.ToString(" 0.0 mA;-0.0 mA; 0.0 mA"); //display format for +, - and 0 values
        }

        public string LEDCurrent(double analogValue5, double shuntResistorAnalog)
        {
            double shuntAnalog = analogValue5 - shuntResistorAnalog;
            double dAnalogValue = (shuntAnalog / ResitorValue);
            if (dAnalogValue < 0)
            {
                dAnalogValue = 0;
            }
            return dAnalogValue.ToString(" 0.0 mA;-0.0 mA; 0.0 mA"); //display format for +, - and 0 values
        }
    }
}