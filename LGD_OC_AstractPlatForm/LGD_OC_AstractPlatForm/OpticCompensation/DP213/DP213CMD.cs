using System;
using LGD_OC_AstractPlatForm.Enums;
using LGD_OC_AstractPlatForm.CommonAPI;
using LGD_OC_AstractPlatForm.OpticCompensation.DP213.AODCompensation;
using LGD_OC_AstractPlatForm.OpticCompensation.DP213.BlackCompensation;
using LGD_OC_AstractPlatForm.OpticCompensation.DP213.ELVSSCompensation;
using LGD_OC_AstractPlatForm.OpticCompensation.DP213.GrayLowReferenceCompensation;
using LGD_OC_AstractPlatForm.OpticCompensation.DP213.MainCompensation;
using LGD_OC_AstractPlatForm.OpticCompensation.DP213.WhiteCompensation;
using BSQH_Csharp_Library;
using LGD_OC_AstractPlatForm.OpticCompensation.DP213.Data;
using System.Drawing;

namespace LGD_OC_AstractPlatForm.OpticCompensation.DP213
{
    public class DP213CMD
    {
        IBusinessAPI api;
        int channel_num;
        public DP213CMD(IBusinessAPI _api, int _channel_num)
        {
            api = _api;
            channel_num = _channel_num;
        }

        public void AODOn()
        {
            //mipi.write 0x05 0x39  
            byte address = Convert.ToByte("39", 16);
            api.WriteData(address, parameters: null, channel_num);
            api.WriteLine("--AOD On--",Color.Blue);
        }

        public void AODOff()
        {
            //mipi.write 0x05 0x38  
            byte address = Convert.ToByte("38", 16);
            api.WriteData(address, parameters: null, channel_num);
            api.WriteLine("--AOD Off--", Color.Blue);
        }

        public void SendGammaSetApplyCMD(Gamma_Set Set)
        {
            byte address = Convert.ToByte("76", 16);
            byte[] parameters = null;

            if (Set == Gamma_Set.Set1) { parameters = new byte[] { 0 }; api.WriteLine("Gamma1 is applied", GetGammaSetColor(Set)); }
            else if (Set == Gamma_Set.Set2){ parameters = new byte[] { 1 }; api.WriteLine("Gamma2 is applied", GetGammaSetColor(Set)); }
            else if (Set == Gamma_Set.Set3){ parameters = new byte[] { 2 }; api.WriteLine("Gamma3 is applied", GetGammaSetColor(Set)); }
            else if (Set == Gamma_Set.Set4){ parameters = new byte[] { 3 }; api.WriteLine("Gamma4 is applied", GetGammaSetColor(Set)); }
            else if (Set == Gamma_Set.Set5){ parameters = new byte[] { 4 }; api.WriteLine("Gamma5 is applied", GetGammaSetColor(Set)); }
            else if (Set == Gamma_Set.Set6){ parameters = new byte[] { 5 }; api.WriteLine("Gamma6 is applied", GetGammaSetColor(Set)); }
            api.WriteData(address, parameters, channel_num);
        }

        public Color GetGammaSetColor(Gamma_Set Set)
        {
            if (Set == Gamma_Set.Set1) return Color.Red;
            if (Set == Gamma_Set.Set2) return Color.Green;
            if (Set == Gamma_Set.Set3) return Color.Blue;
            if (Set == Gamma_Set.Set4) return Color.DarkRed;
            if (Set == Gamma_Set.Set5) return Color.DarkGreen;
            if (Set == Gamma_Set.Set6) return Color.DarkBlue;
            return Color.Black;
        }

        public bool IsBand0orAOD0(int band)
        {
            return (band == 0 || band == 12);
        }

        public void DBV_Setting(string DBV)
        {
            //("mipi.write 0x39 0x51 0x0" + DBV[0] + " 0x" + DBV[1] + DBV[2]);
            byte Address = Convert.ToByte("51", 16);
            byte[] parameters = new byte[] { Convert.ToByte(DBV[0].ToString(), 16), Convert.ToByte(DBV[1].ToString() + DBV[2].ToString(), 16) };
            api.WriteData(Address, parameters, channel_num);
            api.WriteLine($"DBV[{DBV}] was sent");
        }

        public void SendMipiCMD(byte[][] Output_CMD)
        {
            byte offset = Convert.ToByte(Output_CMD[0][0]);
            if (offset > 0)
            {
                byte address1 = Convert.ToByte("B0", 16);
                byte[] parameters1 = new byte[] { offset };
                api.WriteData(address1, parameters1, channel_num);
            }

            byte address2 = Output_CMD[1][0];
            byte[] parameters2 = Output_CMD[2];
            api.WriteData(address2, parameters2, channel_num);
        }

        public void SendOneParamMipiCMD(byte offset, string HexAddress, string HexParam)
        {
            if (offset > 0)
            {
                byte address1 = Convert.ToByte("B0", 16);
                byte[] parameters1 = new byte[] { offset };
                api.WriteData(address1, parameters1, channel_num);
            }

            byte address2 = Convert.ToByte(HexAddress, 16);
            byte[] parameters2 = new byte[] { Convert.ToByte(HexParam, 16) }; 
            api.WriteData(address2, parameters2, channel_num);
        }


        public void Measure(IOCparamters ocparam, OC_Mode mode, int band, int gray)
        {
            double[] XYLv = api.measure_XYL(channel_num);
            api.WriteLine($"X / Y / Lv : {XYLv[0]} / {XYLv[1]} / {XYLv[2]}");
            ocparam.Set_OC_Mode_Measure(new XYLv(XYLv[0], XYLv[1], XYLv[2]), mode, band, gray);
        }
    }
}
