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
                Debug.WriteLine($"Packet NR: {i}");
                //dataDisplay.Text = $"{dataDisplay.Text + i}\n";
                packetList.Items.Add(i);
            }
            DrawGraph(fileWindowHandler.N, fileWindowHandler.C_An0);
        }

        private void DrawGraph(List<int> X, List<double> Y)
        {
            int o = 0;
            int j = 0;
            int z;
            List<Line> lineList = new List<Line>();

            foreach (var i in Y)
            {
                Debug.WriteLine($"Packet NR: {X[o]}, X-Pos: {o}, Y-Pos: {i}");
                Line line2 = new Line();
                Ellipse ellipsePlaceHolder = new Ellipse();
                ellipsePlaceHolder.Width = 4;
                ellipsePlaceHolder.Height = 4;
                ellipsePlaceHolder.Fill = new SolidColorBrush(Colors.Red);
                Canvas.SetZIndex(ellipsePlaceHolder, 999);
                line2.X1 = j;
                line2.Y1 = 400 - Map(0, 3300, 0, 400, i);
                z = j + (400 / fileWindowHandler.N.Count);
                line2.X2 = z;
                if ((o + 1) < fileWindowHandler.V.Count)
                {
                    line2.Y2 = 400 - Map(0, 3300, 0, 400, fileWindowHandler.V[o + 1].Packet.AnalogValue0);
                }
                else
                {
                    line2.Y2 = 400 - Map(0, 3300, 0, 400, i);
                }
                line2.Stroke = new SolidColorBrush(Colors.Black);
                line2.StrokeThickness = 2;
                lineList.Add(line2);
                Canvas.SetLeft(ellipsePlaceHolder, line2.X2 - (ellipsePlaceHolder.Width / 2));
                Canvas.SetTop(ellipsePlaceHolder, line2.Y2 - (ellipsePlaceHolder.Height / 2));
                GraphCanvas.Children.Add(ellipsePlaceHolder);
                o++;
                j = z;
            }

            foreach (var i in lineList)
            {
                GraphCanvas.Children.Add(i);
            }

            //GraphCanvas.Children.Add(line[2]);
            //GraphCanvas.Children.Add(line[3]);
        }

        //http://rosettacode.org/wiki/Map_range#C

        private static double Map(double a1, double a2, double b1, double b2, double s) => b1 + (s - a1) * (b2 - b1) / (a2 - a1);

        private void packetSelectorBtn_Click(object sender, RoutedEventArgs e)
        {
            //int SelectedPacketNr = Convert.ToInt32(PacketSelectortxtbx.Text);
            int i;
            int SelectedPacketNr;

            SelectedPacketNr = Convert.ToInt32(packetList.Items[packetList.SelectedIndex]);
            Debug.WriteLine(SelectedPacketNr);

            //if (fileWindowHandler.N.Contains(SelectedPacketNr))
            {
                i = fileWindowHandler.N.IndexOf(SelectedPacketNr);
                Debug.WriteLine("Contains");
                Debug.WriteLine($"An0: {fileWindowHandler.V[i].Packet.AnalogValue0}");
                Debug.WriteLine($"An0: {fileWindowHandler.V[i].Packet.AnalogValue1}");
                Debug.WriteLine($"An0: {fileWindowHandler.V[i].Packet.AnalogValue2}");
                Debug.WriteLine($"An0: {fileWindowHandler.V[i].Packet.AnalogValue3}");
                Debug.WriteLine($"An0: {fileWindowHandler.V[i].Packet.AnalogValue4}");
                Debug.WriteLine($"An0: {fileWindowHandler.V[i].Packet.AnalogValue5}");
                Debug.WriteLine($"Bat Current: {fileWindowHandler.V[i].Packet.BatteryCurrent}");
                Debug.WriteLine($"Bat Current: {fileWindowHandler.V[i].Packet.BatteryVoltage}");
                Debug.WriteLine($"Bat Current: {fileWindowHandler.V[i].Packet.LED1_Current}");
                Debug.WriteLine($"Bat Current: {fileWindowHandler.V[i].Packet.LED2_Current}");
                Debug.WriteLine($"Bat Current: {fileWindowHandler.V[i].Packet.LED3_Current}");

                dataDisplay.Text = "";
                dataDisplay.Text = $"An0: {fileWindowHandler.V[i].Packet.AnalogValue0} \n" +
                    $"An1: {fileWindowHandler.V[i].Packet.AnalogValue1} \n" +
                    $"An2: {fileWindowHandler.V[i].Packet.AnalogValue1} \n" +
                    $"An3: {fileWindowHandler.V[i].Packet.AnalogValue1} \n" +
                    $"An4: {fileWindowHandler.V[i].Packet.AnalogValue1} \n" +
                    $"An5: {fileWindowHandler.V[i].Packet.AnalogValue1} \n" +
                    $"Bat Current: {fileWindowHandler.V[i].Packet.BatteryCurrent} \n" +
                    $"Bat Voltage: {fileWindowHandler.V[i].Packet.BatteryVoltage} \n" +
                    $"LED1 Current: {fileWindowHandler.V[i].Packet.LED1_Current} \n" +
                    $"LED2 Current: {fileWindowHandler.V[i].Packet.LED2_Current} \n" +
                    $"LED3 Current: {fileWindowHandler.V[i].Packet.LED3_Current} \n";
            }
            //else
            {
                //Debug.WriteLine("Does not Contain");
                //dataDisplay.Text = "Does Not Contain Selected Packet";
            }
        }
    }
}