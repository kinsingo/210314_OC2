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
    public class DP213_OCGray
    {
        //OC Parmeters need to be read from csv.
        int[,] OC_Mode1_Gray = new int[DP213_Static.Max_Band_Amount, DP213_Static.Max_Gray_Amount];
        int[,] OC_Mode2_Gray = new int[DP213_Static.Max_Band_Amount, DP213_Static.Max_Gray_Amount];
        int[,] OC_Mode3_Gray = new int[DP213_Static.Max_Band_Amount, DP213_Static.Max_Gray_Amount];
        int[,] OC_Mode4_Gray = new int[DP213_Static.Max_Band_Amount, DP213_Static.Max_Gray_Amount];
        int[,] OC_Mode5_Gray = new int[DP213_Static.Max_Band_Amount, DP213_Static.Max_Gray_Amount];
        int[,] OC_Mode6_Gray = new int[DP213_Static.Max_Band_Amount, DP213_Static.Max_Gray_Amount];

        public int Get_OC_Mode_Gray(OC_Mode mode, int bandindex, int grayindex)
        {
            if (mode == OC_Mode.Mode1) return OC_Mode1_Gray[bandindex, grayindex];
            if (mode == OC_Mode.Mode2) return OC_Mode2_Gray[bandindex, grayindex];
            if (mode == OC_Mode.Mode3) return OC_Mode3_Gray[bandindex, grayindex];
            if (mode == OC_Mode.Mode4) return OC_Mode4_Gray[bandindex, grayindex];
            if (mode == OC_Mode.Mode5) return OC_Mode5_Gray[bandindex, grayindex];
            if (mode == OC_Mode.Mode6) return OC_Mode6_Gray[bandindex, grayindex];
            throw new Exception("Mode Should be 1~6");
        }

        public void Set_OC_Mode_Gray(OC_Mode mode, int bandindex, int grayindex,int GrayValue)
        {
            if (mode == OC_Mode.Mode1) OC_Mode1_Gray[bandindex, grayindex] = GrayValue;
            else if (mode == OC_Mode.Mode2) OC_Mode2_Gray[bandindex, grayindex] = GrayValue;
            else if (mode == OC_Mode.Mode3) OC_Mode3_Gray[bandindex, grayindex] = GrayValue;
            else if (mode == OC_Mode.Mode4) OC_Mode4_Gray[bandindex, grayindex] = GrayValue;
            else if (mode == OC_Mode.Mode5) OC_Mode5_Gray[bandindex, grayindex] = GrayValue;
            else if (mode == OC_Mode.Mode6) OC_Mode6_Gray[bandindex, grayindex] = GrayValue;
            else throw new Exception("Mode Should be 1~6");
        }

    }
}
