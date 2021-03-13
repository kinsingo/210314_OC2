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
    public class DP213_OCTarget
    {
       
        XYLv[,] OC_Mode1_Target = new XYLv[DP213_Static.Max_Band_Amount , DP213_Static.Max_Gray_Amount];
        XYLv[,] OC_Mode2_Target = new XYLv[DP213_Static.Max_Band_Amount , DP213_Static.Max_Gray_Amount];
        XYLv[,] OC_Mode3_Target = new XYLv[DP213_Static.Max_Band_Amount , DP213_Static.Max_Gray_Amount];
        XYLv[,] OC_Mode4_Target = new XYLv[DP213_Static.Max_Band_Amount , DP213_Static.Max_Gray_Amount];
        XYLv[,] OC_Mode5_Target = new XYLv[DP213_Static.Max_Band_Amount , DP213_Static.Max_Gray_Amount];
        XYLv[,] OC_Mode6_Target = new XYLv[DP213_Static.Max_Band_Amount , DP213_Static.Max_Gray_Amount];
        
        public XYLv Get_OC_Mode_Target(OC_Mode mode, int band, int gray)
        {
            if (mode == OC_Mode.Mode1) return OC_Mode1_Target[band, gray];
            if (mode == OC_Mode.Mode2) return OC_Mode2_Target[band, gray];
            if (mode == OC_Mode.Mode3) return OC_Mode3_Target[band, gray];
            if (mode == OC_Mode.Mode4) return OC_Mode4_Target[band, gray];
            if (mode == OC_Mode.Mode5) return OC_Mode5_Target[band, gray];
            if (mode == OC_Mode.Mode6) return OC_Mode6_Target[band, gray];
            throw new Exception("Mode Should be 1~6");
        }

        public void Set_OC_Mode_Target(OC_Mode mode, int band, int gray, XYLv xylv)
        {
            if (mode == OC_Mode.Mode1) OC_Mode1_Target[band, gray] = xylv;
            else if (mode == OC_Mode.Mode2) OC_Mode2_Target[band, gray] = xylv;
            else if (mode == OC_Mode.Mode3) OC_Mode3_Target[band, gray] = xylv;
            else if (mode == OC_Mode.Mode4) OC_Mode4_Target[band, gray] = xylv;
            else if (mode == OC_Mode.Mode5) OC_Mode5_Target[band, gray] = xylv;
            else if (mode == OC_Mode.Mode6) OC_Mode6_Target[band, gray] = xylv;
            else throw new Exception("Mode Should be 1~6");
        }

    }
}
