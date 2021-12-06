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
        private SolidColorBrush solidColorBrush = new SolidColorBrush();
        private DrawingAttributes inkDrawingAttributes;
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
                dataDisplay.Text = $"{dataDisplay.Text + i}\n";
            }

            DrawGraph(fileWindowHandler.N, fileWindowHandler.C_An0, solidColorBrush);
            //System.Diagnostics.Debug.WriteLine(fileWindowHandler.N);
            //Debug.WriteLine("Hello");
        }

        private void DrawGraph(List<int> X, List<double> Y, SolidColorBrush colorBrush)
        {
            int o = 0;
            foreach (var i in Y)
            {
                Debug.WriteLine($"X-Pos: {X[o]}, Y-Pos: {i}");
                o++;
            }
        }
    }
}