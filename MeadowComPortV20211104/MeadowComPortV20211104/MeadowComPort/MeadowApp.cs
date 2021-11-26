using Meadow;
using Meadow.Devices;
using Meadow.Foundation;
using Meadow.Foundation.Leds;
using Meadow.Foundation.Graphics;
using Meadow.Hardware;
using Meadow.Units;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Meadow.Foundation.Displays.TftSpi;

namespace MeadowComPort
{
    public class MeadowApp : App<F7Micro, MeadowApp>
    {
        private RgbPwmLed onboardLed;
        private ISerialPort serialPort;
        private string milliVolts00, milliVolts01, milliVolts02, milliVolts03, milliVolts04, milliVolts05;

        private IAnalogInputPort analogIn00;
        private IAnalogInputPort analogIn01;
        private IAnalogInputPort analogIn02;
        private IAnalogInputPort analogIn03;
        private IAnalogInputPort analogIn04;
        private IAnalogInputPort analogIn05;

        //private IDigitalInputPort inputPortD01;
        private IDigitalInputPort inputPortD02;

        private IDigitalInputPort inputPortD03;
        private IDigitalInputPort inputPortD04;
        private IDigitalOutputPort outputPortD05;
        private IDigitalOutputPort outputPortD06;
        private IDigitalOutputPort outputPortD07;
        private IDigitalOutputPort outputPortD08;

        private int packetNumber;
        private int milliVolts;
        private int analogPin;
        private int digitalPin;
        private int packetLen;
        private int analogReq = 6;// set to the Max number of analog pins required
        private int digitalInReq = 8;// set to the Max number of digital input pins required
        private int checkSum = 0;
        private int index = 0;
        private int TXstate = 0;
        private int RXstate = 0;
        private int RXindex = 0;
        private String outPacket = "###P##AN00AN01AN02AN03AN04AN05bbbbCHKrn";
        private String inPacket;
        private int packetTime = 100;

        //LCD VARs

        private St7735 st7735;
        private GraphicsLibrary graphics;

        private int displayWidth, displayHeight;

        public MeadowApp()
        {
            var config = new SpiClockConfiguration(6000, SpiClockConfiguration.Mode.Mode3);

            //Setup of LCD board used
            st7735 = new St7735
            (
                device: Device,
                spiBus: Device.CreateSpiBus(Device.Pins.SCK, Device.Pins.MOSI, Device.Pins.MISO, config),
                chipSelectPin: Device.Pins.D02,
                dcPin: Device.Pins.D01,
                resetPin: Device.Pins.D00,
                width: 128, height: 160,
                displayType: St7735.DisplayType.ST7735R_BlackTab
            );

            displayWidth = Convert.ToInt32(st7735.Width);
            displayHeight = Convert.ToInt32(st7735.Height);

            graphics = new GraphicsLibrary(st7735);
            graphics.Rotation = GraphicsLibrary.RotationType._90Degrees;

            graphics.CurrentFont = new Font12x20();
            graphics.Clear(true);
            graphics.DrawText(10, 10, "HELLO", Color.White);
            graphics.Show();

            //DrawShapes();
            Initialize();
            SendLoop();
        }

        //private void DrawShapes()
        //{
        //    Random rand = new Random();

        //    graphics.Clear(true);

        //    int radius = 10;
        //    int originX = displayWidth / 2;
        //    int originY = displayHeight / 2;

        //    //Draws 4 circles originating from the center of the lcd
        //    //Each iteration of circle has a larger diamiter.
        //    for (int i = 1; i < 5; i++)
        //    {
        //        graphics.DrawCircle
        //        (
        //            centerX: originX,
        //            centerY: originY,
        //            radius: radius,
        //            color: Color.FromRgb(rand.Next(128, 255), rand.Next(128, 255), rand.Next(128, 255))
        //        );
        //        graphics.Show();
        //        radius += 20;
        //    }

        //    int sideLength = 30;

        //    //Draws 4 different rectangles originating from the center of the lcd
        //    //Each iteration rectangle has a larger side.
        //    for (int i = 1; i < 5; i++)
        //    {
        //        graphics.DrawRectangle
        //        (
        //            x: (displayWidth - sideLength) / 2,
        //            y: (displayHeight - sideLength) / 2,
        //            width: sideLength,
        //            height: sideLength,
        //            color: Color.FromRgb(rand.Next(128, 255), rand.Next(128, 255), rand.Next(128, 255))
        //        );
        //        graphics.Show();
        //        sideLength += 40;
        //    }
        //    graphics.DrawLine(0, displayHeight / 2, displayWidth, displayHeight / 2,
        //        Color.FromRgb(rand.Next(128, 255), rand.Next(128, 255), rand.Next(128, 255)));
        //    graphics.DrawLine(displayWidth / 2, 0, displayWidth / 2, displayHeight,
        //        Color.FromRgb(rand.Next(128, 255), rand.Next(128, 255), rand.Next(128, 255)));
        //    graphics.DrawLine(0, 0, displayWidth, displayHeight,
        //        Color.FromRgb(rand.Next(128, 255), rand.Next(128, 255), rand.Next(128, 255)));
        //    graphics.DrawLine(0, displayHeight, displayWidth, 0,
        //        Color.FromRgb(rand.Next(128, 255), rand.Next(128, 255), rand.Next(128, 255)));
        //    graphics.Show();

        //    //Thread.Sleep(5000);
        //}

        private void Initialize()
        {
            Console.WriteLine("Initialize hardware...");

            onboardLed = new RgbPwmLed(device: Device,
                redPwmPin: Device.Pins.OnboardLedRed,
                greenPwmPin: Device.Pins.OnboardLedGreen,
                bluePwmPin: Device.Pins.OnboardLedBlue,
                3.3f, 3.3f, 3.3f,
                Meadow.Peripherals.Leds.IRgbLed.CommonType.CommonAnode);
            inPacket = "";
            analogIn00 = Device.CreateAnalogInputPort(Device.Pins.A00);
            analogIn01 = Device.CreateAnalogInputPort(Device.Pins.A01);
            analogIn02 = Device.CreateAnalogInputPort(Device.Pins.A02);
            analogIn03 = Device.CreateAnalogInputPort(Device.Pins.A03);
            analogIn04 = Device.CreateAnalogInputPort(Device.Pins.A04);
            analogIn05 = Device.CreateAnalogInputPort(Device.Pins.A05);
            //inputPortD01 = Device.CreateDigitalInputPort(Device.Pins.D01);
            inputPortD02 = Device.CreateDigitalInputPort(Device.Pins.D02);
            inputPortD03 = Device.CreateDigitalInputPort(Device.Pins.D03);
            inputPortD04 = Device.CreateDigitalInputPort(Device.Pins.D04);
            outputPortD05 = Device.CreateDigitalOutputPort(Device.Pins.D05, false);
            outputPortD06 = Device.CreateDigitalOutputPort(Device.Pins.D06, false);
            outputPortD07 = Device.CreateDigitalOutputPort(Device.Pins.D07, false);
            outputPortD08 = Device.CreateDigitalOutputPort(Device.Pins.D08, false);
            Thread.Sleep(200);
            outputPortD05.State = true;
            Thread.Sleep(200);
            outputPortD07.State = true;
            Thread.Sleep(200);
            outputPortD06.State = true;
            Thread.Sleep(200);
            outputPortD08.State = true;
            serialPort = Device.CreateSerialPort(Device.SerialPortNames.Com1, 115200);
            serialPort.Open();
            serialPort.DataReceived += SerialPort_DataReceived;
            analogIn00.Updated += AnalogIn00_Updated;
            analogIn01.Updated += AnalogIn01_Updated;
            analogIn02.Updated += AnalogIn02_Updated;
            analogIn03.Updated += AnalogIn03_Updated;
            analogIn04.Updated += AnalogIn04_Updated;
            analogIn05.Updated += AnalogIn05_Updated;
            int timeSpanInMilliSec = 100;
            analogIn00.StartUpdating(TimeSpan.FromMilliseconds(timeSpanInMilliSec));
            analogIn01.StartUpdating(TimeSpan.FromMilliseconds(timeSpanInMilliSec));
            analogIn02.StartUpdating(TimeSpan.FromMilliseconds(timeSpanInMilliSec));
            analogIn03.StartUpdating(TimeSpan.FromMilliseconds(timeSpanInMilliSec));
            analogIn04.StartUpdating(TimeSpan.FromMilliseconds(timeSpanInMilliSec));
            analogIn05.StartUpdating(TimeSpan.FromMilliseconds(timeSpanInMilliSec));
            analogIn05.StartUpdating(TimeSpan.FromMilliseconds(timeSpanInMilliSec));
        }

        private void AnalogIn00_Updated(object sender, IChangeResult<Voltage> e)
        {
            //Console.WriteLine(e.New.Millivolts);
            int milliVolts = Convert.ToInt32(e.New.Millivolts);
            milliVolts00 = milliVolts.ToString("D4");
            //Console.WriteLine(milliVolts00);
        }

        private void AnalogIn01_Updated(object sender, IChangeResult<Voltage> e)
        {
            //Console.WriteLine(e.New.Millivolts);
            int miliVolts = Convert.ToInt32(e.New.Millivolts);
            milliVolts01 = miliVolts.ToString("D4");
        }

        private void AnalogIn02_Updated(object sender, IChangeResult<Voltage> e)
        {
            //Console.WriteLine(e.New.Millivolts);
            int miliVolts = Convert.ToInt32(e.New.Millivolts);
            milliVolts02 = miliVolts.ToString("D4");
        }

        private void AnalogIn03_Updated(object sender, IChangeResult<Voltage> e)
        {
            //Console.WriteLine(e.New.Millivolts);
            int miliVolts = Convert.ToInt32(e.New.Millivolts);
            milliVolts03 = miliVolts.ToString("D4");
        }

        private void AnalogIn04_Updated(object sender, IChangeResult<Voltage> e)
        {
            //Console.WriteLine(e.New.Millivolts);
            int miliVolts = Convert.ToInt32(e.New.Millivolts);
            milliVolts04 = miliVolts.ToString("D4");
        }

        private void AnalogIn05_Updated(object sender, IChangeResult<Voltage> e)
        {
            int miliVolts = Convert.ToInt32(e.New.Millivolts);
            milliVolts05 = miliVolts.ToString("D4");
        }

        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            int calChkSum = 0;
            int recChkSum = 0;

            //Console.WriteLine(serialPort.BytesToRead);
            byte[] response = new byte[12];
            this.serialPort.Read(response, 0, 12);
            //serialPort.Write(response);
            if (response[0] == 0x23 &&
               response[1] == 0x23 &&
               response[2] == 0x23)
            {
                for (int i = 3; i < 7; i++)
                {
                    calChkSum += response[i];
                }
                //Console.WriteLine(calChkSum);
                recChkSum = (response[7] - 0x30) * 100 + (response[8] - 0x30) * 10 + (response[9] - 0x30);
                //Console.WriteLine(recChkSum);
                if (calChkSum == recChkSum)
                {
                    //Original code
                    //outputPortD06.State = response[3] == 0x31 ? true : false;
                    //outputPortD07.State = response[4] == 0x31 ? true : false;
                    //outputPortD08.State = response[5] == 0x31 ? true : false;
                    //outputPortD09.State = response[6] == 0x31 ? true : false;
                    //Console.WriteLine(outputPortD09.State);

                    //My Modifications
                    outputPortD05.State = response[3] == 0x31 ? true : false;
                    outputPortD06.State = response[4] == 0x31 ? true : false;
                    outputPortD07.State = response[5] == 0x31 ? true : false;
                    outputPortD08.State = response[6] == 0x31 ? true : false;
                }
            }
        }

        private void SendLoop()
        {
            Console.WriteLine("Sending Data");

            while (true)
            {
                switch (TXstate)
                {
                    case 0: // begin making output packet
                        //Console.WriteLine(outPacket);

                        outPacket = "###"; //header
                        checkSum = 0;
                        index = 0;
                        analogPin = 0;
                        //digitalPin=0;
                        digitalPin = digitalInReq - 1;//start with Most significant pin
                        outPacket += packetNumber++.ToString("D3"); //inc packet number add to outPacket string
                        packetNumber %= 1000;   //packetnumber rollover code
                        TXstate = 1;  //move to next state
                        break;

                    case 1: // continue making output packet and do analog at the same time

                        outPacket += milliVolts00 + milliVolts01 + milliVolts02 + milliVolts03 + milliVolts04 + milliVolts05;

                        TXstate = 2;// move to next state when all analog complete
                        break;

                    case 2:
                        bool[] currentState = new bool[4];
                        //currentState[0] = inputPortD01.State;
                        currentState[1] = inputPortD02.State;
                        currentState[2] = inputPortD03.State;
                        currentState[3] = inputPortD04.State;

                        foreach (bool state in currentState)
                        {
                            string outString = " ";
                            if (state)
                            {
                                outString = "1";
                            }
                            else
                            {
                                outString = "0";
                            }
                            outPacket += outString;
                        }
                        //Console.WriteLine(outPacket);
                        //Thread.Sleep(1000);
                        TXstate = 3;//move to next state when all input complete
                        break;

                    case 3:
                        for (int i = 3; i < outPacket.Length; i++)
                        {
                            checkSum += (byte)outPacket[i];//calculate check sum
                        }
                        checkSum %= 1000; //trucate check sum to 3 digits
                        outPacket += checkSum.ToString("D3");
                        outPacket += "\r\n";// add carriage return, line feed
                        packetLen = outPacket.Length;//set packet length to send
                        //Console.WriteLine(outPacket);
                        //Thread.Sleep(1000);
                        TXstate = 4; //move to next state
                        break;

                    case 4: // stay in case 4 until entire packet is sent
                        //if (index == packetLen)// when entire packet is sent check interval
                        {
                            var buffer = new byte[35];
                            buffer = Encoding.UTF8.GetBytes(outPacket);
                            serialPort.Write(buffer);
                            Thread.Sleep(packetTime);

                            TXstate = 0; //reset the state when the whole packet is sent
                        }
                        break;
                }
            }
        }
    }
}