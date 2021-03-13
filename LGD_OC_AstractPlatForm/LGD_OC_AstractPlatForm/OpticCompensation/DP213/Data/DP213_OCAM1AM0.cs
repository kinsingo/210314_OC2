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
    public class DP213_OCAM1AM0
    {
        DP213_OCAM1 am1;
        DP213_OCAM0 am0;

        public DP213_OCAM1AM0(DP213_OCREF0REF4095 _ref0ref4095, DP213_OCVreg1 _vreg1)
        {
            am1 = new DP213_OCAM1(_ref0ref4095, _vreg1);//Values will be Initilized with zeros
            am0 = new DP213_OCAM0(_ref0ref4095, _vreg1);//Values will be Initilized with zeros
        }
        public RGB Get_OC_Mode_AM1(OC_Mode mode, int band) { return am1.Get_OC_Mode_AM1(mode, band); }
        public RGB_Double Get_OC_Mode_AM1_Voltage(OC_Mode mode, int band) { return am1.Get_OC_Mode_AM1_Voltage(mode, band); }
        public void Set_OC_Mode_AM1(RGB AM1, OC_Mode mode, int band) { am1.Set_OC_Mode_AM1(AM1, mode, band); }
        public void Set_OC_Mode_AM1(RGB_Double AM1_Voltage, OC_Mode mode, int band) { am1.Set_OC_Mode_AM1(AM1_Voltage, mode, band); }
        public RGB Get_OC_Mode_AM0(OC_Mode mode, int band) { return am0.Get_OC_Mode_AM0(mode, band); }
        public RGB_Double Get_OC_Mode_AM0_Voltage(OC_Mode mode, int band) { return am0.Get_OC_Mode_AM0_Voltage(mode, band); }
        public void Set_OC_Mode_AM0(RGB AM0, OC_Mode mode, int band) { am0.Set_OC_Mode_AM0(AM0, mode, band); }
        public void Set_OC_Mode_AM0(RGB_Double AM0_Voltage, OC_Mode mode, int band) { am0.Set_OC_Mode_AM0(AM0_Voltage, mode, band); }
    }

    class DP213_AM1AM0Base
    {
        protected RGB AM1_AM0_BoundaryCheck(RGB AM1_or_AM0)
        {
            if (AM1_or_AM0.int_R > DP213_Static.AM1_AM0_Max) AM1_or_AM0.int_R = DP213_Static.AM1_AM0_Max; if (AM1_or_AM0.int_G > DP213_Static.AM1_AM0_Max) AM1_or_AM0.int_G = DP213_Static.AM1_AM0_Max; if (AM1_or_AM0.int_B > DP213_Static.AM1_AM0_Max) AM1_or_AM0.int_B = DP213_Static.AM1_AM0_Max;
            if (AM1_or_AM0.int_R < 0) AM1_or_AM0.int_R = 0; if (AM1_or_AM0.int_G < 0) AM1_or_AM0.int_G = 0; if (AM1_or_AM0.int_B < 0) AM1_or_AM0.int_B = 0;
            return AM1_or_AM0;
        }
    }

    class DP213_OCAM1 : DP213_AM1AM0Base
    {
        DP213_OCREF0REF4095 ref0ref4095;
        DP213_OCVreg1 vreg1;

        public DP213_OCAM1(DP213_OCREF0REF4095 _ref0ref4095, DP213_OCVreg1 _vreg1)
        {
            vreg1 = _vreg1;
            ref0ref4095 = _ref0ref4095;
            Initalize_As_Zero();
        }

        private void Initalize_As_Zero()
        {
            for (int band = 0; band < DP213_Static.Max_Band_Amount; band++)
            {
                Set_OC_Mode_AM1(new RGB(0), OC_Mode.Mode1, band);
                Set_OC_Mode_AM1(new RGB(0), OC_Mode.Mode2, band);
                Set_OC_Mode_AM1(new RGB(0), OC_Mode.Mode3, band);
                Set_OC_Mode_AM1(new RGB(0), OC_Mode.Mode4, band);
                Set_OC_Mode_AM1(new RGB(0), OC_Mode.Mode5, band);
                Set_OC_Mode_AM1(new RGB(0), OC_Mode.Mode6, band);
            }
        }


        RGB[] OC_Mode1_AM1 = new RGB[DP213_Static.Max_Band_Amount];
        RGB[] OC_Mode2_AM1 = new RGB[DP213_Static.Max_Band_Amount];
        RGB[] OC_Mode3_AM1 = new RGB[DP213_Static.Max_Band_Amount];
        RGB[] OC_Mode4_AM1 = new RGB[DP213_Static.Max_Band_Amount];
        RGB[] OC_Mode5_AM1 = new RGB[DP213_Static.Max_Band_Amount];
        RGB[] OC_Mode6_AM1 = new RGB[DP213_Static.Max_Band_Amount];

        RGB_Double[] OC_Mode1_AM1_Voltage = new RGB_Double[DP213_Static.Max_HBM_and_Normal_Band_Amount];
        RGB_Double[] OC_Mode2_AM1_Voltage = new RGB_Double[DP213_Static.Max_HBM_and_Normal_Band_Amount];
        RGB_Double[] OC_Mode3_AM1_Voltage = new RGB_Double[DP213_Static.Max_HBM_and_Normal_Band_Amount];
        RGB_Double[] OC_Mode4_AM1_Voltage = new RGB_Double[DP213_Static.Max_HBM_and_Normal_Band_Amount];
        RGB_Double[] OC_Mode5_AM1_Voltage = new RGB_Double[DP213_Static.Max_HBM_and_Normal_Band_Amount];
        RGB_Double[] OC_Mode6_AM1_Voltage = new RGB_Double[DP213_Static.Max_HBM_and_Normal_Band_Amount];

        public RGB Get_OC_Mode_AM1(OC_Mode mode, int band)
        {
            if (mode == OC_Mode.Mode1) return OC_Mode1_AM1[band];
            if (mode == OC_Mode.Mode2) return OC_Mode2_AM1[band];
            if (mode == OC_Mode.Mode3) return OC_Mode3_AM1[band];
            if (mode == OC_Mode.Mode4) return OC_Mode4_AM1[band];
            if (mode == OC_Mode.Mode5) return OC_Mode5_AM1[band];
            if (mode == OC_Mode.Mode6) return OC_Mode6_AM1[band];
            throw new Exception("Mode Should be 1~6");
        }

        public RGB_Double Get_OC_Mode_AM1_Voltage(OC_Mode mode, int band)
        {
            if (band > DP213_Static.Max_HBM_and_Normal_Band_Amount)
                throw new Exception("band should be less than " + DP213_Static.Max_HBM_and_Normal_Band_Amount);

            RGB[] OCMode_AM1 = Get_OC_Mode_AM1(mode);
            RGB_Double[] OCMode_AM1_Voltage = Get_OC_Mode_AM1_Voltage(mode);
            OCMode_AM1_Voltage[band].double_R = Imported_my_cpp_dll.DP213_Get_AM1_RGB_Voltage(ref0ref4095.Get_Normal_REF4095_Voltage(), vreg1.Get_OC_Mode_Vreg1_Voltage(mode, band), OCMode_AM1[band].int_R);
            OCMode_AM1_Voltage[band].double_G = Imported_my_cpp_dll.DP213_Get_AM1_RGB_Voltage(ref0ref4095.Get_Normal_REF4095_Voltage(), vreg1.Get_OC_Mode_Vreg1_Voltage(mode, band), OCMode_AM1[band].int_G);
            OCMode_AM1_Voltage[band].double_B = Imported_my_cpp_dll.DP213_Get_AM1_RGB_Voltage(ref0ref4095.Get_Normal_REF4095_Voltage(), vreg1.Get_OC_Mode_Vreg1_Voltage(mode, band), OCMode_AM1[band].int_B);

            return OCMode_AM1_Voltage[band];
        }

        private RGB[] Get_OC_Mode_AM1(OC_Mode mode)
        {
            if (mode == OC_Mode.Mode1) return OC_Mode1_AM1;
            if (mode == OC_Mode.Mode2) return OC_Mode2_AM1;
            if (mode == OC_Mode.Mode3) return OC_Mode3_AM1;
            if (mode == OC_Mode.Mode4) return OC_Mode4_AM1;
            if (mode == OC_Mode.Mode5) return OC_Mode5_AM1;
            if (mode == OC_Mode.Mode6) return OC_Mode6_AM1;
            throw new Exception("Mode Should be 1~6");
        }

        private RGB_Double[] Get_OC_Mode_AM1_Voltage(OC_Mode mode)
        {
            if (mode == OC_Mode.Mode1) return OC_Mode1_AM1_Voltage;
            if (mode == OC_Mode.Mode2) return OC_Mode2_AM1_Voltage;
            if (mode == OC_Mode.Mode3) return OC_Mode3_AM1_Voltage;
            if (mode == OC_Mode.Mode4) return OC_Mode4_AM1_Voltage;
            if (mode == OC_Mode.Mode5) return OC_Mode5_AM1_Voltage;
            if (mode == OC_Mode.Mode6) return OC_Mode6_AM1_Voltage;
            throw new Exception("Mode Should be 1~6");
        }

        public void Set_OC_Mode_AM1(RGB AM1, OC_Mode mode, int band)
        {
            AM1 = AM1_AM0_BoundaryCheck(AM1);
            RGB[] OCMode_AM1 = Get_OC_Mode_AM1(mode);
            RGB_Double[] OCMode_AM1_Voltage = Get_OC_Mode_AM1_Voltage(mode);

            OCMode_AM1[band] = AM1;
            if (band < DP213_Static.Max_HBM_and_Normal_Band_Amount)
            {
                OCMode_AM1_Voltage[band].double_R = Imported_my_cpp_dll.DP213_Get_AM1_RGB_Voltage(ref0ref4095.Get_Normal_REF4095_Voltage(), vreg1.Get_OC_Mode_Vreg1_Voltage(mode, band), AM1.int_R);
                OCMode_AM1_Voltage[band].double_G = Imported_my_cpp_dll.DP213_Get_AM1_RGB_Voltage(ref0ref4095.Get_Normal_REF4095_Voltage(), vreg1.Get_OC_Mode_Vreg1_Voltage(mode, band), AM1.int_G);
                OCMode_AM1_Voltage[band].double_B = Imported_my_cpp_dll.DP213_Get_AM1_RGB_Voltage(ref0ref4095.Get_Normal_REF4095_Voltage(), vreg1.Get_OC_Mode_Vreg1_Voltage(mode, band), AM1.int_B);
            }
        }


        public void Set_OC_Mode_AM1(RGB_Double AM1_Voltage, OC_Mode mode, int band)
        {
            if (band > DP213_Static.Max_HBM_and_Normal_Band_Amount)
                throw new Exception("band should be less than " + DP213_Static.Max_HBM_and_Normal_Band_Amount);

            RGB AM1 = new RGB();
            AM1.int_R = Imported_my_cpp_dll.DP213_Get_AM1_RGB_Dec(ref0ref4095.Get_Normal_REF4095_Voltage(), vreg1.Get_OC_Mode_Vreg1_Voltage(mode, band), AM1_Voltage.double_R);
            AM1.int_G = Imported_my_cpp_dll.DP213_Get_AM1_RGB_Dec(ref0ref4095.Get_Normal_REF4095_Voltage(), vreg1.Get_OC_Mode_Vreg1_Voltage(mode, band), AM1_Voltage.double_G);
            AM1.int_B = Imported_my_cpp_dll.DP213_Get_AM1_RGB_Dec(ref0ref4095.Get_Normal_REF4095_Voltage(), vreg1.Get_OC_Mode_Vreg1_Voltage(mode, band), AM1_Voltage.double_B);
            Set_OC_Mode_AM1(AM1, mode, band);
        }
    }

    class DP213_OCAM0 : DP213_AM1AM0Base
    {
        DP213_OCREF0REF4095 ref0ref4095;
        DP213_OCVreg1 vreg1;

        public DP213_OCAM0(DP213_OCREF0REF4095 _ref0ref4095, DP213_OCVreg1 _vreg1)
        {
            vreg1 = _vreg1;
            ref0ref4095 = _ref0ref4095;
            Initalize_As_Zero();
        }

        private void Initalize_As_Zero()
        {
            for (int band = 0; band < DP213_Static.Max_Band_Amount; band++)
            {
                Set_OC_Mode_AM0(new RGB(0), OC_Mode.Mode1, band);
                Set_OC_Mode_AM0(new RGB(0), OC_Mode.Mode2, band);
                Set_OC_Mode_AM0(new RGB(0), OC_Mode.Mode3, band);
                Set_OC_Mode_AM0(new RGB(0), OC_Mode.Mode4, band);
                Set_OC_Mode_AM0(new RGB(0), OC_Mode.Mode5, band);
                Set_OC_Mode_AM0(new RGB(0), OC_Mode.Mode6, band);
            }
        }


        RGB[] OC_Mode1_AM0 = new RGB[DP213_Static.Max_Band_Amount];
        RGB[] OC_Mode2_AM0 = new RGB[DP213_Static.Max_Band_Amount];
        RGB[] OC_Mode3_AM0 = new RGB[DP213_Static.Max_Band_Amount];
        RGB[] OC_Mode4_AM0 = new RGB[DP213_Static.Max_Band_Amount];
        RGB[] OC_Mode5_AM0 = new RGB[DP213_Static.Max_Band_Amount];
        RGB[] OC_Mode6_AM0 = new RGB[DP213_Static.Max_Band_Amount];

        RGB_Double[] OC_Mode1_AM0_Voltage = new RGB_Double[DP213_Static.Max_HBM_and_Normal_Band_Amount];
        RGB_Double[] OC_Mode2_AM0_Voltage = new RGB_Double[DP213_Static.Max_HBM_and_Normal_Band_Amount];
        RGB_Double[] OC_Mode3_AM0_Voltage = new RGB_Double[DP213_Static.Max_HBM_and_Normal_Band_Amount];
        RGB_Double[] OC_Mode4_AM0_Voltage = new RGB_Double[DP213_Static.Max_HBM_and_Normal_Band_Amount];
        RGB_Double[] OC_Mode5_AM0_Voltage = new RGB_Double[DP213_Static.Max_HBM_and_Normal_Band_Amount];
        RGB_Double[] OC_Mode6_AM0_Voltage = new RGB_Double[DP213_Static.Max_HBM_and_Normal_Band_Amount];

        public RGB Get_OC_Mode_AM0(OC_Mode mode, int band)
        {
            if (mode == OC_Mode.Mode1) return OC_Mode1_AM0[band];
            if (mode == OC_Mode.Mode2) return OC_Mode2_AM0[band];
            if (mode == OC_Mode.Mode3) return OC_Mode3_AM0[band];
            if (mode == OC_Mode.Mode4) return OC_Mode4_AM0[band];
            if (mode == OC_Mode.Mode5) return OC_Mode5_AM0[band];
            if (mode == OC_Mode.Mode6) return OC_Mode6_AM0[band];
            throw new Exception("Mode Should be 1~6");
        }

        public RGB_Double Get_OC_Mode_AM0_Voltage(OC_Mode mode, int band)
        {
            if (band > DP213_Static.Max_HBM_and_Normal_Band_Amount)
                throw new Exception("band should be less than " + DP213_Static.Max_HBM_and_Normal_Band_Amount);

            RGB[] OCMode_AM0 = Get_OC_Mode_AM0(mode);
            RGB_Double[] OCMode_AM0_Voltage = Get_OC_Mode_AM0_Voltage(mode);
            OCMode_AM0_Voltage[band].double_R = Imported_my_cpp_dll.DP213_Get_AM0_RGB_Voltage(ref0ref4095.Get_Normal_REF4095_Voltage(), vreg1.Get_OC_Mode_Vreg1_Voltage(mode, band), OCMode_AM0[band].int_R);
            OCMode_AM0_Voltage[band].double_G = Imported_my_cpp_dll.DP213_Get_AM0_RGB_Voltage(ref0ref4095.Get_Normal_REF4095_Voltage(), vreg1.Get_OC_Mode_Vreg1_Voltage(mode, band), OCMode_AM0[band].int_G);
            OCMode_AM0_Voltage[band].double_B = Imported_my_cpp_dll.DP213_Get_AM0_RGB_Voltage(ref0ref4095.Get_Normal_REF4095_Voltage(), vreg1.Get_OC_Mode_Vreg1_Voltage(mode, band), OCMode_AM0[band].int_B);

            return OCMode_AM0_Voltage[band];
        }

        private RGB[] Get_OC_Mode_AM0(OC_Mode mode)
        {
            if (mode == OC_Mode.Mode1) return OC_Mode1_AM0;
            if (mode == OC_Mode.Mode2) return OC_Mode2_AM0;
            if (mode == OC_Mode.Mode3) return OC_Mode3_AM0;
            if (mode == OC_Mode.Mode4) return OC_Mode4_AM0;
            if (mode == OC_Mode.Mode5) return OC_Mode5_AM0;
            if (mode == OC_Mode.Mode6) return OC_Mode6_AM0;
            throw new Exception("Mode Should be 1~6");
        }

        private RGB_Double[] Get_OC_Mode_AM0_Voltage(OC_Mode mode)
        {
            if (mode == OC_Mode.Mode1) return OC_Mode1_AM0_Voltage;
            if (mode == OC_Mode.Mode2) return OC_Mode2_AM0_Voltage;
            if (mode == OC_Mode.Mode3) return OC_Mode3_AM0_Voltage;
            if (mode == OC_Mode.Mode4) return OC_Mode4_AM0_Voltage;
            if (mode == OC_Mode.Mode5) return OC_Mode5_AM0_Voltage;
            if (mode == OC_Mode.Mode6) return OC_Mode6_AM0_Voltage;
            throw new Exception("Mode Should be 1~6");
        }

        public void Set_OC_Mode_AM0(RGB AM0, OC_Mode mode, int band)
        {
            AM0 = AM1_AM0_BoundaryCheck(AM0);
            RGB[] OCMode_AM0 = Get_OC_Mode_AM0(mode);
            RGB_Double[] OCMode_AM0_Voltage = Get_OC_Mode_AM0_Voltage(mode);

            OCMode_AM0[band] = AM0;
            if (band < DP213_Static.Max_HBM_and_Normal_Band_Amount)
            {
                OCMode_AM0_Voltage[band].double_R = Imported_my_cpp_dll.DP213_Get_AM1_RGB_Voltage(ref0ref4095.Get_Normal_REF4095_Voltage(), vreg1.Get_OC_Mode_Vreg1_Voltage(mode, band), AM0.int_R);
                OCMode_AM0_Voltage[band].double_G = Imported_my_cpp_dll.DP213_Get_AM1_RGB_Voltage(ref0ref4095.Get_Normal_REF4095_Voltage(), vreg1.Get_OC_Mode_Vreg1_Voltage(mode, band), AM0.int_G);
                OCMode_AM0_Voltage[band].double_B = Imported_my_cpp_dll.DP213_Get_AM1_RGB_Voltage(ref0ref4095.Get_Normal_REF4095_Voltage(), vreg1.Get_OC_Mode_Vreg1_Voltage(mode, band), AM0.int_B);
            }
        }

        public void Set_OC_Mode_AM0(RGB_Double AM0_Voltage, OC_Mode mode, int band)
        {
            if (band > DP213_Static.Max_HBM_and_Normal_Band_Amount)
                throw new Exception("band should be less than " + DP213_Static.Max_HBM_and_Normal_Band_Amount);

            RGB AM0 = new RGB();
            AM0.int_R = Imported_my_cpp_dll.DP213_Get_AM0_RGB_Dec(ref0ref4095.Get_Normal_REF4095_Voltage(), vreg1.Get_OC_Mode_Vreg1_Voltage(mode, band), AM0_Voltage.double_R);
            AM0.int_G = Imported_my_cpp_dll.DP213_Get_AM0_RGB_Dec(ref0ref4095.Get_Normal_REF4095_Voltage(), vreg1.Get_OC_Mode_Vreg1_Voltage(mode, band), AM0_Voltage.double_G);
            AM0.int_B = Imported_my_cpp_dll.DP213_Get_AM0_RGB_Dec(ref0ref4095.Get_Normal_REF4095_Voltage(), vreg1.Get_OC_Mode_Vreg1_Voltage(mode, band), AM0_Voltage.double_B);
            Set_OC_Mode_AM0(AM0, mode, band);
        }
    }




}
