   M S B A M L   `   `   `  ����   MeadowSolar    usbMeadow.MainWindow   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     @d2http://schemas.microsoft.com/expression/blend/2008     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     -     � Title$  
MainWindow��$	��530��$	��800��.����  �� Orientation$ 
Horizontal=�.+��� .+��� .+��� $	��Top=�$ 
Horizontal=�$��50��$
��Auto��.+��� -    ��Name$ btnOpenClose��$��0,0,10,0q�$��25��$��50��$
��Left=�$
��Open���� -   $ btnClear��$��Clear��$��25��$��50��$
��Left=�$��0,0,10,0q��� -    ��Name$ 	comboBox1��$��25��$��75��$��Center=��� $
��auto��$ 
Horizontal=�.+��� $��Solar Voltage��$��90���� -    ��Name$ txtSolarVoltage��$��80��$��25��$	��top=�${�False���� $
��auto��$ 
Horizontal=�.+��� $��Battery Voltage��$��90���� -   $ txtBatteryVoltage��$��80��$��25��$	��top=�${�False���� $
��auto��$ 
Horizontal=�.+��� $��Battery Current��$��90���� -   $ txtBatteryCurrent��$��80��$��25��$	��top=�${�False���� $
��auto��$ 
Horizontal=�.+��� $��LED 1 Current��$��90���� -   $ txtLED1Current��$��80��$��25��$	��top=�${�False���� $
��auto��$ 
Horizontal=�.+��� $��LED 2 Current��$��90���� -	   $ txtLED2Current��$��80��$��25��$	��top=�${�False���� $
��auto��$ 
Horizontal=�.+��� $��LED 3 Current��$��90���� -
   $ txtLED3Current��$��80��$��25��$	��top=�${�False���� $��0,10,0,0q�$ 
Horizontal=�$��Center=�.+��� $
��left=�$ 
Horizontal=�.+��� -   $ btnSaveFile��$��File Save Location��$��25��$	��110�� � ToolTip$4 .Selects file location for current session file��$��Stretch=�$��Stretch=��� .+��� -   $ OpenFileBtn��$��	Open File��$��25��$��60��$��10,0,0,0q��� $ 
Horizontal=�$	��605��.+��� $��	20,0,10,0q�.+��� $ 
Horizontal=�.+��� $��65��$��0,10,0,0q�$��Packet���� $ 
Horizontal=�.+��� $��65��$��Number���� -   $ txtPacketNum��$��60��$��Center=�${�False���� $ 
Horizontal=�$��0,10,0,0q�.+��� $��65��$��Length���� -   $ txtPacketLength��$��60��$��Center=�${�False���� $ 
Horizontal=�$��0,10,0,0q�.+��� $��65��$��Analog 0���� -   $ txtAN0��$��60��$��Center=�${�False���� $ 
Horizontal=�$��0,10,0,0q�.+��� $��65��$��Analog 1���� -   $ txtAN1��$��60��$��Center=�${�False���� $ 
Horizontal=�$��0,10,0,0q�.+��� $��65��$��Analog 2���� -   $ txtAN2��$��60��$��Center=�${�False���� $ 
Horizontal=�$��0,10,0,0q�.+��� $��65��$��Analog 3���� -   $ txtAN3��$��60��$��Center=�${�False���� $ 
Horizontal=�$��0,10,0,0q�.+��� $��65��$��Analog 4���� -   $ txtAN4��$��60��$��Center=�${�False���� $ 
Horizontal=�$��0,10,0,0q�.+��� $��65��$��Analog 5���� -   $ txtAN5��$��60��$��Center=�${�False���� $ 
Horizontal=�$��0,10,0,0q�.+��� $��65��$��Binary���� -   $ 	txtBinary��$��60��$��Center=�${�False���� $ 
Horizontal=�$��0,10,0,0q�.+��� $��65��$��Rollover���� -   $ txtPacketRollover��$��60��$��Center=�${�False���� $	��Top=�$	��502��$	��388��$ Vertical=�$��0,12,0,0q�.+��� $ 
Horizontal=�.+��� .+��� $	��192��$	��280��$ Vertical=�$
��Left=�.+��� -   $ txtReceived��${�False��" �� VerticalScrollBarVisibility$
 Auto=� �� TextWrapping$
 Wrap=�$
��auto��$	��180��$	��280���� .+��� $ Vertical=�$��70��$��right=�$��10,-5.5,0,0q�.+��� -    ��Name$ historyCheck��$
��Left=�$��Center=�$��64��$��25��$��history���� $ 
Horizontal=�.+��� $ Vertical=�.+��� $ 
Horizontal=�$��0,10,0,0q�.+��� $��65��$��CheckSum���� $ 
Horizontal=�$��0,10,0,0q�.+��� $��70��$��Recieved���� -   $ txtRxChkSum��$��60��$��Center=�${�False���� $ 
Horizontal=�$��0,10,0,0q�.+��� $��70��$��
Calculated���� -   $ txtCalChkSum��$��60��$��Center=�${�False���� $ 
Horizontal=�$��0,10,0,0q�.+��� $��70��$�� CheckError���� -   $ txtCheckSumError��$��60��$��Center=�${�False���� $ 
Horizontal=�$��0,10,0,0q�.+��� $��70��$��Packet Loss���� -   $ txtPacketLost��$��60��$��Center=�${�False���� $ Vertical=�.+��� $ 
Horizontal=�$��0,10,0,0q�.+��� $��75��$��	Send Data���� -   $��60��$
��Send��$��25���� $ 
Horizontal=�$��0,10,0,0q�.+��� $��75��$��Packet Send���� -   $ txtPacketSend��$	��100��$��Center=�${�False���� $ 
Horizontal=�$��0,10,0,0q�.+��� $��75��$��LED 0���� -   $ btnBit0��$��60��$��1��$��18(��� $ 
Horizontal=�$��0,10,0,0q�.+��� $��75��$��LED 1���� -    $ btnBit1��$��60��$��1��$��18(��� $ 
Horizontal=�$��0,10,0,0q�.+��� $��75��$��LED 2���� -!   $ btnBit2��$��60��$��1��$��18(��� $ 
Horizontal=�$��0,10,0,0q�.+��� $��75��$��LED 3���� -"   $ btnBit3��$��60��$��1��$��18(�