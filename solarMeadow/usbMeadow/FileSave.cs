using System;
using System.Collections.Generic;
using System.Text;

namespace MeadowSolar
{
    internal class FileSave
    {
        //Average calc variables
        private static double ResitorValue;

        public double[] analogVoltage = new double[6];
        private const int numberOfSamples = 5;
        private static int currentIndex = 0;

        //https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/arrays/multidimensional-arrays
        private double[,] slidingWindowVoltage = new double[6, numberOfSamples];

        /// <summary>
        /// Parses solar data received from serial data
        /// </summary>
        /// <param name="newPacket"></param>
        public void ParseSolarData(string newPacket)
        {
            for (int i = 0; i < 6; i++)
            {
                //For index 0 the substring starts at 6. For index 1 the substring starts at 6 + 4 = 10 etc.
                analogVoltage[i] = Convert.ToDouble(newPacket.Substring(6 + (i * 4), 4));
                analogVoltage[i] = averageVoltage(analogVoltage[i], i); //Adds voltage reading to an incrementing location inside of 2d array
            }
        }

        private double averageVoltage(double voltageToAverage, int indexOfAnalog)
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
    }
}