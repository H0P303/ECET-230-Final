using Meadow.Foundation;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeadowComPort
{
    internal class Grapher
    {
        public int X_Value { get; set; }   //Value read on pin
        public int Y_value { get; set; }   //Packet number
        public int xStart { get; set; }
        public int yStart { get; set; }
        public int xSize { get; set; }  //Size of Graph X
        public int ySize { get; set; }  //Size of Graph Y
        public Color bgColor { get; set; }  //Background color
        public Color fgndColor { get; set; }    //Foreground Color

        public Grapher(int xStart, int yStart, int xSize, int ySize, Color bgColor, Color fgndColor)
        {
            this.xStart = xStart;
            this.yStart = yStart;
            this.xSize = xSize;
            this.ySize = ySize;
            this.bgColor = bgColor;
            this.fgndColor = fgndColor;
        }
    }
}