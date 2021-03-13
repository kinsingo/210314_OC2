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
    public class DP213_OCLoopCount
    {
        int[,] OC_Mode1_LoopCount = new int[DP213_Static.Max_Band_Amount , DP213_Static.Max_Gray_Amount];
        int[,] OC_Mode2_LoopCount = new int[DP213_Static.Max_Band_Amount , DP213_Static.Max_Gray_Amount];
        int[,] OC_Mode3_LoopCount = new int[DP213_Static.Max_Band_Amount , DP213_Static.Max_Gray_Amount];
        int[,] OC_Mode4_LoopCount = new int[DP213_Static.Max_Band_Amount , DP213_Static.Max_Gray_Amount];
        int[,] OC_Mode5_LoopCount = new int[DP213_Static.Max_Band_Amount , DP213_Static.Max_Gray_Amount];
        int[,] OC_Mode6_LoopCount = new int[DP213_Static.Max_Band_Amount , DP213_Static.Max_Gray_Amount];

        public int Get_OC_Mode_LoopCount(OC_Mode mode,int band,int gray)
        {
            if (mode == OC_Mode.Mode1) return OC_Mode1_LoopCount[band, gray];
            if (mode == OC_Mode.Mode2) return OC_Mode2_LoopCount[band, gray];
            if (mode == OC_Mode.Mode3) return OC_Mode3_LoopCount[band, gray];
            if (mode == OC_Mode.Mode4) return OC_Mode4_LoopCount[band, gray];
            if (mode == OC_Mode.Mode5) return OC_Mode5_LoopCount[band, gray];
            if (mode == OC_Mode.Mode6) return OC_Mode6_LoopCount[band, gray];
            throw new Exception("Mode Should be 1~6");
        }

        public void Set_OC_Mode_LoopCount(int loopcount, OC_Mode mode, int band, int gray)
        {
            if (mode == OC_Mode.Mode1) OC_Mode1_LoopCount[band, gray] = loopcount;
            else if (mode == OC_Mode.Mode2) OC_Mode2_LoopCount[band, gray] = loopcount;
            else if (mode == OC_Mode.Mode3) OC_Mode3_LoopCount[band, gray] = loopcount;
            else if (mode == OC_Mode.Mode4) OC_Mode4_LoopCount[band, gray] = loopcount;
            else if (mode == OC_Mode.Mode5) OC_Mode5_LoopCount[band, gray] = loopcount;
            else if (mode == OC_Mode.Mode6) OC_Mode6_LoopCount[band, gray] = loopcount;
            else throw new Exception("Mode Should be 1~6");
        }
    }
}
