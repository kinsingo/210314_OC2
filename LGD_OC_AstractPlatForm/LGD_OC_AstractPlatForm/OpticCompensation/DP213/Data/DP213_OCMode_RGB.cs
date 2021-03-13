using System;
using System.IO;
using System.Data;
using System.Linq;
using BSQH_Csharp_Library;
using System.Windows.Forms;
using System.Text;
using LGD_OC_AstractPlatForm.CommonAPI;
using System.Drawing;

namespace LGD_OC_AstractPlatForm.OpticCompensation.DP213.Data
{
    public class DP213_OCMode_RGB
    {
        DP213_OCREF0REF4095 ref0ref4095;
        DP213_OCVreg1 vreg1;
        DP213_OCAM1AM0 am1am0;
        DataProtocal dprotocal;
        public DP213_OCMode_RGB(DP213_OCREF0REF4095 _ref0ref4095, DP213_OCVreg1 _vreg1, DP213_OCAM1AM0 _am1am0, DataProtocal _dprotocal)
        {
            ref0ref4095 = _ref0ref4095;
            vreg1 = _vreg1;
            am1am0 = _am1am0;
            dprotocal = _dprotocal;
        }

        RGB[,] OC_Mode1_RGB = new RGB[DP213_Static.Max_Band_Amount , DP213_Static.Max_Gray_Amount];
        RGB[,] OC_Mode2_RGB = new RGB[DP213_Static.Max_Band_Amount , DP213_Static.Max_Gray_Amount];
        RGB[,] OC_Mode3_RGB = new RGB[DP213_Static.Max_Band_Amount , DP213_Static.Max_Gray_Amount];
        RGB[,] OC_Mode4_RGB = new RGB[DP213_Static.Max_Band_Amount , DP213_Static.Max_Gray_Amount];
        RGB[,] OC_Mode5_RGB = new RGB[DP213_Static.Max_Band_Amount , DP213_Static.Max_Gray_Amount];
        RGB[,] OC_Mode6_RGB = new RGB[DP213_Static.Max_Band_Amount , DP213_Static.Max_Gray_Amount];

        RGB_Double[,] OC_Mode1_RGB_Voltage = new RGB_Double[DP213_Static.Max_HBM_and_Normal_Band_Amount, DP213_Static.Max_Gray_Amount];
        RGB_Double[,] OC_Mode2_RGB_Voltage = new RGB_Double[DP213_Static.Max_HBM_and_Normal_Band_Amount, DP213_Static.Max_Gray_Amount];
        RGB_Double[,] OC_Mode3_RGB_Voltage = new RGB_Double[DP213_Static.Max_HBM_and_Normal_Band_Amount, DP213_Static.Max_Gray_Amount];
        RGB_Double[,] OC_Mode4_RGB_Voltage = new RGB_Double[DP213_Static.Max_HBM_and_Normal_Band_Amount, DP213_Static.Max_Gray_Amount];
        RGB_Double[,] OC_Mode5_RGB_Voltage = new RGB_Double[DP213_Static.Max_HBM_and_Normal_Band_Amount, DP213_Static.Max_Gray_Amount];
        RGB_Double[,] OC_Mode6_RGB_Voltage = new RGB_Double[DP213_Static.Max_HBM_and_Normal_Band_Amount, DP213_Static.Max_Gray_Amount];

        public RGB Get_OC_Mode_RGB(OC_Mode mode, int band, int gray)
        {
            if (mode == OC_Mode.Mode1) return OC_Mode1_RGB[band, gray];
            if (mode == OC_Mode.Mode2) return OC_Mode2_RGB[band, gray];
            if (mode == OC_Mode.Mode3) return OC_Mode3_RGB[band, gray];
            if (mode == OC_Mode.Mode4) return OC_Mode4_RGB[band, gray];
            if (mode == OC_Mode.Mode5) return OC_Mode5_RGB[band, gray];
            if (mode == OC_Mode.Mode6) return OC_Mode6_RGB[band, gray];
            throw new Exception("Mode Should be 1~6");
        }

        public RGB_Double Get_OC_Mode_RGB_Voltage(OC_Mode mode, int band, int gray)
        {
            if (band > DP213_Static.Max_HBM_and_Normal_Band_Amount)
                throw new Exception("band should be less than " + DP213_Static.Max_HBM_and_Normal_Band_Amount);

            RGB[,] OCMode_RGB = Get_OC_Mode_RGB(mode);
            return Update_HBM_Normal_Gamma_Voltage(mode, band, gray, OCMode_RGB[band, gray]);
        }
        
        public RGB[] Get_OC_Mode_RGB(OC_Mode mode, int band)
        {
            RGB[,] OC_Mode_RGB;

            if (mode == OC_Mode.Mode1) OC_Mode_RGB = OC_Mode1_RGB;
            else if (mode == OC_Mode.Mode2) OC_Mode_RGB = OC_Mode2_RGB;
            else if (mode == OC_Mode.Mode3) OC_Mode_RGB = OC_Mode3_RGB;
            else if (mode == OC_Mode.Mode4) OC_Mode_RGB = OC_Mode4_RGB;
            else if (mode == OC_Mode.Mode5) OC_Mode_RGB = OC_Mode5_RGB;
            else if (mode == OC_Mode.Mode6) OC_Mode_RGB = OC_Mode6_RGB;
            else throw new Exception("Mode Should be 1~6");

            RGB[] Ans = new RGB[DP213_Static.Max_Gray_Amount];
            for (int gray = 0; gray < DP213_Static.Max_Gray_Amount; gray++)
                Ans[gray] = OC_Mode_RGB[band, gray];
            return Ans;
        }

        public void Set_OC_Mode_RGB(RGB Gamma, OC_Mode mode, int band, int gray)
        {
            dprotocal.api.WriteLine($"[Mode{mode}/Band{band}/grayindex{gray}] R[{Gamma.int_R}]/G[{Gamma.int_G}]/B[{Gamma.int_B}]",Color.Blue);

            RGB[,] OCMode_RGB = Get_OC_Mode_RGB(mode);
            OCMode_RGB[band, gray] = Gamma;

            if (band < DP213_Static.Max_HBM_and_Normal_Band_Amount)
                Update_HBM_Normal_Gamma_Voltage(mode, band, gray, Gamma);
        }

        public void Set_OC_Mode_RGB(RGB[] Gamma, OC_Mode mode, int band)
        {
            for (int gray = 0; gray < DP213_Static.Max_Gray_Amount; gray++)
                Set_OC_Mode_RGB(Gamma[gray], mode, band, gray);
        }

        private RGB[,] Get_OC_Mode_RGB(OC_Mode mode)
        {
            if (mode == OC_Mode.Mode1) return OC_Mode1_RGB;
            if (mode == OC_Mode.Mode2) return OC_Mode2_RGB;
            if (mode == OC_Mode.Mode3) return OC_Mode3_RGB;
            if (mode == OC_Mode.Mode4) return OC_Mode4_RGB;
            if (mode == OC_Mode.Mode5) return OC_Mode5_RGB;
            if (mode == OC_Mode.Mode6) return OC_Mode6_RGB;
            throw new Exception("Mode Should be 1~6");
        }

        private RGB_Double[,] Get_OC_Mode_RGB_Voltage(OC_Mode mode)
        {
            if (mode == OC_Mode.Mode1) return OC_Mode1_RGB_Voltage;
            if (mode == OC_Mode.Mode2) return OC_Mode2_RGB_Voltage;
            if (mode == OC_Mode.Mode3) return OC_Mode3_RGB_Voltage;
            if (mode == OC_Mode.Mode4) return OC_Mode4_RGB_Voltage;
            if (mode == OC_Mode.Mode5) return OC_Mode5_RGB_Voltage;
            if (mode == OC_Mode.Mode6) return OC_Mode6_RGB_Voltage;
            throw new Exception("Mode Should be 1~6");
        }


        private RGB_Double Update_HBM_Normal_Gamma_Voltage(OC_Mode mode, int band, int gray, RGB New_Gamma)
        {
            RGB_Double[,] OCMode_RGB_Voltage = Get_OC_Mode_RGB_Voltage(mode);

            if (gray == 0)
            {
                OCMode_RGB_Voltage[band, gray].double_R = Imported_my_cpp_dll.DP213_Get_AM2_Gamma_Voltage(ref0ref4095.Get_Normal_REF4095_Voltage(), vreg1.Get_OC_Mode_Vreg1_Voltage(mode, band), New_Gamma.int_R);
                OCMode_RGB_Voltage[band, gray].double_G = Imported_my_cpp_dll.DP213_Get_AM2_Gamma_Voltage(ref0ref4095.Get_Normal_REF4095_Voltage(), vreg1.Get_OC_Mode_Vreg1_Voltage(mode, band), New_Gamma.int_G);
                OCMode_RGB_Voltage[band, gray].double_B = Imported_my_cpp_dll.DP213_Get_AM2_Gamma_Voltage(ref0ref4095.Get_Normal_REF4095_Voltage(), vreg1.Get_OC_Mode_Vreg1_Voltage(mode, band), New_Gamma.int_B);
            }
            else
            {
                RGB_Double Prev_GR_Gamma_Voltage = Get_Prev_GR_Gamma_Voltage(OCMode_RGB_Voltage, band, gray);
                OCMode_RGB_Voltage[band, gray].double_R = Imported_my_cpp_dll.DP213_Get_GR_Gamma_Voltage(am1am0.Get_OC_Mode_AM1_Voltage(mode,band).double_R, Prev_GR_Gamma_Voltage.double_R, New_Gamma.int_R, gray);
                OCMode_RGB_Voltage[band, gray].double_G = Imported_my_cpp_dll.DP213_Get_GR_Gamma_Voltage(am1am0.Get_OC_Mode_AM1_Voltage(mode, band).double_G, Prev_GR_Gamma_Voltage.double_G, New_Gamma.int_G, gray);
                OCMode_RGB_Voltage[band, gray].double_B = Imported_my_cpp_dll.DP213_Get_GR_Gamma_Voltage(am1am0.Get_OC_Mode_AM1_Voltage(mode, band).double_B, Prev_GR_Gamma_Voltage.double_B, New_Gamma.int_B, gray);
            }
            return OCMode_RGB_Voltage[band, gray];
        }

        private RGB_Double Get_Prev_GR_Gamma_Voltage(RGB_Double[,] OCMode_RGB_Voltage, int band, int gray)
        {
            RGB_Double Prev_GR_Gamma_Voltage = new RGB_Double();
            if (gray == 1 || gray == 2 || gray == 3 || gray == 5 || gray == 7 || gray == 9 || gray == 10)
            {
                Prev_GR_Gamma_Voltage.double_R = OCMode_RGB_Voltage[band, (gray - 1)].double_R;
                Prev_GR_Gamma_Voltage.double_G = OCMode_RGB_Voltage[band, (gray - 1)].double_G;
                Prev_GR_Gamma_Voltage.double_B = OCMode_RGB_Voltage[band, (gray - 1)].double_B;
            }

            else if (gray == 4 || gray == 6 || gray == 8)
            {
                Prev_GR_Gamma_Voltage.double_R = OCMode_RGB_Voltage[band, (gray - 2)].double_R;
                Prev_GR_Gamma_Voltage.double_G = OCMode_RGB_Voltage[band, (gray - 2)].double_G;
                Prev_GR_Gamma_Voltage.double_B = OCMode_RGB_Voltage[band, (gray - 2)].double_B;
            }
            return Prev_GR_Gamma_Voltage;
        }
    }
}
