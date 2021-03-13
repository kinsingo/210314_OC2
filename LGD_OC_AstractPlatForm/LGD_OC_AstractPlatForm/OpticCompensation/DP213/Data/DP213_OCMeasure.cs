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
    public class DP213_OCMeasure
    {
        XYLv[,] OC_Mode1_Measure = new XYLv[DP213_Static.Max_Band_Amount , DP213_Static.Max_Gray_Amount];
        XYLv[,] OC_Mode2_Measure = new XYLv[DP213_Static.Max_Band_Amount , DP213_Static.Max_Gray_Amount];
        XYLv[,] OC_Mode3_Measure = new XYLv[DP213_Static.Max_Band_Amount , DP213_Static.Max_Gray_Amount];
        XYLv[,] OC_Mode4_Measure = new XYLv[DP213_Static.Max_Band_Amount , DP213_Static.Max_Gray_Amount];
        XYLv[,] OC_Mode5_Measure = new XYLv[DP213_Static.Max_Band_Amount , DP213_Static.Max_Gray_Amount];
        XYLv[,] OC_Mode6_Measure = new XYLv[DP213_Static.Max_Band_Amount , DP213_Static.Max_Gray_Amount];

        public XYLv Get_OC_Mode_Measure(OC_Mode mode, int band, int gray)
        {
            if (mode == OC_Mode.Mode1) return OC_Mode1_Measure[band, gray];
            if (mode == OC_Mode.Mode2) return OC_Mode2_Measure[band, gray];
            if (mode == OC_Mode.Mode3) return OC_Mode3_Measure[band, gray];
            if (mode == OC_Mode.Mode4) return OC_Mode4_Measure[band, gray];
            if (mode == OC_Mode.Mode5) return OC_Mode5_Measure[band, gray];
            if (mode == OC_Mode.Mode6) return OC_Mode6_Measure[band, gray];
            throw new Exception("Mode Should be 1~6");
        }

        public void Set_OC_Mode_Measure(XYLv measured,OC_Mode mode, int band, int gray)
        {
            if (mode == OC_Mode.Mode1) OC_Mode1_Measure[band, gray] = measured;
            else if (mode == OC_Mode.Mode2) OC_Mode2_Measure[band, gray] = measured;
            else if (mode == OC_Mode.Mode3) OC_Mode3_Measure[band, gray] = measured;
            else if (mode == OC_Mode.Mode4) OC_Mode4_Measure[band, gray] = measured;
            else if (mode == OC_Mode.Mode5) OC_Mode5_Measure[band, gray] = measured;
            else if (mode == OC_Mode.Mode6) OC_Mode6_Measure[band, gray] = measured; 
            else throw new Exception("Mode Should be 1~6");
        }
    }
}
