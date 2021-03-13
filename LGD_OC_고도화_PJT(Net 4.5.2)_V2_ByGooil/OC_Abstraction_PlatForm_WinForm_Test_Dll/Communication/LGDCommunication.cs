using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LGD_OC_AstractPlatForm.CommonAPI;
using System.Windows.Forms;

//PNC
using System.IO.MemoryMappedFiles;
using System.Threading;


namespace OC_Abstraction_PlatForm_WinForm_Test_Dll.Communication
{
    class LGDCommunication : ICommunication
    {
        RichTextBox richtextbox;
        //PNC parameters
        bool bIPC_Open = false;
        MemoryMappedViewAccessor m_hMemoryAccessor = null;
        MemoryMappedFile m_hMemoryMapped = null;
        EventWaitHandle evt = null;
        int cnt_Send = 0;

        public LGDCommunication(RichTextBox _richtextbox)
        {
            richtextbox = _richtextbox;
        }

        public void DisplayBoxPattern(byte[] Box_RGB, byte[] Background_RGB, int[] Pos_BoxLeftTop, int[] Pos_BoxRightBottom, int channel_num)
        {
            int X1 = Pos_BoxLeftTop[0];
            int Y1 = Pos_BoxLeftTop[1];

            int X2 = Pos_BoxRightBottom[0];
            int Y2 = Pos_BoxRightBottom[1];

            string box_image = string.Format("image.box2 {0} {1} {2} {3} {4} {5} {6} {7} {8} {9}", Box_RGB[0], Box_RGB[1], Box_RGB[2], Background_RGB[0], Background_RGB[1], Background_RGB[2], X1, Y1, X2, Y2);
            IPC_Quick_Send(box_image);
        }


        public void DisplayMonoPattern(byte[] RGB, int channel_num)
        {
            IPC_Quick_Send(String.Format("image.mono {0} {1} {2}", RGB[0], RGB[1], RGB[2]));
        }

        public byte[] ReadData(byte address, int amount, int offset, int channel_num)
        {
            if (offset != 0) IPC_Quick_Send("mipi.write 0x15 0xB0 0x" + offset.ToString("X2"));
            return OTP_Read(amount, address.ToString("X2")).ToArray();
        }

        private List<byte> OTP_Read(int Quantity, string Register)
        {
            List<byte> listReadData = new List<byte>();
        
            try
            {
                int dex_params_num = Quantity;
                if (dex_params_num > 512)
                {
                    dex_params_num = 512;
                    MessageBox.Show("Cannot read more than 512ea");
                }

                string Hex_Params_num = dex_params_num.ToString("X3");

                string str1 = (dex_params_num > 255)? "mipi.write 0x37 " + "0x" + Hex_Params_num[1] + Hex_Params_num[2] + " 0x0" + Hex_Params_num[0] :  "mipi.write 0x37 0x" + Hex_Params_num[1] + Hex_Params_num[2];
                IPC_Quick_Send(str1);

                string str2 = "mipi.read 0x06 0x" + Register;
                char[] mipi_data = IPC_Quick_Send(str2).ToCharArray();

                int i;
                if (dex_params_num < 10) i = 17;
                else if (dex_params_num < 100) i = 18;
                else i = 19;

                for (; i < mipi_data.Length; i = i + 5)
                {
                    if (listReadData.Count >= Quantity)
                        break;

                    string tmp = new string(mipi_data, i + 2, 2);
                    listReadData.Add(Convert.ToByte(tmp,16));
                    
                }
            }
            catch
            {
                MessageBox.Show("OTP Read fail !\nPlease check the Sample or System-connection status");
            }

            Application.DoEvents();
            Thread.Sleep(50);
            return listReadData;
        }

        public void WriteData(byte address, byte[] parameters, int channel_num)
        {
            StringBuilder mipiCMD = new StringBuilder("mipi.write");
           
            if (parameters == null || parameters.Length == 0)
                mipiCMD.Append(" 0x05 0x").Append(address.ToString("X2"));
            else
            {
                if (parameters.Length == 1)
                    mipiCMD.Append(" 0x15 0x").Append(address.ToString("X2"));
                else
                    mipiCMD.Append(" 0x39 0x").Append(address.ToString("X2"));

                foreach (byte papam in parameters)
                    mipiCMD.Append(" 0x").Append(papam.ToString("X2"));
            }

            IPC_Quick_Send(mipiCMD.ToString());

            if (richtextbox.Lines.Count() > 1000)
                richtextbox.Clear();
        }

        private void IPC_Open()
        {
            try
            {
                m_hMemoryMapped = MemoryMappedFile.OpenExisting("PNC_DIKI_IPC", MemoryMappedFileRights.ReadWrite);
                if (m_hMemoryMapped == null)
                {
                    System.Windows.Forms.MessageBox.Show("Memory Mapping Error");
                    bIPC_Open = false;
                }

                m_hMemoryAccessor = m_hMemoryMapped.CreateViewAccessor();
                if (m_hMemoryAccessor == null)
                {
                    MessageBox.Show("Memory Access Error");
                    bIPC_Open = false;
                }

                evt = new EventWaitHandle(false, EventResetMode.ManualReset, "PNC_DIKI_IPC_READ");
                bIPC_Open = true;
            }
            catch (Exception e)
            {
                MessageBox.Show("IPC Open Error !! : " + e.Message);
                bIPC_Open = false;
            }
        }

        private string IPC_Quick_Send(string Ipc_str)
        {
            if(bIPC_Open == false)
                IPC_Open();

            if(bIPC_Open)
            {
                byte[] WriteByte;
                string Ipc_tx = "s" + cnt_Send + Ipc_str;
                WriteByte = ASCIIEncoding.ASCII.GetBytes(Ipc_tx);
                evt.Set();

                // --------------------------------------------------------------------------
                // 1024 byte dummy send..
                byte[] DummyWrite;
                DummyWrite = new byte[1024];
                m_hMemoryAccessor.WriteArray<byte>(0, DummyWrite, 0, DummyWrite.Length);
                Thread.Sleep(2);

                // --------------------------------------------------------------------------
                // command send
                m_hMemoryAccessor.WriteArray<byte>(0, WriteByte, 0, WriteByte.Length);
                cnt_Send++;
                if (cnt_Send == 10)
                    cnt_Send = 0;

                Thread.Sleep(10);

                byte[] bReadData = new byte[1024];

                //To make sure during iteration these params will not be changed
                const int PNC_ACK_Sleep_ms = 20;
                const int PNC_ACK_Loop_Max_local = 100;

                for (int Ack = 0; Ack < PNC_ACK_Loop_Max_local; Ack++)
                {
                    Thread.Sleep(PNC_ACK_Sleep_ms);
                    m_hMemoryAccessor.ReadArray<byte>(0, bReadData, 0, bReadData.Length);
                    if (bReadData[0] == 'r')
                    {
                        break;
                    }
                    else if (bReadData[0] == 'm')
                    {
                        string tta = Encoding.Default.GetString(bReadData) + "\r\n";
                        richtextbox.AppendText(tta);
                        return tta;
                    }
                }
                evt.Reset();
            }

            return string.Empty;
        }
    }
}
