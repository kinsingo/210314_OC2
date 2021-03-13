using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LGD_OC_AstractPlatForm.CommonAPI;
using System.Windows.Forms;
using OC_Abstraction_PlatForm_WinForm_Test_Dll.Communication.Gooil;
using System.Globalization;
using System.Threading;

namespace OC_Abstraction_PlatForm_WinForm_Test_Dll.Communication
{
    class GooilCommunication : ICommunication
    {
        RichTextBox richtextbox;
        int Height;
        int Width;
        public GooilCommunication(RichTextBox _richtextbox,int _Height,int _Width)
        {
            richtextbox = _richtextbox;
            Height = _Height;
            Width = _Width;
        }

        public void DisplayBoxPattern(byte[] Box_RGB, byte[] Background_RGB, int[] Pos_BoxLeftTop, int[] Pos_BoxRightBottom, int channel_num)
        {
            if (!ConfigInfo.sockControl[channel_num].IsConnected())
                return;

            StringBuilder sbStringInfo = new StringBuilder(256);

            string fullHight = Height.ToString(); //"2700";
            string fullWidth = Width.ToString("X3");// "4CC";//(1228 -> hex 값);
            string boxPattern = string.Format("TA022000{0:D4}8{1}FFFFNOTA048100{2:D4}8{3:X3}FFFFNO1019{4:X3}FFFFNO1028{1}FFFFNOTA022200{5}8{1}FFFFNOCFG{6:X2}FF{7:X2}FF{8:X2}FFCBG{9:X2}FF{10:X2}FF{11:X2}FF", Pos_BoxLeftTop[1], fullWidth, Pos_BoxRightBottom[1], Pos_BoxLeftTop[0], Pos_BoxRightBottom[0], fullHight, Box_RGB[0], Box_RGB[1], Box_RGB[2], Background_RGB[0], Background_RGB[1], Background_RGB[2]);

            sbStringInfo.AppendFormat("{0:X2}{1:X2}{2:X2}", 0xFF, boxPattern.Length, boxPattern);
            sbStringInfo.Insert(0, sbStringInfo.Length.ToString("X4"));
            sbStringInfo.Insert(0, "A1A333");

            ConfigInfo.SendPacket(ConfigInfo.sockControl[channel_num], false, sbStringInfo.ToString(), 3000);
            Thread.Sleep(100);
            Write_MIPI_Set(new byte[] { 0x3D, 0x86, 0x00 }, 3, channel_num);
            Thread.Sleep(100);
        }

        public void DisplayMonoPattern(byte[] RGB, int channel_num)
        {
            if (!ConfigInfo.sockControl[channel_num].IsConnected())
                return;
            StringBuilder sbStringInfo = new StringBuilder(256);
            sbStringInfo.AppendFormat("{0:X2}{1:X2}{2:X2}{3:X2}{4:X2}", 5, 0, RGB[0], RGB[1], RGB[2]);
            
            sbStringInfo.Insert(0, sbStringInfo.Length.ToString("X4"));
            sbStringInfo.Insert(0, "A1A333");
            ConfigInfo.SendPacket(ConfigInfo.sockControl[channel_num], false, sbStringInfo.ToString(), 3000);
            Thread.Sleep(100);
            Write_MIPI_Set(new byte[] { 0x3D, 0x86, 0x00 }, 3, channel_num);
            Thread.Sleep(100);
        }
        public byte[] ReadData(byte address, int amount, int offset, int channel_num)
        {
            if (!ConfigInfo.sockControl[channel_num].IsConnected())
                return new byte[] { 0x00 };
            Write_MIPI_Set(new byte[] { 0x40, 0x51, 0x00 }, 3, channel_num);

            StringBuilder strData = new StringBuilder();
            if (offset != 0)
                Write_MIPI_Register(0x39, new byte[] { 0xB0, (byte)offset }, 2, channel_num);
            Read_MIPI_Regester(address, amount, channel_num, 0x30);
            ConfigInfo.sockControl[channel_num].barrier_wait_for_recv.SignalAndWait(3000);
            byte[] read_Register = getNetworkReadVal(channel_num) as byte[];

            byte[] readData = new byte[amount];
            strData.AppendFormat("Read Data [0x{0:X2}Reg] :", address);
            for (int i = 0; i < amount; i++)
            {
                readData[i] = byte.Parse(Encoding.Default.GetString(read_Register, 17 + (i * 2), 2), NumberStyles.HexNumber);
                strData.AppendFormat("{0:X2} ", readData[i]);
            }
            Write_MIPI_Set(new byte[] { 0x40, 0x51, 0x00 }, 3, channel_num);

            richtextbox.AppendText(strData.ToString() + "\n");
            return readData;
        }

        public void WriteData(byte address, byte[] parameters, int channel_num)
        {
            if (!ConfigInfo.sockControl[channel_num].IsConnected())
                return;
            
            Write_MIPI_Set(new byte[] { 0x40, 0x51, 0x00 }, 3, channel_num);
            StringBuilder strData = new StringBuilder();

            if (parameters == null)
            {
                Write_MIPI_Register(0x05, new byte[] { address }, 1, channel_num);

                /*
                strData.Append("Write Data :");
                strData.AppendFormat("0x{0:X2} ", address);
                richtextbox.AppendText(strData.ToString() + "\n");
                */
            }
            else
            {
                byte[] data = new byte[parameters.Length + 1];
                data[0] = address;
                Array.Copy(parameters, 0, data, 1, parameters.Length);

                if (parameters.Length == 1)
                    Write_MIPI_Register(0x15, data, data.Length, channel_num);
                else
                    Write_MIPI_Register(0x39, data, data.Length, channel_num);

                /*
                strData.Append("Write Data :");
                for (int i = 0; i < data.Length; i++) strData.AppendFormat("0x{0:X2} ", data[i]);
                richtextbox.AppendText(strData.ToString() + "\n");
                */
            }


            if (richtextbox.Lines.Count() > 1000)
                richtextbox.Clear();
        }

        public static void GI_Set()
        {
            ConnectSocket();
            //barrier_init();

        }
        private static void ConnectSocket()
        {
            int[] nCount = { 0, 0 };
            for (int i = 0; i < ConfigInfo.nPgNum; i++)
            {
                ConfigInfo.sockControl[i] = new SocketControl();
                if (!ConfigInfo.sockControl[i].IsConnected())
                {
                    if (ConfigInfo.sockControl[i].startClient(i, string.Format("192.168.0.{0}", 230 + i), 50230 + i))
                    {
                        ConfigInfo.sockControl[i].barrier_wait_for_recv = new Barrier(2, (b) => { });
                        if (i < 4)
                        {
                            nCount[0]++;
                        }
                        else
                        {
                            nCount[1]++;
                        }
                        //sock_ReConnect_Barrier_In(i);

                        MessageBox.Show(string.Format("PG Connect CH{0}", i + 1));
                    }
                }
            }
        }

        private void Write_MIPI_Set(byte[] data, int length, int ch, byte type = 0)
        {
            StringBuilder sbStrInfo = new StringBuilder(80);

            for (int i = 1; i < length; i++)
                sbStrInfo.Append(data[i].ToString("X4"));

            sbStrInfo.Insert(0, sbStrInfo.Length.ToString("d4"));
            sbStrInfo.Insert(0, string.Format("5321{0:X4}", data[0]));
            sbStrInfo.Insert(0, sbStrInfo.Length.ToString("X4"));
            sbStrInfo.Insert(0, "A1A330");
            ConfigInfo.SendPacket(ConfigInfo.sockControl[ch], false, sbStrInfo.ToString(), 3000);
            //Thread.Sleep(5);
        }

        private void Write_MIPI_Register(byte type, byte[] data, int length, int ch)
        {
            StringBuilder sbStrInfo = new StringBuilder(80);

            switch (type)
            {
                case 0x15:
                    {
                        sbStrInfo.Append(length.ToString("X4"));
                        sbStrInfo.Append("0000");
                        sbStrInfo.Insert(0, sbStrInfo.Length.ToString("d4"));
                        sbStrInfo.Insert(0, "53210050");

                        sbStrInfo.Append("53210053");
                        sbStrInfo.Append((length * 4).ToString("d4"));
                        for (int i = 0; i < length; i++)
                            sbStrInfo.Append(data[i].ToString("X4"));
                    }
                    break;

                case 0x39:
                    {
                        sbStrInfo.Append(length.ToString("X4"));
                        sbStrInfo.Append("0000");
                        sbStrInfo.Insert(0, sbStrInfo.Length.ToString("d4"));
                        sbStrInfo.Insert(0, "53210050");

                        sbStrInfo.Append("53210053");
                        sbStrInfo.Append((length * 4).ToString("d4"));
                        for (int i = 0; i < length; i++)
                            sbStrInfo.Append(data[i].ToString("X4"));
                    }
                    break;

                case 0x05:
                    {
                        sbStrInfo.Append(length.ToString("X4"));
                        sbStrInfo.Append("0000");
                        sbStrInfo.Insert(0, sbStrInfo.Length.ToString("d4"));
                        sbStrInfo.Insert(0, "53210050");

                        sbStrInfo.Append("53210053");
                        sbStrInfo.Append((length * 4).ToString("d4"));
                        for (int i = 0; i < length; i++)
                            sbStrInfo.Append(data[i].ToString("X4"));
                    }
                    break;
            }

            sbStrInfo.Insert(0, sbStrInfo.Length.ToString("X4"));
            sbStrInfo.Insert(0, "A1A330");

            ConfigInfo.SendPacket(ConfigInfo.sockControl[ch], false, sbStrInfo.ToString(), Definition.N_PACKET_MIN_WAIT_COUNT);
        }

        public void Read_MIPI_Regester(byte reg, int length, int ch, byte type = 0)
        {
            StringBuilder sbStringInfo = new StringBuilder(80);
            sbStringInfo.AppendFormat("{0:X2}{1:X2}{2:X2}", type, reg, length);
            sbStringInfo.Insert(0, sbStringInfo.Length.ToString("X4"));
            sbStringInfo.Insert(0, "A1A333");
            ConfigInfo.SendPacket(ConfigInfo.sockControl[ch], false, sbStringInfo.ToString(), Definition.N_PACKET_WAIT_COUNT);
        }

        public object getNetworkReadVal(int ch)
        {
            return ConfigInfo.sockControl[ch].getCheckPass().getNetworkReadVal();
        }
    }
}
