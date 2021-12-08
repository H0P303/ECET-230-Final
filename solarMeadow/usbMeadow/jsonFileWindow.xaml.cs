using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Diagnostics;
using System.Windows.Ink;
using System.Windows.Threading;

namespace MeadowSolar
{
    /// <summary>
    /// Interaction logic for jsonFileWindow.xaml
    ///
    /// https://www.youtube.com/watch?v=ZqARpaB39TY&ab_channel=DimitriPatarroyo
    /// </summary>
    public partial class jsonFileWindow : Window
    {
        private FileWindowHandler fileWindowHandler = new FileWindowHandler();
        private List<Ellipse> ellipseList = new List<Ellipse>();
        private List<Line> lineList = new List<Line>();
        private int OldIndex;
        //private int[] PacketNrAvailable;

        public jsonFileWindow()
        {
            InitializeComponent();
        }

        private void JsonWindowMain_Loaded(object sender, RoutedEventArgs e)
        {
            fileWindowHandler.SelectFile();
            JsonWindowMain.Title = $"File Open: {fileWindowHandler.file}";
            //dataDisplay.Text = fileWindowHandler.V[0].Packet.PacketNR.ToString();

            foreach (var i in fileWindowHandler.N)
            {
                //Debug.WriteLine($"Packet NR: {i}");
                //dataDisplay.Text = $"{dataDisplay.Text + i}\n";
                packetList.Items.Add(i);
            }
            DrawGraph(fileWindowHandler.N, fileWindowHandler.C_An0);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        private void DrawGraph(List<int> X, List<double> Y)
        {
            int PacketIterationTracker = 0;
            int CurrentXAxisPos = 0;
            double tempXAxisPos = 0;
            double CurrentYAxisPos;
            int NextXAxisPos;
            int j = 0;

            //Loops through each AN0 reading inside C_An0 list
            //Creates line beginning and ending at the current X and Y[i] point
            //And ends at X+1 and Y[i+1](Next An0 reading of list)
            foreach (var i in Y)
            {
                //Debug.WriteLine($"Packet NR: {X[PacketIterationTracker]}, X-Pos: {PacketIterationTracker}, Y-Pos: {i}");
                Line line2 = new Line();
                line2.X1 = CurrentXAxisPos;
                CurrentYAxisPos = 400 - Map(0, 3300, 0, 400, i);
                tempXAxisPos = Map(0, 400, 0, 400, i);
                line2.Y1 = CurrentYAxisPos;
                NextXAxisPos = CurrentXAxisPos + (400 / fileWindowHandler.N.Count);
                if (NextXAxisPos == CurrentXAxisPos)
                {
                    NextXAxisPos = NextXAxisPos + 1;
                }
                line2.X2 = NextXAxisPos;
                if ((PacketIterationTracker + 1) < fileWindowHandler.V.Count)
                {
                    line2.Y2 = 400 - Map(0, 3300, 0, 400, fileWindowHandler.V[PacketIterationTracker + 1].Packet.AnalogValue0);
                }
                else
                {
                    line2.Y2 = 400 - Map(0, 3300, 0, 400, i);
                }
                line2.Stroke = new SolidColorBrush(Colors.Black);
                line2.StrokeThickness = 2;
                lineList.Add(line2);

                Ellipse ellipsePlaceHolder = new Ellipse();
                ellipsePlaceHolder.Width = 5;
                ellipsePlaceHolder.Height = 5;
                ellipsePlaceHolder.Fill = new SolidColorBrush(Colors.Red);
                Canvas.SetZIndex(ellipsePlaceHolder, 999);
                ellipseList.Add(ellipsePlaceHolder);
                //Canvas.SetLeft(ellipsePlaceHolder, line2.X2 - (ellipsePlaceHolder.Width / 2));
                //Canvas.SetTop(ellipsePlaceHolder, line2.Y2 - (ellipsePlaceHolder.Height / 2));
                //GraphCanvas.Children.Add(ellipsePlaceHolder);
                PacketIterationTracker++;
                CurrentXAxisPos = NextXAxisPos;
            }

            //Ellipse ellipsePlaceHolder2 = new Ellipse();
            //ellipsePlaceHolder2.Width = 5;
            //ellipsePlaceHolder2.Height = 5;
            //ellipsePlaceHolder2.Fill = new SolidColorBrush(Colors.Red);
            //Canvas.SetZIndex(ellipsePlaceHolder2, 999);
            //ellipseList.Add(ellipsePlaceHolder2);

            //Line linePlaceHolder = new Line();
            //linePlaceHolder.X1 = CurrentXAxisPos + 1;
            //linePlaceHolder.Y1 = CurrentYAxisPos + 1;
            //linePlaceHolder.X2 = CurrentXAxisPos;
            //linePlaceHolder.Y2 = CurrentYAxisPos;
            //linePlaceHolder.Stroke = new SolidColorBrush(Colors.Black);
            //linePlaceHolder.StrokeThickness = 2;
            //lineList.Add(linePlaceHolder);

            //Draws each line object inside Line List
            foreach (var i in lineList)
            {
                GraphCanvas.Children.Add(i);
            }
            //Draws Each packet marker
            //foreach (var i in ellipseList)
            //{
            //    Canvas.SetLeft(i, lineList[j].X1 - (i.Width / 2));
            //    Canvas.SetTop(i, lineList[j].Y1 - (i.Height / 2));
            //    GraphCanvas.Children.Add(i);
            //    j++;
            //}
            //int ye = ellipseList.Count;
            //Canvas.SetLeft(ellipseList[ye - 1], lineList[j - 1].X2 - (ellipseList[ye - 1].Width / 2));
            //Canvas.SetTop(ellipseList[ye - 1], lineList[j - 1].Y2 - (ellipseList[ye - 1].Height / 2));
            //GraphCanvas.Children.Add(ellipseList[ye]);
        }

        /// <summary>
        /// http://rosettacode.org/wiki/Map_range#C
        ///
        /// Maps packet values to desired range
        /// </summary>
        /// <param name="a1">Min Input</param>
        /// <param name="a2">Max Input</param>
        /// <param name="b1">Min Output</param>
        /// <param name="b2">Max Output</param>
        /// <param name="s">Value From Input</param>
        /// <returns>Maped Input As Output</returns>
        private static double Map(double a1, double a2, double b1, double b2, double s) => b1 + (s - a1) * (b2 - b1) / (a2 - a1);

        /// <summary>
        /// Displays selected packet data in textbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void packetSelectorBtn_Click(object sender, RoutedEventArgs e)
        {
            int CurrentIndex;
            int SelectedPacketNr;

            //Sets all packet markers to default color
            foreach (var i in ellipseList)
            {
                //i.Fill = new SolidColorBrush(Colors.Red);
                GraphCanvas.Children.Remove(i);
            }

            SelectedPacketNr = Convert.ToInt32(packetList.Items[packetList.SelectedIndex]);
            //Debug.WriteLine(SelectedPacketNr);
            CurrentIndex = fileWindowHandler.N.IndexOf(SelectedPacketNr);
            OldIndex = CurrentIndex;

            //Displays packet values to debug.
            //Debug.WriteLine("Contains");
            //Debug.WriteLine($"An0: {fileWindowHandler.V[CurrentIndex].Packet.AnalogValue0}");
            //Debug.WriteLine($"An0: {fileWindowHandler.V[CurrentIndex].Packet.AnalogValue1}");
            //Debug.WriteLine($"An0: {fileWindowHandler.V[CurrentIndex].Packet.AnalogValue2}");
            //Debug.WriteLine($"An0: {fileWindowHandler.V[CurrentIndex].Packet.AnalogValue3}");
            //Debug.WriteLine($"An0: {fileWindowHandler.V[CurrentIndex].Packet.AnalogValue4}");
            //Debug.WriteLine($"An0: {fileWindowHandler.V[CurrentIndex].Packet.AnalogValue5}");
            //Debug.WriteLine($"Bat Current: {fileWindowHandler.V[CurrentIndex].Packet.BatteryCurrent}");
            //Debug.WriteLine($"Bat Current: {fileWindowHandler.V[CurrentIndex].Packet.BatteryVoltage}");
            //Debug.WriteLine($"Bat Current: {fileWindowHandler.V[CurrentIndex].Packet.LED1_Current}");
            //Debug.WriteLine($"Bat Current: {fileWindowHandler.V[CurrentIndex].Packet.LED2_Current}");
            //Debug.WriteLine($"Bat Current: {fileWindowHandler.V[CurrentIndex].Packet.LED3_Current}");

            dataDisplay.Text = "";
            dataDisplay.Text =
                $"An0: {fileWindowHandler.V[CurrentIndex].Packet.AnalogValue0} \n" +
                $"An1: {fileWindowHandler.V[CurrentIndex].Packet.AnalogValue1} \n" +
                $"An2: {fileWindowHandler.V[CurrentIndex].Packet.AnalogValue1} \n" +
                $"An3: {fileWindowHandler.V[CurrentIndex].Packet.AnalogValue1} \n" +
                $"An4: {fileWindowHandler.V[CurrentIndex].Packet.AnalogValue1} \n" +
                $"An5: {fileWindowHandler.V[CurrentIndex].Packet.AnalogValue1} \n" +
                $"Bat Current: {fileWindowHandler.V[CurrentIndex].Packet.BatteryCurrent} \n" +
                $"Bat Voltage: {fileWindowHandler.V[CurrentIndex].Packet.BatteryVoltage} \n" +
                $"LED1 Current: {fileWindowHandler.V[CurrentIndex].Packet.LED1_Current} \n" +
                $"LED2 Current: {fileWindowHandler.V[CurrentIndex].Packet.LED2_Current} \n" +
                $"LED3 Current: {fileWindowHandler.V[CurrentIndex].Packet.LED3_Current} \n";

            ellipseList[CurrentIndex].Fill = new SolidColorBrush(Colors.Red);
            Canvas.SetLeft(ellipseList[CurrentIndex], lineList[CurrentIndex].X1 - (ellipseList[CurrentIndex].Width / 2));
            Canvas.SetTop(ellipseList[CurrentIndex], lineList[CurrentIndex].Y1 - (ellipseList[CurrentIndex].Height / 2));
            GraphCanvas.Children.Add(ellipseList[CurrentIndex]);
        }
    }
}