
using BSQH_Csharp_Library;
using LGD_OC_AstractPlatForm.CommonAPI;
using LGD_OC_AstractPlatForm.OpticCompensation.DP213.Data;
using LGD_OC_AstractPlatForm.OpticCompensation.DP213.SingleBandCompensation;
using System;
using System.Drawing;
using System.Threading;

namespace LGD_OC_AstractPlatForm.OpticCompensation.DP213.FlashMemory
{
    public class DP213Flash : IFlashMemory
    {
        IBusinessAPI api;
        OCVars vars;
        DP213CMD cmd;
        int channel_num;
        public DP213Flash(IBusinessAPI _api, IOCparamters _ocparam, int _channel_num, OCVars _vars)
        {
            api = _api;
            channel_num = _channel_num;
            vars = _vars;
            cmd = new DP213CMD(api, _channel_num);
        }

        public void FlashEraseAndWrite()
        {
            if (vars.Optic_Compensation_Stop == false)
            {
                api.WriteLine("DP213 FlashEraseAndWrite() Start", Color.Blue);
                SubFlashEraseAndWrite();
                api.WriteLine("DP213 FlashEraseAndWrite() Finish", Color.Green);
            }
            else
            {
                api.WriteLine("DP213 FlashEraseAndWrite() Skip", Color.Red);
            }
        }

        private void SubFlashEraseAndWrite()
        {
            Enable_Flash_Erase_Write();
            
            //CMOTP
            cmd.SendOneParamMipiCMD(8, "E8", "1F");//Flash Erase&Write
            Thread.Sleep(200);
            Flash_Fail_Verify_Check("CMOTP");

            //GMOTP
            cmd.SendOneParamMipiCMD(9, "E8", "1F");//Flash Erase&Write
            Thread.Sleep(200);
            Flash_Fail_Verify_Check("GammaOTP");

            //LGOTP
            cmd.SendOneParamMipiCMD(10, "E8", "1F");//Flash Erase&Write
            Thread.Sleep(200);
            Flash_Fail_Verify_Check("LGOTP");
        }

        private void Enable_Flash_Erase_Write()
        {
            cmd.SendOneParamMipiCMD(5, "E8", "AA");//Unlock CMOTP protection (Flash Writng Enabled)
            cmd.SendOneParamMipiCMD(7, "E8", "01");//Set Flash Write Clock as 1ms 
        }

        private void Flash_Fail_Verify_Check(string Show_Memory_Block)
        {
            byte[] readdata = api.ReadData(address : Convert.ToByte("DD", 16),amount : 1,offset : 2, channel_num);
            if (readdata[0] == Convert.ToByte("01",16))
                api.WriteLine(Show_Memory_Block + " Writing is not finished yet(Flash Busy). it needs to be set more writing time", Color.Red);
            else if (readdata[0] == Convert.ToByte("80",16))
                api.WriteLine(Show_Memory_Block + " OC Writing Failed.", Color.Red);
            else if (readdata[0] == Convert.ToByte("00",16) || readdata[0] == Convert.ToByte("02",16))
                api.WriteLine(Show_Memory_Block + " Flash OC Writing OK", Color.Green);
            else
                api.WriteLine("Unknown Status, DD(P3) = " + readdata[0], Color.Blue);
        }

        
        public void Read_From_Frame_and_Show()
        {
            Unlock_CMOTP_GammaOTP_LGOTP_IDOTP_Protection();
            Preload_For_CMOTP_GammaOTP_LGOTP_IDOTP();
            Thread.Sleep(30);
            Read_and_Show_CMOTP_GammaOTP_LGOTP_IDOTP_CRC();
        }

        public void Read_From_Flash_and_Show()
        {
            Unlock_Read_After_Sleepout_Done_Protection();
            Read_For_CMOTP_GammaOTP_LGOTP_IDOTP();
            Thread.Sleep(30);
            Read_and_Show_CMOTP_GammaOTP_LGOTP_IDOTP_CRC();
        }

        private void Unlock_CMOTP_GammaOTP_LGOTP_IDOTP_Protection()
        {
            cmd.SendOneParamMipiCMD(5, "E8", "AA");//Unlock CM/Gamma/LG/IDOTP protection (Flash Writng Enabled)
        }

        private void Preload_For_CMOTP_GammaOTP_LGOTP_IDOTP()
        {
            for (byte i = 8; i <= 11; i++)
            {
                cmd.SendOneParamMipiCMD(offset: i, "E8", "00");
                cmd.SendOneParamMipiCMD(offset: i, "E8", "80");
            }
        }

        private void Unlock_Read_After_Sleepout_Done_Protection()
        {
            cmd.SendOneParamMipiCMD(5, "E8", "11");
        }

        private void Read_For_CMOTP_GammaOTP_LGOTP_IDOTP()
        {
            cmd.SendOneParamMipiCMD(4, "E8", "00");
            cmd.SendOneParamMipiCMD(4, "E8", "11");
            cmd.SendOneParamMipiCMD(4, "E8", "00");
            cmd.SendOneParamMipiCMD(4, "E8", "12");
            cmd.SendOneParamMipiCMD(4, "E8", "00");
            cmd.SendOneParamMipiCMD(4, "E8", "13");
            cmd.SendOneParamMipiCMD(4, "E8", "00");
            cmd.SendOneParamMipiCMD(4, "E8", "14");
        }

        private void Read_and_Show_CMOTP_GammaOTP_LGOTP_IDOTP_CRC()
        {
            byte[] readdata = api.ReadData(address: Convert.ToByte("DD", 16), amount: 8, offset: 7, channel_num);
            api.WriteLine("CMOTP CRC : " + readdata[0] + "," + readdata[1], Color.Black);
            api.WriteLine("GammaOTP CRC : " + readdata[2] + "," + readdata[3], Color.Black);
            api.WriteLine("LGOTP CRC : " + readdata[4] + "," + readdata[5], Color.Black);
            api.WriteLine("IDOTP CRC : " + readdata[6] + "," + readdata[7], Color.Black);
        }
    }
}
