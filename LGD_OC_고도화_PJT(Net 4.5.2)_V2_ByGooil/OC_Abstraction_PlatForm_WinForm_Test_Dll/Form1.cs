using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LGD_OC_AstractPlatForm.CommonAPI;
using LGD_OC_AstractPlatForm.OpticCompensation;
using LGD_OC_AstractPlatForm.OpticCompensation.DP213;
using LGD_OC_AstractPlatForm.OpticCompensation.DP213.ELVSSCompensation;
using LGD_OC_AstractPlatForm.OpticCompensation.DP213.WhiteCompensation;
using LGD_OC_AstractPlatForm.OpticCompensation.DP213.BlackCompensation;
using LGD_OC_AstractPlatForm.OpticCompensation.DP213.GrayLowReferenceCompensation;
using LGD_OC_AstractPlatForm.OpticCompensation.DP213.AODCompensation;
using LGD_OC_AstractPlatForm.OpticCompensation.DP213.MainCompensation;
using LGD_OC_AstractPlatForm.OpticCompensation.DP213.Data;
using LGD_OC_AstractPlatForm.OpticCompensation.DP213.FlashMemory;
using OC_Abstraction_PlatForm_WinForm_Test_Dll.Measurement;
using OC_Abstraction_PlatForm_WinForm_Test_Dll.DiaLog;
using OC_Abstraction_PlatForm_WinForm_Test_Dll.Factory;
using OC_Abstraction_PlatForm_WinForm_Test_Dll.Communication.Gooil;
using BSQH_Csharp_Library;
using LGD_OC_AstractPlatForm.Enums;
using System.IO;
using System.Threading;


namespace OC_Abstraction_PlatForm_WinForm_Test_Dll
{
    public partial class Form1 : Form
    {

        const int channel_length = 8;
        IOCparamters[] ocparams = new DP213_OCParameters[channel_length];
        bool[] IsCAConnected = new bool[channel_length];
        bool[] IsSampleDisplayed = new bool[channel_length];
        IBusinessAPI[] Channel_API = new IBusinessAPI[channel_length];
        RichTextBox[] richTextBoxes = new RichTextBox[channel_length];
        CompensationFacade[] Channel_OC = new CompensationFacade[channel_length];
        byte[][] ReadData = new byte[channel_length][];
        double[][] MeasuredXYLv = new double[channel_length][];        

        public RichTextBox[] GetRichTextBoxes()
        {
            return richTextBoxes;
        }

        public Form1()
        {
            InitializeComponent();
            for (int ch = 0; ch < channel_length; ch++)
            {
                IsCAConnected[ch] = false;
                IsSampleDisplayed[ch] = false;
                Channel_API[ch] = null;
                ocparams[ch] = null;
                MeasuredXYLv[ch] = new double[3];
            }
            richTextBoxes[0] = richTextBox_ch1;
            richTextBoxes[1] = richTextBox_ch2;
            richTextBoxes[2] = richTextBox_ch3;
            richTextBoxes[3] = richTextBox_ch4;
            richTextBoxes[4] = richTextBox_ch5;
            richTextBoxes[5] = richTextBox_ch6;
            richTextBoxes[6] = richTextBox_ch7;
            richTextBoxes[7] = richTextBox_ch8;

         
        }

        private void button_RichTestBox_Clear_Click(object sender, EventArgs e)
        {
            richTextBox_ch1.Clear();
            richTextBox_ch2.Clear();
            richTextBox_ch3.Clear();
            richTextBox_ch4.Clear();
            richTextBox_ch5.Clear();
            richTextBox_ch6.Clear();
            richTextBox_ch7.Clear();
            richTextBox_ch8.Clear();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            VendorForm.GetInstance().Show();
            this.Enabled = false;
        }

        private void button_Read_Click(object sender, EventArgs e)
        {
            ChannelWinformAPIFactory channelAPIFactory = new ChannelWinformAPIFactory(VendorForm.GetInstance().GetVendor());
            byte Address = Convert.ToByte(Convert.ToInt32(textBox_Read_Address.Text, 16));
            int amount = Convert.ToInt32(textBox_Read_HowMany.Text);
            int offset = Convert.ToInt32(textBox_Read_Offset.Text, 16);

            int height = Convert.ToInt32(textBox_Full_Height.Text);
            int width = Convert.ToInt32(textBox_Full_Width.Text);

            for (int ch = 0; ch < VendorForm.GetInstance().GetChannelLength(); ch++)
            {
                if (VendorForm.GetInstance().GetVendor() == WhichVendor.Gooil) IsSampleDisplayed[ch] = true;
                if (VendorForm.GetInstance().GetVendor() == WhichVendor.LGD) IsSampleDisplayed[ch] = true;

                if (IsSampleDisplayed[ch])
                {
                    if (Channel_API[ch] == null) Channel_API[ch] = channelAPIFactory.GetIBusinessAPI(richTextBoxes[ch], height, width);
                    ReadData[ch] = Channel_API[ch].ReadData(Address, amount, offset,ch);
                }
            }
        }

        private void button_Write_Click(object sender, EventArgs e)
        {
            ChannelWinformAPIFactory channelAPIFactory = new ChannelWinformAPIFactory(VendorForm.GetInstance().GetVendor());
            byte Address = Convert.ToByte(Convert.ToInt32(textBox_Write_Address.Text, 16));

            byte[] parameters = Get_pamaeters();

            int height = Convert.ToInt32(textBox_Full_Height.Text);
            int width = Convert.ToInt32(textBox_Full_Width.Text);

            for (int ch = 0; ch < VendorForm.GetInstance().GetChannelLength(); ch++)
            {
                if (VendorForm.GetInstance().GetVendor() == WhichVendor.Gooil) IsSampleDisplayed[ch] = true;
                if (VendorForm.GetInstance().GetVendor() == WhichVendor.LGD) IsSampleDisplayed[ch] = true;//for test

                if (IsSampleDisplayed[ch])
                {
                    richTextBoxes[ch].AppendText("ch : " + ch);

                    if (Channel_API[ch] == null) Channel_API[ch] = channelAPIFactory.GetIBusinessAPI(richTextBoxes[ch], height, width);
                    Channel_API[ch].WriteData(Address, parameters,ch);
                }
            }
        }


        private byte[] Get_pamaeters()
        {
            byte[] parameters = null;
            if (textBox_Write_MipiData.Text.Trim() != "")
            {
                string[] HexData = textBox_Write_MipiData.Text.Split(' ');
                parameters = new byte[HexData.Length];
                for (int i = 0; i < HexData.Length; i++)
                    parameters[i] = Convert.ToByte(Convert.ToInt32(HexData[i].Substring(2), 16));
            }
            return parameters;
        }


        private void button_Measure_Click(object sender, EventArgs e)
        {
            ChannelWinformAPIFactory channelAPIFactory = new ChannelWinformAPIFactory(VendorForm.GetInstance().GetVendor());
            int height = Convert.ToInt32(textBox_Full_Height.Text);
            int width = Convert.ToInt32(textBox_Full_Width.Text);

            for (int ch = 0; ch < VendorForm.GetInstance().GetChannelLength(); ch++)
            {
                if (VendorForm.GetInstance().GetVendor() == WhichVendor.Gooil) IsCAConnected[ch] = true;
                if (VendorForm.GetInstance().GetVendor() == WhichVendor.LGD) IsCAConnected[ch] = true;

                if (IsCAConnected[ch])
                {
                    if (Channel_API[ch] == null) Channel_API[ch] = channelAPIFactory.GetIBusinessAPI(richTextBoxes[ch], height, width);
                    MeasuredXYLv[ch] = Channel_API[ch].measure_XYL(ch);
                }
            }
        }

        private void button_Compensation_Click(object sender, EventArgs e)
        {
            ChannelWinformAPIFactory channelAPIFactory = new ChannelWinformAPIFactory(VendorForm.GetInstance().GetVendor());
            int height = Convert.ToInt32(textBox_Full_Height.Text);
            int width = Convert.ToInt32(textBox_Full_Width.Text);

            for (int ch = 0; ch < VendorForm.GetInstance().GetChannelLength(); ch++)
            {
                if (VendorForm.GetInstance().GetVendor() == WhichVendor.Gooil
                    || VendorForm.GetInstance().GetVendor() == WhichVendor.LGD)
                {
                    IsCAConnected[ch] = true;
                    IsSampleDisplayed[ch] = true;
                }

                if (IsCAConnected[ch] && IsSampleDisplayed[ch])
                {
                    if (Channel_API[ch] == null) Channel_API[ch] = channelAPIFactory.GetIBusinessAPI(richTextBoxes[ch], height, width);
                    if (ocparams[ch] == null) ocparams[ch] = new DP213_OCParameters(Channel_API[ch], ch, richTextBoxes[ch]);
                    
                    Channel_OC[ch] = new CompensationFacade(ModelName.DP213,Channel_API[ch], ocparams[ch],ch);
                    Channel_OC[ch].OpticCompensation();
                }
            }
        }

        private void button_Display_Pattern_Click(object sender, EventArgs e)
        {
            ChannelWinformAPIFactory channelAPIFactory = new ChannelWinformAPIFactory(VendorForm.GetInstance().GetVendor());
            byte R = Convert.ToByte(textBox_R.Text);
            byte G = Convert.ToByte(textBox_G.Text);
            byte B = Convert.ToByte(textBox_B.Text);
            byte[] RGB = new byte[3] { R, G, B };
            int height = Convert.ToInt32(textBox_Full_Height.Text);
            int width = Convert.ToInt32(textBox_Full_Width.Text);

            for (int ch = 0; ch < VendorForm.GetInstance().GetChannelLength(); ch++)
            {
                if (VendorForm.GetInstance().GetVendor() == WhichVendor.Gooil) IsSampleDisplayed[ch] = true;
                if (VendorForm.GetInstance().GetVendor() == WhichVendor.LGD) IsSampleDisplayed[ch] = true;//for test

                if (IsSampleDisplayed[ch])
                {
                    if (Channel_API[ch] == null) Channel_API[ch] = channelAPIFactory.GetIBusinessAPI(richTextBoxes[ch], height, width);
                    Channel_API[ch].DisplayMonoPattern(RGB,ch);
                }
            }
        }

        private void button_CA_Connect_Click(object sender, EventArgs e)
        {
                if (VendorForm.GetInstance().GetVendor() == WhichVendor.Gooil)
                    GooilMeasurement.CA_Connect();
                else if (VendorForm.GetInstance().GetVendor() == WhichVendor.LGD)
                    LGDMeasurement.CA_Connect();
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void button_display_box_pattern_Click(object sender, EventArgs e)
        {
            ChannelWinformAPIFactory channelAPIFactory = new ChannelWinformAPIFactory(VendorForm.GetInstance().GetVendor());

            byte f_red = Convert.ToByte(textBox_box_fore_R.Text);
            byte f_green = Convert.ToByte(textBox_box_fore_G.Text);
            byte f_blue = Convert.ToByte(textBox_box_fore_B.Text);
            byte[] Box_RGB = new byte[3] { f_red, f_green, f_blue };

            byte b_red = Convert.ToByte(textBox_box_back_R.Text);
            byte b_green = Convert.ToByte(textBox_box_back_G.Text);
            byte b_blue = Convert.ToByte(textBox_box_back_B.Text);
            byte[] BackGround_RGB = new byte[3] { b_red, b_green, b_blue };

            int box_left = Convert.ToInt32(textBox_Pos_BoxLeft.Text);
            int box_top = Convert.ToInt32(textBox_Pos_BoxTop.Text);
            int[] Pos_BoxLeftTop = new int[2] { box_left, box_top };

            int box_right = Convert.ToInt32(textBox_Pos_BoxRight.Text);
            int box_bottom = Convert.ToInt32(textBox_Pos_BoxBottom.Text);
            int[] Pos_BoxRightBottom = new int[2] { box_right, box_bottom };

            int height = Convert.ToInt32(textBox_Full_Height.Text);
            int width = Convert.ToInt32(textBox_Full_Width.Text);

            for (int ch = 0; ch < VendorForm.GetInstance().GetChannelLength(); ch++)
            {
                if (VendorForm.GetInstance().GetVendor() == WhichVendor.Gooil) IsSampleDisplayed[ch] = true;
                if (VendorForm.GetInstance().GetVendor() == WhichVendor.LGD) IsSampleDisplayed[ch] = true;//for test

                if (IsSampleDisplayed[ch])
                {
                    if (Channel_API[ch] == null) Channel_API[ch] = channelAPIFactory.GetIBusinessAPI(richTextBoxes[ch], height, width);
                    Channel_API[ch].DisplayBoxPattern(Box_RGB, BackGround_RGB, Pos_BoxLeftTop, Pos_BoxRightBottom,ch);
                }
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            for (int i = 0; i < ConfigInfo.nPgNum; i++)
            {
                if (ConfigInfo.sockControl[i] != null && ConfigInfo.sockControl[i].IsConnected())
                {
                    ConfigInfo.sockControl[i].endClient();
                }
            }
        }

        private void button_OCParameters_Class_Verify_Click(object sender, EventArgs e)
        {
            //Verify OK
            ChannelWinformAPIFactory channelAPIFactory = new ChannelWinformAPIFactory(VendorForm.GetInstance().GetVendor());
            int height = Convert.ToInt32(textBox_Full_Height.Text);
            int width = Convert.ToInt32(textBox_Full_Width.Text);

            for (int ch = 0; ch < VendorForm.GetInstance().GetChannelLength(); ch++)
            {
                if (Channel_API[ch] == null) Channel_API[ch] = channelAPIFactory.GetIBusinessAPI(richTextBoxes[ch], height, width);
                DP213_OCParameters oc_param = new DP213_OCParameters(Channel_API[ch], ch, richTextBoxes[ch]);

                richTextBoxes[ch].AppendText("\n\n--Mode1--");
                oc_param.ShowOCParamData(OC_Mode.Mode1);
                oc_param.ShowVoltageData(OC_Mode.Mode1);

                richTextBoxes[ch].AppendText("\n\n--Mode2--");
                oc_param.ShowOCParamData(OC_Mode.Mode2);
                oc_param.ShowVoltageData(OC_Mode.Mode2);

                richTextBoxes[ch].AppendText("\n\n--Mode3--");
                oc_param.ShowOCParamData(OC_Mode.Mode3);
                oc_param.ShowVoltageData(OC_Mode.Mode3);

                richTextBoxes[ch].AppendText("\n\n--Mode4--");
                oc_param.ShowOCParamData(OC_Mode.Mode4);
                oc_param.ShowVoltageData(OC_Mode.Mode4);

                richTextBoxes[ch].AppendText("\n\n--Mode5--");
                oc_param.ShowOCParamData(OC_Mode.Mode5);
                oc_param.ShowVoltageData(OC_Mode.Mode5);

                richTextBoxes[ch].AppendText("\n\n--Mode6--");
                oc_param.ShowOCParamData(OC_Mode.Mode6);
                oc_param.ShowVoltageData(OC_Mode.Mode6);
            }
        }

        private void button_Test_DP213CMD_Class_Click(object sender, EventArgs e)
        {
            //Verify OK
            ChannelWinformAPIFactory channelAPIFactory = new ChannelWinformAPIFactory(VendorForm.GetInstance().GetVendor());
            const int ch = 0;
            int height = Convert.ToInt32(textBox_Full_Height.Text);
            int width = Convert.ToInt32(textBox_Full_Width.Text);
            Channel_API[ch] = channelAPIFactory.GetIBusinessAPI(richTextBoxes[ch], height, width);
            DP213CMD cmd = new DP213CMD(Channel_API[ch], ch);

            cmd.SendGammaSetApplyCMD(Gamma_Set.Set1);
            cmd.SendGammaSetApplyCMD(Gamma_Set.Set2);
            cmd.SendGammaSetApplyCMD(Gamma_Set.Set3);
            cmd.SendGammaSetApplyCMD(Gamma_Set.Set4);
            cmd.SendGammaSetApplyCMD(Gamma_Set.Set5);
            cmd.SendGammaSetApplyCMD(Gamma_Set.Set6);

            cmd.DBV_Setting("FFF");
            cmd.DBV_Setting("DDD");
            cmd.DBV_Setting("ABC");

            if (ocparams[0] == null) ocparams[0] = new DP213_OCParameters(Channel_API[0], 0, richTextBoxes[ch]);
            cmd.Measure(ocparams[0], OC_Mode.Mode1, band: 0, gray: 0);
        }

        private void button_Test_ELVSS_Function_Click(object sender, EventArgs e)
        {
            ChannelWinformAPIFactory channelAPIFactory = new ChannelWinformAPIFactory(VendorForm.GetInstance().GetVendor());
            int height = Convert.ToInt32(textBox_Full_Height.Text);
            int width = Convert.ToInt32(textBox_Full_Width.Text);

            for (int ch = 0; ch < VendorForm.GetInstance().GetChannelLength(); ch++)
            {
                if (Channel_API[ch] == null) Channel_API[ch] = channelAPIFactory.GetIBusinessAPI(richTextBoxes[ch], height, width);
                if (ocparams[ch] == null) ocparams[ch] = new DP213_OCParameters(Channel_API[ch], ch, richTextBoxes[ch]);

                IOCFactory factory = new DP213OCFactory(Channel_API[ch], ocparams[ch], ch, new OCVars(Channel_API[ch]));
                ICompensation elvssOC = factory.GetELVSSCompensation();
                elvssOC.Compensation();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ChannelWinformAPIFactory channelAPIFactory = new ChannelWinformAPIFactory(VendorForm.GetInstance().GetVendor());
            int height = Convert.ToInt32(textBox_Full_Height.Text);
            int width = Convert.ToInt32(textBox_Full_Width.Text);

            for (int ch = 0; ch < VendorForm.GetInstance().GetChannelLength(); ch++)
            {
                if (Channel_API[ch] == null) Channel_API[ch] = channelAPIFactory.GetIBusinessAPI(richTextBoxes[ch], height, width);
                if (ocparams[ch] == null) ocparams[ch] = new DP213_OCParameters(Channel_API[ch], ch, richTextBoxes[ch]);
                ICompensation whiteOC = new DP213_WhiteCompensation(Channel_API[ch], ocparams[ch], ch, new OCVars(Channel_API[ch]));
                whiteOC.Compensation();
            }
        }

        private void button_Black_OC_Test_Click(object sender, EventArgs e)
        {
            ChannelWinformAPIFactory channelAPIFactory = new ChannelWinformAPIFactory(VendorForm.GetInstance().GetVendor());
            int height = Convert.ToInt32(textBox_Full_Height.Text);
            int width = Convert.ToInt32(textBox_Full_Width.Text);

            for (int ch = 0; ch < VendorForm.GetInstance().GetChannelLength(); ch++)
            {
                if (Channel_API[ch] == null) Channel_API[ch] = channelAPIFactory.GetIBusinessAPI(richTextBoxes[ch], height, width);
                if (ocparams[ch] == null) ocparams[ch] = new DP213_OCParameters(Channel_API[ch], ch, richTextBoxes[ch]);
                ICompensation blackOC = new DP213_BlackCompensation(Channel_API[ch], ocparams[ch], ch, new OCVars(Channel_API[ch]));
                blackOC.Compensation();
            }
        }

        private void button_GrayLowRef_OC_Test_Click(object sender, EventArgs e)
        {
            ChannelWinformAPIFactory channelAPIFactory = new ChannelWinformAPIFactory(VendorForm.GetInstance().GetVendor());
            int height = Convert.ToInt32(textBox_Full_Height.Text);
            int width = Convert.ToInt32(textBox_Full_Width.Text);

            for (int ch = 0; ch < VendorForm.GetInstance().GetChannelLength(); ch++)
            {
                if (Channel_API[ch] == null) Channel_API[ch] = channelAPIFactory.GetIBusinessAPI(richTextBoxes[ch], height, width);
                if (ocparams[ch] == null) ocparams[ch] = new DP213_OCParameters(Channel_API[ch], ch, richTextBoxes[ch]);
                ICompensation graylowrefOC = new DP213_GrayLowRefCompensation(Channel_API[ch], ocparams[ch], ch, new OCVars(Channel_API[ch]));
                graylowrefOC.Compensation();
            }
        }

        private void button_Test_AOD_Click(object sender, EventArgs e)
        {
            ChannelWinformAPIFactory channelAPIFactory = new ChannelWinformAPIFactory(VendorForm.GetInstance().GetVendor());
            int height = Convert.ToInt32(textBox_Full_Height.Text);
            int width = Convert.ToInt32(textBox_Full_Width.Text);

            for (int ch = 0; ch < VendorForm.GetInstance().GetChannelLength(); ch++)
            {
                if (Channel_API[ch] == null) Channel_API[ch] = channelAPIFactory.GetIBusinessAPI(richTextBoxes[ch], height, width);
                if (ocparams[ch] == null) ocparams[ch] = new DP213_OCParameters(Channel_API[ch], ch, richTextBoxes[ch]);
                ICompensation graylowrefOC = new DP213_AODCompensation(Channel_API[ch], ocparams[ch], ch, new OCVars(Channel_API[ch]));
                graylowrefOC.Compensation();
            }
        }

        private void button_Main123_Test_Click(object sender, EventArgs e)
        {
            ChannelWinformAPIFactory channelAPIFactory = new ChannelWinformAPIFactory(VendorForm.GetInstance().GetVendor());
            int height = Convert.ToInt32(textBox_Full_Height.Text);
            int width = Convert.ToInt32(textBox_Full_Width.Text);

            for (int ch = 0; ch < VendorForm.GetInstance().GetChannelLength(); ch++)
            {
                if (Channel_API[ch] == null) Channel_API[ch] = channelAPIFactory.GetIBusinessAPI(richTextBoxes[ch], height, width);
                if (ocparams[ch] == null) ocparams[ch] = new DP213_OCParameters(Channel_API[ch], ch, richTextBoxes[ch]);
                ICompensation main123 = new DP213_Mode123_Main_Compensation(Channel_API[ch], ocparams[ch], ch, new OCVars(Channel_API[ch]));
                main123.Compensation();
            }
        }

        private void button_Main456_Test_Click(object sender, EventArgs e)
        {
            ChannelWinformAPIFactory channelAPIFactory = new ChannelWinformAPIFactory(VendorForm.GetInstance().GetVendor());
            int height = Convert.ToInt32(textBox_Full_Height.Text);
            int width = Convert.ToInt32(textBox_Full_Width.Text);

            for (int ch = 0; ch < VendorForm.GetInstance().GetChannelLength(); ch++)
            {
                if (Channel_API[ch] == null) Channel_API[ch] = channelAPIFactory.GetIBusinessAPI(richTextBoxes[ch], height, width);
                if (ocparams[ch] == null) ocparams[ch] = new DP213_OCParameters(Channel_API[ch], ch, richTextBoxes[ch]);
                
                double[] xyl = Channel_API[ch].measure_XYL(ch);
                if (xyl[2] > 50)
                {
                    ICompensation main456 = new DP213_Mode456_Main_Compensation(Channel_API[ch], ocparams[ch], ch, new OCVars(Channel_API[ch]));
                    main456.Compensation();
                }
            }
        }

        private void button_Flash_Frame_CRC_Check_Click(object sender, EventArgs e)
        {
            ChannelWinformAPIFactory channelAPIFactory = new ChannelWinformAPIFactory(VendorForm.GetInstance().GetVendor());
            int height = Convert.ToInt32(textBox_Full_Height.Text);
            int width = Convert.ToInt32(textBox_Full_Width.Text);

            for (int ch = 0; ch < VendorForm.GetInstance().GetChannelLength(); ch++)
            {
                if (Channel_API[ch] == null) Channel_API[ch] = channelAPIFactory.GetIBusinessAPI(richTextBoxes[ch], height, width);
                if (ocparams[ch] == null) ocparams[ch] = new DP213_OCParameters(Channel_API[ch], ch, richTextBoxes[ch]);

                DP213Flash dP213Flash = new DP213Flash(Channel_API[ch], ocparams[ch], ch, new OCVars(Channel_API[ch]));
                dP213Flash.Read_From_Frame_and_Show();

            }
        }

        private void button_Flash_Flash_CRC_Check_Click(object sender, EventArgs e)
        {
            ChannelWinformAPIFactory channelAPIFactory = new ChannelWinformAPIFactory(VendorForm.GetInstance().GetVendor());
            int height = Convert.ToInt32(textBox_Full_Height.Text);
            int width = Convert.ToInt32(textBox_Full_Width.Text);

            for (int ch = 0; ch < VendorForm.GetInstance().GetChannelLength(); ch++)
            {
                if (Channel_API[ch] == null) Channel_API[ch] = channelAPIFactory.GetIBusinessAPI(richTextBoxes[ch], height, width);
                if (ocparams[ch] == null) ocparams[ch] = new DP213_OCParameters(Channel_API[ch], ch, richTextBoxes[ch]);

                DP213Flash dP213Flash = new DP213Flash(Channel_API[ch], ocparams[ch], ch, new OCVars(Channel_API[ch]));
                dP213Flash.Read_From_Flash_and_Show();
            }
        }

        private void button_Flash_Erase_and_Write_Click(object sender, EventArgs e)
        {
            ChannelWinformAPIFactory channelAPIFactory = new ChannelWinformAPIFactory(VendorForm.GetInstance().GetVendor());
            int height = Convert.ToInt32(textBox_Full_Height.Text);
            int width = Convert.ToInt32(textBox_Full_Width.Text);

            for (int ch = 0; ch < VendorForm.GetInstance().GetChannelLength(); ch++)
            {

                if (Channel_API[ch] == null) Channel_API[ch] = channelAPIFactory.GetIBusinessAPI(richTextBoxes[ch], height, width);
                if (ocparams[ch] == null) ocparams[ch] = new DP213_OCParameters(Channel_API[ch], ch, richTextBoxes[ch]);

                IFlashMemory dP213Flash = new DP213Flash(Channel_API[ch], ocparams[ch], ch, new OCVars(Channel_API[ch]));
                dP213Flash.FlashEraseAndWrite();

            }
        }

        private void button_ThreadWithout_Test_Click(object sender, EventArgs e)
        {
            for (int ch = 0; ch < VendorForm.GetInstance().GetChannelLength(); ch++)
            {
                Test test = new Test(richTextBoxes[ch]);
                test.TestThread();
            }
        }

     
        private void button_ThreadWith_Test_Click(object sender, EventArgs e)
        {
            Thread[] th = new Thread[VendorForm.GetInstance().GetChannelLength()];
            Test[] test = new Test[VendorForm.GetInstance().GetChannelLength()]; 

            for (int ch = 0; ch < VendorForm.GetInstance().GetChannelLength(); ch++)
            {
                test[ch] = new Test(richTextBoxes[ch]);
                th[ch] = new Thread(test[ch].TestThread);
                th[ch].Start();
            }
        }

 
    }
}
