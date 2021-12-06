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
            dataDisplay.Text = fileWindowHandler.V[0].Packet.PacketNR.ToString();

            foreach (var i in fileWindowHandler.N)
            {
                Debug.WriteLine(i);
            }
            //System.Diagnostics.Debug.WriteLine(fileWindowHandler.N);
            //Debug.WriteLine("Hello");
        }
    }
}