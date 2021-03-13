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
    public class DP213_OCExtensionXY
    {

        XYLv[,] OC_Mode1_ExtensionXY = new XYLv[DP213_Static.Max_Band_Amount, DP213_Static.Max_Gray_Amount];
        XYLv[,] OC_Mode2_ExtensionXY = new XYLv[DP213_Static.Max_Band_Amount, DP213_Static.Max_Gray_Amount];
        XYLv[,] OC_Mode3_ExtensionXY = new XYLv[DP213_Static.Max_Band_Amount, DP213_Static.Max_Gray_Amount];
        XYLv[,] OC_Mode4_ExtensionXY = new XYLv[DP213_Static.Max_Band_Amount, DP213_Static.Max_Gray_Amount];
        XYLv[,] OC_Mode5_ExtensionXY = new XYLv[DP213_Static.Max_Band_Amount, DP213_Static.Max_Gray_Amount];
        XYLv[,] OC_Mode6_ExtensionXY = new XYLv[DP213_Static.Max_Band_Amount, DP213_Static.Max_Gray_Amount];

        public XYLv Get_OC_Mode_ExtensionXY(OC_Mode mode, int band, int gray)
        {
            if (mode == OC_Mode.Mode1) return OC_Mode1_ExtensionXY[band, gray];
            if (mode == OC_Mode.Mode2) return OC_Mode2_ExtensionXY[band, gray];
            if (mode == OC_Mode.Mode3) return OC_Mode3_ExtensionXY[band, gray];
            if (mode == OC_Mode.Mode4) return OC_Mode4_ExtensionXY[band, gray];
            if (mode == OC_Mode.Mode5) return OC_Mode5_ExtensionXY[band, gray];
            if (mode == OC_Mode.Mode6) return OC_Mode6_ExtensionXY[band, gray];
            throw new Exception("Mode Should be 1~6");
        }

        public void Set_OC_Mode_ExtensionXY(OC_Mode mode, int band, int gray,XYLv xylv)
        {
            if (mode == OC_Mode.Mode1) OC_Mode1_ExtensionXY[band, gray] = xylv;
            else if (mode == OC_Mode.Mode2) OC_Mode2_ExtensionXY[band, gray] = xylv;
            else if (mode == OC_Mode.Mode3) OC_Mode3_ExtensionXY[band, gray] = xylv;
            else if (mode == OC_Mode.Mode4) OC_Mode4_ExtensionXY[band, gray] = xylv;
            else if (mode == OC_Mode.Mode5) OC_Mode5_ExtensionXY[band, gray] = xylv;
            else if (mode == OC_Mode.Mode6) OC_Mode6_ExtensionXY[band, gray] = xylv;
            else throw new Exception("Mode Should be 1~6");
        }


        bool[,] OC_Mode1_IsExtensionApplied = new bool[DP213_Static.Max_Band_Amount, DP213_Static.Max_Gray_Amount];
        bool[,] OC_Mode2_IsExtensionApplied = new bool[DP213_Static.Max_Band_Amount, DP213_Static.Max_Gray_Amount];
        bool[,] OC_Mode3_IsExtensionApplied = new bool[DP213_Static.Max_Band_Amount, DP213_Static.Max_Gray_Amount];
        bool[,] OC_Mode4_IsExtensionApplied = new bool[DP213_Static.Max_Band_Amount, DP213_Static.Max_Gray_Amount];
        bool[,] OC_Mode5_IsExtensionApplied = new bool[DP213_Static.Max_Band_Amount, DP213_Static.Max_Gray_Amount];
        bool[,] OC_Mode6_IsExtensionApplied = new bool[DP213_Static.Max_Band_Amount, DP213_Static.Max_Gray_Amount];

        public bool Get_OC_Mode_IsExtensionApplied(OC_Mode mode, int band, int gray)
        {
            if (mode == OC_Mode.Mode1) return OC_Mode1_IsExtensionApplied[band, gray];
            if (mode == OC_Mode.Mode2) return OC_Mode2_IsExtensionApplied[band, gray];
            if (mode == OC_Mode.Mode3) return OC_Mode3_IsExtensionApplied[band, gray];
            if (mode == OC_Mode.Mode4) return OC_Mode4_IsExtensionApplied[band, gray];
            if (mode == OC_Mode.Mode5) return OC_Mode5_IsExtensionApplied[band, gray];
            if (mode == OC_Mode.Mode6) return OC_Mode6_IsExtensionApplied[band, gray];
            throw new Exception("Mode Should be 1~6");
        }

        public void Set_OC_Mode_IsExtensionApplied(OC_Mode mode, int band, int gray, bool IsApplied)
        {
            if (mode == OC_Mode.Mode1) OC_Mode1_IsExtensionApplied[band, gray] = IsApplied;
            else if (mode == OC_Mode.Mode2) OC_Mode2_IsExtensionApplied[band, gray] = IsApplied;
            else if (mode == OC_Mode.Mode3) OC_Mode3_IsExtensionApplied[band, gray] = IsApplied;
            else if (mode == OC_Mode.Mode4) OC_Mode4_IsExtensionApplied[band, gray] = IsApplied;
            else if (mode == OC_Mode.Mode5) OC_Mode5_IsExtensionApplied[band, gray] = IsApplied;
            else if (mode == OC_Mode.Mode6) OC_Mode6_IsExtensionApplied[band, gray] = IsApplied;
            else throw new Exception("Mode Should be 1~6");
        }
    }
}
