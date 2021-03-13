using System;
using System.IO;
using System.Data;
using System.Linq;
using BSQH_Csharp_Library;
using System.Windows.Forms;
using System.Text;
using LGD_OC_AstractPlatForm.CommonAPI;

namespace LGD_OC_AstractPlatForm.OpticCompensation.DP253.Data
{
    public class DP253_OCParameters : IOCparamters
    {
        public int GetDBV(int band)
        {
            throw new NotImplementedException();
        }

        public byte Get_Normal_REF0()
        {
            throw new NotImplementedException();
        }

        public double Get_Normal_REF0_Voltage()
        {
            throw new NotImplementedException();
        }

        public byte Get_Normal_REF4095()
        {
            throw new NotImplementedException();
        }

        public double Get_Normal_REF4095_Voltage()
        {
            throw new NotImplementedException();
        }

        public RGB Get_OC_Mode_AM0(OC_Mode mode, int band)
        {
            throw new NotImplementedException();
        }

        public RGB_Double Get_OC_Mode_AM0_Voltage(OC_Mode mode, int band)
        {
            throw new NotImplementedException();
        }

        public RGB Get_OC_Mode_AM1(OC_Mode mode, int band)
        {
            throw new NotImplementedException();
        }

        public RGB_Double Get_OC_Mode_AM1_Voltage(OC_Mode mode, int band)
        {
            throw new NotImplementedException();
        }

        public double Get_OC_Mode_Cold_ELVSS_Voltage(OC_Mode mode, int band)
        {
            throw new NotImplementedException();
        }

        public double[] Get_OC_Mode_Cold_ELVSS_Voltages(OC_Mode mode)
        {
            throw new NotImplementedException();
        }

        public double Get_OC_Mode_Cold_Vinit2_Voltage(OC_Mode mode, int band)
        {
            throw new NotImplementedException();
        }

        public double[] Get_OC_Mode_Cold_Vinit2_Voltages(OC_Mode mode)
        {
            throw new NotImplementedException();
        }

        public double Get_OC_Mode_ELVSS_Voltage(OC_Mode mode, int band)
        {
            throw new NotImplementedException();
        }

        public double[] Get_OC_Mode_ELVSS_Voltages(OC_Mode mode)
        {
            throw new NotImplementedException();
        }

        public XYLv Get_OC_Mode_ExtensionXY(OC_Mode mode, int band, int gray)
        {
            throw new NotImplementedException();
        }

        public int Get_OC_Mode_Gray(OC_Mode mode, int bandindex, int grayindex)
        {
            throw new NotImplementedException();
        }

        public bool Get_OC_Mode_IsExtensionApplied(OC_Mode mode, int band, int gray)
        {
            throw new NotImplementedException();
        }

        public XYLv Get_OC_Mode_Limit(OC_Mode mode, int band, int gray)
        {
            throw new NotImplementedException();
        }

        public int Get_OC_Mode_LoopCount(OC_Mode mode, int band, int gray)
        {
            throw new NotImplementedException();
        }

        public XYLv Get_OC_Mode_Measure(OC_Mode mode, int band, int gray)
        {
            throw new NotImplementedException();
        }

        public RGB Get_OC_Mode_RGB(OC_Mode mode, int band, int gray)
        {
            throw new NotImplementedException();
        }

        public RGB[] Get_OC_Mode_RGB(OC_Mode mode, int band)
        {
            throw new NotImplementedException();
        }

        public RGB_Double Get_OC_Mode_RGB_Voltage(OC_Mode mode, int band, int gray)
        {
            throw new NotImplementedException();
        }

        public XYLv Get_OC_Mode_Target(OC_Mode mode, int band, int gray)
        {
            throw new NotImplementedException();
        }

        public double Get_OC_Mode_Vinit2_Voltage(OC_Mode mode, int band)
        {
            throw new NotImplementedException();
        }

        public double[] Get_OC_Mode_Vinit2_Voltages(OC_Mode mode)
        {
            throw new NotImplementedException();
        }

        public int[] Get_OC_Mode_Vreg1(OC_Mode mode)
        {
            throw new NotImplementedException();
        }

        public int Get_OC_Mode_Vreg1(OC_Mode mode, int band)
        {
            throw new NotImplementedException();
        }

        public double Get_OC_Mode_Vreg1_Voltage(OC_Mode mode, int band)
        {
            throw new NotImplementedException();
        }

        public void Set_Normal_REF0(byte _Normal_REF0)
        {
            throw new NotImplementedException();
        }

        public void Set_Normal_REF4095(byte _Normal_REF4095)
        {
            throw new NotImplementedException();
        }

        public void Set_OC_Mode_AM0(RGB AM0, OC_Mode mode, int band)
        {
            throw new NotImplementedException();
        }

        public void Set_OC_Mode_AM0(RGB_Double AM0_Voltage, OC_Mode mode, int band)
        {
            throw new NotImplementedException();
        }

        public void Set_OC_Mode_AM1(RGB AM1, OC_Mode mode, int band)
        {
            throw new NotImplementedException();
        }

        public void Set_OC_Mode_AM1(RGB_Double AM1_Voltage, OC_Mode mode, int band)
        {
            throw new NotImplementedException();
        }

        public void Set_OC_Mode_Cold_ELVSS_Voltage(double Cold_ELVSS_Voltage, OC_Mode mode, int band)
        {
            throw new NotImplementedException();
        }

        public void Set_OC_Mode_Cold_Vinit2_Voltage(double Cold_Vinit2_Voltage, OC_Mode mode, int band)
        {
            throw new NotImplementedException();
        }

        public void Set_OC_Mode_ELVSS_Voltage(double ELVSS_Voltage, OC_Mode mode, int band)
        {
            throw new NotImplementedException();
        }

        public void Set_OC_Mode_ExtensionXY(OC_Mode mode, int band, int gray, XYLv xylv)
        {
            throw new NotImplementedException();
        }

        public void Set_OC_Mode_Gray(OC_Mode mode, int bandindex, int grayindex, int GrayValue)
        {
            throw new NotImplementedException();
        }

        public void Set_OC_Mode_IsExtensionApplied(OC_Mode mode, int band, int gray, bool IsApplied)
        {
            throw new NotImplementedException();
        }

        public void Set_OC_Mode_Limit(OC_Mode mode, int band, int gray, XYLv xylv)
        {
            throw new NotImplementedException();
        }

        public void Set_OC_Mode_LoopCount(int loopcount, OC_Mode mode, int band, int gray)
        {
            throw new NotImplementedException();
        }

        public void Set_OC_Mode_Measure(XYLv measured, OC_Mode mode, int band, int gray)
        {
            throw new NotImplementedException();
        }

        public void Set_OC_Mode_RGB(RGB Gamma, OC_Mode mode, int band, int gray)
        {
            throw new NotImplementedException();
        }

        public void Set_OC_Mode_RGB(RGB[] Gamma, OC_Mode mode, int band)
        {
            throw new NotImplementedException();
        }

        public void Set_OC_Mode_Target(OC_Mode mode, int band, int gray, XYLv xylv)
        {
            throw new NotImplementedException();
        }

        public void Set_OC_Mode_Vinit2_Voltage(double Vinit2_Voltage, OC_Mode mode, int band)
        {
            throw new NotImplementedException();
        }

        public void Set_OC_Mode_Vreg1(int Vreg1, OC_Mode mode, int band)
        {
            throw new NotImplementedException();
        }
    }
}
