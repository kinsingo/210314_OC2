using System;
using System.IO;
using System.Data;
using System.Linq;
using BSQH_Csharp_Library;
using System.Windows.Forms;
using System.Text;
using LGD_OC_AstractPlatForm.CommonAPI;

namespace LGD_OC_AstractPlatForm.OpticCompensation.DP213.Data
{
    public class DP213_OCELVSSVinit2
    {
        double[] OC_Mode1_ELVSS_Voltage = new double[DP213_Static.Max_HBM_and_Normal_Band_Amount];
        double[] OC_Mode2_ELVSS_Voltage = new double[DP213_Static.Max_HBM_and_Normal_Band_Amount];
        double[] OC_Mode3_ELVSS_Voltage = new double[DP213_Static.Max_HBM_and_Normal_Band_Amount];
        double[] OC_Mode4_ELVSS_Voltage = new double[DP213_Static.Max_HBM_and_Normal_Band_Amount];
        double[] OC_Mode5_ELVSS_Voltage = new double[DP213_Static.Max_HBM_and_Normal_Band_Amount];
        double[] OC_Mode6_ELVSS_Voltage = new double[DP213_Static.Max_HBM_and_Normal_Band_Amount];

        public double Get_OC_Mode_ELVSS_Voltage(OC_Mode mode, int band)
        {
            if (mode == OC_Mode.Mode1) return OC_Mode1_ELVSS_Voltage[band];
            if (mode == OC_Mode.Mode2) return OC_Mode2_ELVSS_Voltage[band];
            if (mode == OC_Mode.Mode3) return OC_Mode3_ELVSS_Voltage[band];
            if (mode == OC_Mode.Mode4) return OC_Mode4_ELVSS_Voltage[band];
            if (mode == OC_Mode.Mode5) return OC_Mode5_ELVSS_Voltage[band];
            if (mode == OC_Mode.Mode6) return OC_Mode6_ELVSS_Voltage[band];
            throw new Exception("Mode Should be 1~6");
        }

        public double[] Get_OC_Mode_ELVSS_Voltages(OC_Mode mode)
        {
            if (mode == OC_Mode.Mode1) return OC_Mode1_ELVSS_Voltage;
            if (mode == OC_Mode.Mode2) return OC_Mode2_ELVSS_Voltage;
            if (mode == OC_Mode.Mode3) return OC_Mode3_ELVSS_Voltage;
            if (mode == OC_Mode.Mode4) return OC_Mode4_ELVSS_Voltage;
            if (mode == OC_Mode.Mode5) return OC_Mode5_ELVSS_Voltage;
            if (mode == OC_Mode.Mode6) return OC_Mode6_ELVSS_Voltage;
            throw new Exception("Mode Should be 1~6");
        }

        public void Set_OC_Mode_ELVSS_Voltage(double ELVSS_Voltage, OC_Mode mode, int band)
        {
            if (mode == OC_Mode.Mode1) OC_Mode1_ELVSS_Voltage[band] = ELVSS_Voltage;
            else if (mode == OC_Mode.Mode2) OC_Mode2_ELVSS_Voltage[band] = ELVSS_Voltage;
            else if (mode == OC_Mode.Mode3) OC_Mode3_ELVSS_Voltage[band] = ELVSS_Voltage;
            else if (mode == OC_Mode.Mode4) OC_Mode4_ELVSS_Voltage[band] = ELVSS_Voltage;
            else if (mode == OC_Mode.Mode5) OC_Mode5_ELVSS_Voltage[band] = ELVSS_Voltage;
            else if (mode == OC_Mode.Mode6) OC_Mode6_ELVSS_Voltage[band] = ELVSS_Voltage;
            else throw new Exception("Mode Should be 1~6");
        }

        double[] OC_Mode1_Vinit2_Voltage = new double[DP213_Static.Max_HBM_and_Normal_Band_Amount];
        double[] OC_Mode2_Vinit2_Voltage = new double[DP213_Static.Max_HBM_and_Normal_Band_Amount];
        double[] OC_Mode3_Vinit2_Voltage = new double[DP213_Static.Max_HBM_and_Normal_Band_Amount];
        double[] OC_Mode4_Vinit2_Voltage = new double[DP213_Static.Max_HBM_and_Normal_Band_Amount];
        double[] OC_Mode5_Vinit2_Voltage = new double[DP213_Static.Max_HBM_and_Normal_Band_Amount];
        double[] OC_Mode6_Vinit2_Voltage = new double[DP213_Static.Max_HBM_and_Normal_Band_Amount];

        public double Get_OC_Mode_Vinit2_Voltage(OC_Mode mode, int band)
        {
            if (mode == OC_Mode.Mode1) return OC_Mode1_Vinit2_Voltage[band];
            if (mode == OC_Mode.Mode2) return OC_Mode2_Vinit2_Voltage[band];
            if (mode == OC_Mode.Mode3) return OC_Mode3_Vinit2_Voltage[band];
            if (mode == OC_Mode.Mode4) return OC_Mode4_Vinit2_Voltage[band];
            if (mode == OC_Mode.Mode5) return OC_Mode5_Vinit2_Voltage[band];
            if (mode == OC_Mode.Mode6) return OC_Mode6_Vinit2_Voltage[band];
            throw new Exception("Mode Should be 1~6");
        }

        public double[] Get_OC_Mode_Vinit2_Voltages(OC_Mode mode)
        {
            if (mode == OC_Mode.Mode1) return OC_Mode1_Vinit2_Voltage;
            if (mode == OC_Mode.Mode2) return OC_Mode2_Vinit2_Voltage;
            if (mode == OC_Mode.Mode3) return OC_Mode3_Vinit2_Voltage;
            if (mode == OC_Mode.Mode4) return OC_Mode4_Vinit2_Voltage;
            if (mode == OC_Mode.Mode5) return OC_Mode5_Vinit2_Voltage;
            if (mode == OC_Mode.Mode6) return OC_Mode6_Vinit2_Voltage;
            throw new Exception("Mode Should be 1~6");
        }

        public void Set_OC_Mode_Vinit2_Voltage(double Vinit2_Voltage, OC_Mode mode, int band)
        {
            if (mode == OC_Mode.Mode1) OC_Mode1_Vinit2_Voltage[band] = Vinit2_Voltage;
            else if (mode == OC_Mode.Mode2) OC_Mode2_Vinit2_Voltage[band] = Vinit2_Voltage;
            else if (mode == OC_Mode.Mode3) OC_Mode3_Vinit2_Voltage[band] = Vinit2_Voltage;
            else if (mode == OC_Mode.Mode4) OC_Mode4_Vinit2_Voltage[band] = Vinit2_Voltage;
            else if (mode == OC_Mode.Mode5) OC_Mode5_Vinit2_Voltage[band] = Vinit2_Voltage;
            else if (mode == OC_Mode.Mode6) OC_Mode6_Vinit2_Voltage[band] = Vinit2_Voltage;
            else throw new Exception("Mode Should be 1~6");
        }

        double[] OC_Mode1_Cold_ELVSS_Voltage = new double[DP213_Static.Max_HBM_and_Normal_Band_Amount];
        double[] OC_Mode2_Cold_ELVSS_Voltage = new double[DP213_Static.Max_HBM_and_Normal_Band_Amount];
        double[] OC_Mode3_Cold_ELVSS_Voltage = new double[DP213_Static.Max_HBM_and_Normal_Band_Amount];
        double[] OC_Mode4_Cold_ELVSS_Voltage = new double[DP213_Static.Max_HBM_and_Normal_Band_Amount];
        double[] OC_Mode5_Cold_ELVSS_Voltage = new double[DP213_Static.Max_HBM_and_Normal_Band_Amount];
        double[] OC_Mode6_Cold_ELVSS_Voltage = new double[DP213_Static.Max_HBM_and_Normal_Band_Amount];

        public double Get_OC_Mode_Cold_ELVSS_Voltage(OC_Mode mode, int band)
        {
            if (mode == OC_Mode.Mode1) return OC_Mode1_Cold_ELVSS_Voltage[band];
            if (mode == OC_Mode.Mode2) return OC_Mode2_Cold_ELVSS_Voltage[band];
            if (mode == OC_Mode.Mode3) return OC_Mode3_Cold_ELVSS_Voltage[band];
            if (mode == OC_Mode.Mode4) return OC_Mode4_Cold_ELVSS_Voltage[band];
            if (mode == OC_Mode.Mode5) return OC_Mode5_Cold_ELVSS_Voltage[band];
            if (mode == OC_Mode.Mode6) return OC_Mode6_Cold_ELVSS_Voltage[band];
            throw new Exception("Mode Should be 1~6");
        }

        public double[] Get_OC_Mode_Cold_ELVSS_Voltages(OC_Mode mode)
        {
            if (mode == OC_Mode.Mode1) return OC_Mode1_Cold_ELVSS_Voltage;
            if (mode == OC_Mode.Mode2) return OC_Mode2_Cold_ELVSS_Voltage;
            if (mode == OC_Mode.Mode3) return OC_Mode3_Cold_ELVSS_Voltage;
            if (mode == OC_Mode.Mode4) return OC_Mode4_Cold_ELVSS_Voltage;
            if (mode == OC_Mode.Mode5) return OC_Mode5_Cold_ELVSS_Voltage;
            if (mode == OC_Mode.Mode6) return OC_Mode6_Cold_ELVSS_Voltage;
            throw new Exception("Mode Should be 1~6");
        }

        public void Set_OC_Mode_Cold_ELVSS_Voltage(double Cold_ELVSS_Voltage, OC_Mode mode, int band)
        {
            if (mode == OC_Mode.Mode1) OC_Mode1_Cold_ELVSS_Voltage[band] = Cold_ELVSS_Voltage;
            else if (mode == OC_Mode.Mode2) OC_Mode2_Cold_ELVSS_Voltage[band] = Cold_ELVSS_Voltage;
            else if (mode == OC_Mode.Mode3) OC_Mode3_Cold_ELVSS_Voltage[band] = Cold_ELVSS_Voltage;
            else if (mode == OC_Mode.Mode4) OC_Mode4_Cold_ELVSS_Voltage[band] = Cold_ELVSS_Voltage;
            else if (mode == OC_Mode.Mode5) OC_Mode5_Cold_ELVSS_Voltage[band] = Cold_ELVSS_Voltage;
            else if (mode == OC_Mode.Mode6) OC_Mode6_Cold_ELVSS_Voltage[band] = Cold_ELVSS_Voltage;
            else throw new Exception("Mode Should be 1~6");
        }

        double[] OC_Mode1_Cold_Vinit2_Voltage = new double[DP213_Static.Max_HBM_and_Normal_Band_Amount];
        double[] OC_Mode2_Cold_Vinit2_Voltage = new double[DP213_Static.Max_HBM_and_Normal_Band_Amount];
        double[] OC_Mode3_Cold_Vinit2_Voltage = new double[DP213_Static.Max_HBM_and_Normal_Band_Amount];
        double[] OC_Mode4_Cold_Vinit2_Voltage = new double[DP213_Static.Max_HBM_and_Normal_Band_Amount];
        double[] OC_Mode5_Cold_Vinit2_Voltage = new double[DP213_Static.Max_HBM_and_Normal_Band_Amount];
        double[] OC_Mode6_Cold_Vinit2_Voltage = new double[DP213_Static.Max_HBM_and_Normal_Band_Amount];

        public double Get_OC_Mode_Cold_Vinit2_Voltage(OC_Mode mode, int band)
        {
            if (mode == OC_Mode.Mode1) return OC_Mode1_Cold_Vinit2_Voltage[band];
            if (mode == OC_Mode.Mode2) return OC_Mode2_Cold_Vinit2_Voltage[band];
            if (mode == OC_Mode.Mode3) return OC_Mode3_Cold_Vinit2_Voltage[band];
            if (mode == OC_Mode.Mode4) return OC_Mode4_Cold_Vinit2_Voltage[band];
            if (mode == OC_Mode.Mode5) return OC_Mode5_Cold_Vinit2_Voltage[band];
            if (mode == OC_Mode.Mode6) return OC_Mode6_Cold_Vinit2_Voltage[band];
            throw new Exception("Mode Should be 1~6");
        }

        public double[] Get_OC_Mode_Cold_Vinit2_Voltages(OC_Mode mode)
        {
            if (mode == OC_Mode.Mode1) return OC_Mode1_Cold_Vinit2_Voltage;
            if (mode == OC_Mode.Mode2) return OC_Mode2_Cold_Vinit2_Voltage;
            if (mode == OC_Mode.Mode3) return OC_Mode3_Cold_Vinit2_Voltage;
            if (mode == OC_Mode.Mode4) return OC_Mode4_Cold_Vinit2_Voltage;
            if (mode == OC_Mode.Mode5) return OC_Mode5_Cold_Vinit2_Voltage;
            if (mode == OC_Mode.Mode6) return OC_Mode6_Cold_Vinit2_Voltage;
            throw new Exception("Mode Should be 1~6");
        }

        public void Set_OC_Mode_Cold_Vinit2_Voltage(double Cold_Vinit2_Voltage, OC_Mode mode, int band)
        {
            if (mode == OC_Mode.Mode1) OC_Mode1_Cold_Vinit2_Voltage[band] = Cold_Vinit2_Voltage;
            else if (mode == OC_Mode.Mode2) OC_Mode2_Cold_Vinit2_Voltage[band] = Cold_Vinit2_Voltage;
            else if (mode == OC_Mode.Mode3) OC_Mode3_Cold_Vinit2_Voltage[band] = Cold_Vinit2_Voltage;
            else if (mode == OC_Mode.Mode4) OC_Mode4_Cold_Vinit2_Voltage[band] = Cold_Vinit2_Voltage;
            else if (mode == OC_Mode.Mode5) OC_Mode5_Cold_Vinit2_Voltage[band] = Cold_Vinit2_Voltage;
            else if (mode == OC_Mode.Mode6) OC_Mode6_Cold_Vinit2_Voltage[band] = Cold_Vinit2_Voltage;
            else throw new Exception("Mode Should be 1~6");
        }
    }
}
