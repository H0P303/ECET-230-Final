using MeadowSolar;
using System;
using System.ComponentModel;
using System.IO.Ports;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace usbMeadow
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    ///

    ///TODO:
    ///Make UI txt received box get larger when history is checked.
    public partial class MainWindow : Window
    {
        private bool bPortOpen = false;
        private string text;
        private SerialPort serialPort = new SerialPort();
        private int newPacketNumber = 0, checkSumError = 0, oldPacketNumber = -1, packetRollover = 0, lostPacketCount = 0, CheckSum;
        private bool isSaveLocationSelected = false;
        private bool showDebug = false;

        private StringBuilder stringBuilderSend = new StringBuilder("###1111196");
        private FileSave fileSave = new FileSave();
        private SolarCalc solarCalc = new SolarCalc();
        //private MainWindow mainWindow = new MainWindow();

        //private MainWindow mainWindow = new MainWindow();

        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        /// <summary>
        /// Sets init paramaters for serial port
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            serialPort.BaudRate = 115200;
            serialPort.ReceivedBytesThreshold = 1;
            serialPort.DataReceived += SerialPort_DataReceived;
            setSerialPort();
        }

        /// <summary>
        /// Adds comports to combobox
        /// </summary>
        private void setSerialPort()
        {
            string[] ports = SerialPort.GetPortNames();
            comboBox1.ItemsSource = ports;
            comboBox1.SelectedIndex = 1;
        }

        /// <summary>
        /// Allows thread access to the serial thread and main thread.
        /// If it isnt allowed we catch the exception
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                text = serialPort.ReadLine();

                //Uses the dispatcher .checkaccesss method to determin if the
                //calling thread has access to the thread the UI object is on
                if (txtReceived.Dispatcher.CheckAccess())
                {
                    //This thread has acceess so it can be update the UI thread.
                    UpdateUI(text);
                }
                else
                {
                    //this thread does not have access to the UI thread.
                    //Place the update method on the displatcher of the UI thread.
                    txtReceived.Dispatcher.Invoke(() => { UpdateUI(text); });
                }
            }
            catch (TimeoutException) { }
        }

        /// <summary>
        /// Updates UI with parsed serial data.
        ///
        /// </summary>
        /// <param name="newPacket"></param>    //Gets newPacket from serial data received method
        private void UpdateUI(string newPacket)
        {
            if (historyCheck.IsChecked == true)
            {
                txtReceived.Text = newPacket + txtReceived.Text;    //displays the concatinated received packet on new line
            }
            else
            {
                txtReceived.Text = newPacket;   //displays only one packet
            }

            txtPacketLength.Text = newPacket.Length.ToString();
            int calChkSum = 0;
            if (newPacket.Length > 37)  //if packet length is complete
            {
                if (newPacket.Substring(0, 3) == "###") //checks if beginning of packet is formatted correctly
                {
                    //Parses each part of the packet into substrings, then displays each part
                    //in the appropriate txtbox
                    txtPacketNum.Text = newPacket.Substring(3, 3);
                    newPacketNumber = Convert.ToInt32(txtPacketNum.Text);
                    txtAN0.Text = newPacket.Substring(6, 4);
                    txtAN1.Text = newPacket.Substring(10, 4);
                    txtAN2.Text = newPacket.Substring(14, 4);
                    txtAN3.Text = newPacket.Substring(18, 4);
                    txtAN4.Text = newPacket.Substring(22, 4);
                    txtAN5.Text = newPacket.Substring(26, 4);
                    txtBinary.Text = newPacket.Substring(30, 4);
                    txtRxChkSum.Text = newPacket.Substring(34, 3);

                    for (int i = 3; i < 34; i++)    //Calculates the checksum of the received packet
                    {
                        calChkSum += (byte)newPacket[i];
                    }
                    calChkSum %= 1000;
                    txtCalChkSum.Text = calChkSum.ToString();
                    int recCheckSum = Convert.ToInt32(newPacket.Substring(34, 3));
                    if (recCheckSum == calChkSum)   //if checksum is correct and matching
                    {
                        DisplaySolarData(newPacket);    //use packet data for solar
                        fileSave.parser(newPacket);
                    }
                    else
                    {   //if not matching ++ the checksum error count
                        checkSumError++;
                        txtCheckSumError.Text = checkSumError.ToString();
                    }

                    if (oldPacketNumber > -1)   //If this is not the first packet sent.
                    {
                        if (newPacketNumber == 0) // and if newpacket number is 0
                        {
                            packetRollover++;   //++ the rollover count
                            txtPacketRollover.Text = packetRollover.ToString();
                            if (oldPacketNumber != 999) //if oldpacket number != 999 it means that we lost count of a packet along the way.
                            {
                                lostPacketCount++;  //++ lostpacketcount
                                txtPacketLost.Text = lostPacketCount.ToString();
                            }
                        }
                        else
                        {
                            if (newPacketNumber != oldPacketNumber + 1) //if newpacket != 0, we lost a packet
                            {
                                lostPacketCount++;
                                txtPacketLost.Text = lostPacketCount.ToString();
                            }
                        }
                    }
                    oldPacketNumber = newPacketNumber;
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            sendPacket();
        }

        /// <summary>
        /// calculates checksum of packet we want to send.
        /// changes the stringbuilder string using the sending bytes
        /// </summary>
        private void sendPacket()
        {
            try
            {
                string message = txtPacketSend.Text;

                for (int i = 3; i < 7; i++)
                {
                    CheckSum += (byte)stringBuilderSend[i];
                }

                CheckSum %= 1000;
                stringBuilderSend.Remove(7, 3);
                stringBuilderSend.Insert(7, CheckSum.ToString("D3"));
                txtPacketSend.Text = stringBuilderSend.ToString();
                string messageOut = stringBuilderSend.ToString();
                messageOut += "\r\n";   //Adds return linefeed to string
                byte[] messageBytes = Encoding.UTF8.GetBytes(messageOut);   //Converts message string to byte array.
                serialPort.Write(messageBytes, 0, messageBytes.Length); //Send bytePacket to serialPort
                CheckSum = 0;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void btnBit0_Click(object sender, RoutedEventArgs e)
        {
            ButtonClicked(0);
        }

        private void btnBit1_Click(object sender, RoutedEventArgs e)
        {
            ButtonClicked(1);
        }

        private void btnBit2_Click(object sender, RoutedEventArgs e)
        {
            ButtonClicked(2);
        }

        private void btnBit3_Click(object sender, RoutedEventArgs e)
        {
            ButtonClicked(3);
        }

        private void btnSaveFile_Click(object sender, RoutedEventArgs e)
        {
            isSaveLocationSelected = true;
            fileSave.saveToJson();
        }

        private void OpenFileBtn_Click(object sender, RoutedEventArgs e)
        {
            jsonFileWindow FileWindow = new jsonFileWindow();
            FileWindow.Show();
        }

        /// <summary>
        /// Only Lets User Close Window If Com Port Has Been Closed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (bPortOpen == true)
            {
                e.Cancel = true;
                MessageBox.Show("Close COM Port Before Closing Window", "COM Port Open", MessageBoxButton.OK);
            }
            else
            {
                e.Cancel = false;
            }
        }

        private void showDebugChk_Click(object sender, RoutedEventArgs e)
        {
            if (!showDebug)
            {
                showDebugChk.Content = "Show Debug";
                windowMain.Width = 230;
                showDebug = true;
            }
            else
            {
                windowMain.Width = 800;
                showDebugChk.Content = "Hide Debug";
                showDebug = false;
            }
        }

        /// <summary>
        /// checks which button was clicked and alters the stringbuilder[i] for the specific button clicked.
        /// </summary>
        /// <param name="i"></param>
        private void ButtonClicked(int i)
        {
            Button[] btnBit = new Button[] { btnBit0, btnBit1, btnBit2, btnBit3 };
            if (btnBit[i].Content.ToString() == "0")
            {
                btnBit[i].Content = "1";
                stringBuilderSend[i + 3] = '1';
            }
            else
            {
                btnBit[i].Content = "0";
                stringBuilderSend[i + 3] = '0';
            }
            sendPacket();
        }

        private void DisplaySolarData(string newPacket)
        {
            solarCalc.ParseSolarData(newPacket);
            txtSolarVoltage.Text = solarCalc.GetVoltage(solarCalc.analogVoltage[3]);
            txtBatteryVoltage.Text = solarCalc.GetVoltage(solarCalc.analogVoltage[4]);
            txtBatteryCurrent.Text = solarCalc.GetCurrent(solarCalc.analogVoltage[5], solarCalc.analogVoltage[4]);
            txtLED1Current.Text = solarCalc.LEDCurrent(solarCalc.analogVoltage[5], solarCalc.analogVoltage[2]); //LED1 AN2
            txtLED2Current.Text = solarCalc.LEDCurrent(solarCalc.analogVoltage[5], solarCalc.analogVoltage[1]); //LED2 AN1
            txtLED3Current.Text = solarCalc.LEDCurrent(solarCalc.analogVoltage[5], solarCalc.analogVoltage[0]); //LED3 AN0
        }

        private void btnOpenClose_Click(object sender, RoutedEventArgs e)
        {
            if (isSaveLocationSelected == true)
            {
                if (!bPortOpen)
                {
                    serialPort.PortName = comboBox1.Text;
                    serialPort.Open();
                    btnOpenClose.Content = "Close";
                    bPortOpen = true;
                }
                else
                {
                    serialPort.Close();
                    btnOpenClose.Content = "Open";
                    bPortOpen = false;
                    isSaveLocationSelected = false;
                    fileSave.finish();
                }
            }
            else
            {
                MessageBox.Show("Select location to save data to", "Select save location", MessageBoxButton.OK);
                serialPort.Close();
                btnOpenClose.Content = "Open";
                bPortOpen = false;
            }
        }

        /// <summary>
        /// clears each txtbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            txtReceived.Text = "";
            txtAN0.Text = "";
            txtAN1.Text = "";
            txtAN2.Text = "";
            txtAN3.Text = "";
            txtAN4.Text = "";
            txtAN5.Text = "";
            txtBinary.Text = "";
            txtCalChkSum.Text = "";
            txtCheckSumError.Text = "";
            txtPacketLength.Text = "";
            txtPacketNum.Text = "";
            txtReceived.Text = "";
            txtRxChkSum.Text = "";
        }

        /// <summary>
        /// updates combobox with com ports
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBox1_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            setSerialPort();
        }
    }
}