using System;
using LGD_OC_AstractPlatForm.Enums;
using LGD_OC_AstractPlatForm.CommonAPI;
using LGD_OC_AstractPlatForm.OpticCompensation.DP213.AODCompensation;
using LGD_OC_AstractPlatForm.OpticCompensation.DP213.BlackCompensation;
using LGD_OC_AstractPlatForm.OpticCompensation.DP213.ELVSSCompensation;
using LGD_OC_AstractPlatForm.OpticCompensation.DP213.GrayLowReferenceCompensation;
using LGD_OC_AstractPlatForm.OpticCompensation.DP213.MainCompensation;
using LGD_OC_AstractPlatForm.OpticCompensation.DP213.WhiteCompensation;
using LGD_OC_AstractPlatForm.OpticCompensation.DP213.Data;
using BSQH_Csharp_Library;

namespace LGD_OC_AstractPlatForm.OpticCompensation.DP213
{
    public class DP213OCSet
    {
        static bool Initialized = false;
        static Gamma_Set OCMode1_GammaSet;
        static Gamma_Set OCMode2_GammaSet;
        static Gamma_Set OCMode3_GammaSet;
        static Gamma_Set OCMode4_GammaSet;
        static Gamma_Set OCMode5_GammaSet;
        static Gamma_Set OCMode6_GammaSet;
        public static Gamma_Set GetGammaSet(OC_Mode mode)
        {
            if (Initialized == false) Initialize();
            if (mode == OC_Mode.Mode1) return OCMode1_GammaSet;
            if (mode == OC_Mode.Mode2) return OCMode2_GammaSet;
            if (mode == OC_Mode.Mode3) return OCMode3_GammaSet;
            if (mode == OC_Mode.Mode4) return OCMode4_GammaSet;
            if (mode == OC_Mode.Mode5) return OCMode5_GammaSet;
            if (mode == OC_Mode.Mode6) return OCMode6_GammaSet;
            throw new Exception("Mode Should be 1~6");
        }

        static OC_Mode ELVSSCompensation_OCMode;
        static double ELVSS_Voltage_Max;
        static double ELVSS_Voltage_Min;

        public static OC_Mode Get_ELVSS_OCMode()
        {
            if (Initialized == false) Initialize();
            return ELVSSCompensation_OCMode;
        }

        public static double Get_ELVSS_Voltage_Max()
        {
            if (Initialized == false) Initialize();
            return ELVSS_Voltage_Max;
        }

        public static double Get_ELVSS_Voltage_Min()
        {
            if (Initialized == false) Initialize();
            return ELVSS_Voltage_Min;
        }

        static OC_Mode WhiteCompensation_OCMode;

        public static OC_Mode Get_WhiteCompensation_OCMode()
        {
            if (Initialized == false) Initialize();
            return WhiteCompensation_OCMode;
        }


        static OC_Mode BlackCompensation_OCMode;
        public static OC_Mode Get_BlackCompensation_OCMode()
        {
            if (Initialized == false) Initialize();
            return BlackCompensation_OCMode;
        }


        static int MaxLoopCount;
        static double SkipTargetLv;

        public static int Get_MaxLoopCount()
        {
            if (Initialized == false) Initialize();
            return MaxLoopCount;
        }

        public static double Get_SkipTargetLv()
        {
            if (Initialized == false) Initialize();
            return SkipTargetLv;
        }


        static double[] ELVSS_Offset_OCMode1;
        static double[] ELVSS_Offset_OCMode2;
        static double[] ELVSS_Offset_OCMode3;
        static double[] ELVSS_Offset_OCMode4;
        static double[] ELVSS_Offset_OCMode5;
        static double[] ELVSS_Offset_OCMode6;

        static double[] Vinit2_Offset_OCMode1;
        static double[] Vinit2_Offset_OCMode2;
        static double[] Vinit2_Offset_OCMode3;
        static double[] Vinit2_Offset_OCMode4;
        static double[] Vinit2_Offset_OCMode5;
        static double[] Vinit2_Offset_OCMode6;

        public static double Get_ELVSS_Offset(OC_Mode mode, int band)
        {
            if (Initialized == false) Initialize();
            if (mode == OC_Mode.Mode1) return ELVSS_Offset_OCMode1[band];
            if (mode == OC_Mode.Mode2) return ELVSS_Offset_OCMode2[band];
            if (mode == OC_Mode.Mode3) return ELVSS_Offset_OCMode3[band];
            if (mode == OC_Mode.Mode4) return ELVSS_Offset_OCMode4[band];
            if (mode == OC_Mode.Mode5) return ELVSS_Offset_OCMode5[band];
            if (mode == OC_Mode.Mode6) return ELVSS_Offset_OCMode6[band];
            throw new Exception("Mode Should be 1~6");
        }

        public static double Get_Vinit2_Offset(OC_Mode mode, int band)
        {
            if (Initialized == false) Initialize();
            if (mode == OC_Mode.Mode1) return Vinit2_Offset_OCMode1[band];
            if (mode == OC_Mode.Mode2) return Vinit2_Offset_OCMode2[band];
            if (mode == OC_Mode.Mode3) return Vinit2_Offset_OCMode3[band];
            if (mode == OC_Mode.Mode4) return Vinit2_Offset_OCMode4[band];
            if (mode == OC_Mode.Mode5) return Vinit2_Offset_OCMode5[band];
            if (mode == OC_Mode.Mode6) return Vinit2_Offset_OCMode6[band];
            throw new Exception("Mode Should be 1~6");
        }

        static double VREF0_Margin;
        public static double Get_VREF0_Margin()
        {
            if (Initialized == false) Initialize();
            return VREF0_Margin;
        }


        static double REF4095_Margin;
        static double Black_Limit_Lv;
        static RGB_Double AM0_Margin;
        public static double Get_REF4095_Margin()
        {
            if (Initialized == false) Initialize();
            return REF4095_Margin;
        }
        public static double Get_Black_Limit_Lv()
        {
            if (Initialized == false) Initialize();
            return Black_Limit_Lv;
        }

        public static RGB_Double Get_AM0_Margin()
        {
            if (Initialized == false) Initialize();
            return AM0_Margin;
        }


        static OC_Mode LowGrayRefCompensastion_OCMode;
        static RGB_Double AM1_Margin;
        public static OC_Mode Get_LowGrayRefCompensastion_OCMode()
        {
            if (Initialized == false) Initialize();
            return LowGrayRefCompensastion_OCMode;
        }

        public static RGB_Double Get_AM1_Margin()
        {
            if (Initialized == false) Initialize();
            return AM1_Margin;
        }


        static int[] AOD_LeftTopPos;
        static int[] AOD_RightBottomPos;
        public static int[] Get_AOD_LeftTopPos()
        {
            if (Initialized == false) Initialize();
            return AOD_LeftTopPos;
        }

        public static int[] Get_AOD_RightBottomPos()
        {
            if (Initialized == false) Initialize();
            return AOD_RightBottomPos;
        }


        static bool IsAODCompensationApply;
        public static bool Get_IsAODCompensationApply()
        {
            if (Initialized == false) Initialize();
            return IsAODCompensationApply;
        }
        static bool IsBlackCompensationApply;
        public static bool Get_IsBlackCompensationApply()
        {
            if (Initialized == false) Initialize();
            return IsBlackCompensationApply;
        }
        static bool IsELVSSCompensationApply;
        public static bool Get_IsELVSSCompensationApply()
        {
            if (Initialized == false) Initialize();
            return IsELVSSCompensationApply;
        }
        static bool IsGrayLowRefCompensationApply;
        public static bool Get_IsGrayLowRefCompensationApply()
        {
            if (Initialized == false) Initialize();
            return IsGrayLowRefCompensationApply;
        }
        static bool IsWhiteCompensationApply;
        public static bool Get_IsWhiteCompensationApply()
        {
            if (Initialized == false) Initialize();
            return IsWhiteCompensationApply;
        }
        static bool IsMain123CompensationApply;
        public static bool Get_IsMain123CompensationApply()
        {
            if (Initialized == false) Initialize();
            return IsMain123CompensationApply;
        }

        static bool IsMain456CompensationApply;
        public static bool Get_IsMain456CompensationApply()
        {
            if (Initialized == false) Initialize();
            return IsMain456CompensationApply;
        }

        static int mode456_max_skip_band;
        public static int Get_mode456_max_skip_band()
        {
            if (Initialized == false) Initialize();
            return mode456_max_skip_band;
        }

        public static void Initialize()
        {
            OCMode1_GammaSet = Gamma_Set.Set2;
            OCMode2_GammaSet = Gamma_Set.Set1;
            OCMode3_GammaSet = Gamma_Set.Set3;
            OCMode4_GammaSet = Gamma_Set.Set5;
            OCMode5_GammaSet = Gamma_Set.Set4;
            OCMode6_GammaSet = Gamma_Set.Set6;

            MaxLoopCount = 500;
            SkipTargetLv = 0.02;

            //ELVSS
            IsELVSSCompensationApply = true;
            ELVSSCompensation_OCMode = OC_Mode.Mode1;
            ELVSS_Voltage_Max = -2.0;
            ELVSS_Voltage_Min = -2.5;

            ELVSS_Offset_OCMode1 = new double[] { -2.0, -1.0, 0.0, 0.3, 0.5, 0.5, 0.5, 0.5, 0.5, 0.5, 0.5, 0.5 };
            ELVSS_Offset_OCMode2 = new double[] { -2.0, -1.0, 0.0, 0.3, 0.5, 0.5, 0.5, 0.5, 0.5, 0.5, 0.5, 0.5 };
            ELVSS_Offset_OCMode3 = new double[] { -2.0, -1.0, 0.0, 0.3, 0.5, 0.5, 0.5, 0.5, 0.5, 0.5, 0.5, 0.5 };
            ELVSS_Offset_OCMode4 = new double[] { -2.0, -1.0, 0.0, 0.3, 0.5, 0.5, 0.5, 0.5, 0.5, 0.5, 0.5, 0.5 };
            ELVSS_Offset_OCMode5 = new double[] { -2.0, -1.0, 0.0, 0.3, 0.5, 0.5, 0.5, 0.5, 0.5, 0.5, 0.5, 0.5 };
            ELVSS_Offset_OCMode6 = new double[] { -2.0, -1.0, 0.0, 0.3, 0.5, 0.5, 0.5, 0.5, 0.5, 0.5, 0.5, 0.5 };

            Vinit2_Offset_OCMode1 = new double[] { 0.5, 0.5, 0.5, 0.5, 0.5, 0.5, 0.5, 0.5, 0.5, 0.5, 0.5, 0.5 };
            Vinit2_Offset_OCMode2 = new double[] { 0.4, 0.4, 0.4, 0.4, 0.4, 0.4, 0.4, 0.4, 0.4, 0.4, 0.4, 0.4 };
            Vinit2_Offset_OCMode3 = new double[] { 0.6, 0.6, 0.6, 0.6, 0.6, 0.6, 0.6, 0.6, 0.6, 0.6, 0.6, 0.6 };
            Vinit2_Offset_OCMode4 = new double[] { 0.5, 0.5, 0.5, 0.5, 0.5, 0.5, 0.5, 0.5, 0.5, 0.5, 0.5, 0.5 };
            Vinit2_Offset_OCMode5 = new double[] { 0.4, 0.4, 0.4, 0.4, 0.4, 0.4, 0.4, 0.4, 0.4, 0.4, 0.4, 0.4 };
            Vinit2_Offset_OCMode6 = new double[] { 0.6, 0.6, 0.6, 0.6, 0.6, 0.6, 0.6, 0.6, 0.6, 0.6, 0.6, 0.6 };

            //White
            IsWhiteCompensationApply = true;
            WhiteCompensation_OCMode = OC_Mode.Mode1;
            VREF0_Margin = 0.2;

            //Black
            IsBlackCompensationApply = true;
            BlackCompensation_OCMode = OC_Mode.Mode1;
            REF4095_Margin = 0.6;
            Black_Limit_Lv = 0.005;
            AM0_Margin = new RGB_Double();
            AM0_Margin.double_R = 0.0;
            AM0_Margin.double_G = 0.1;
            AM0_Margin.double_B = 0.1;

            //AM1
            IsGrayLowRefCompensationApply = true;
            LowGrayRefCompensastion_OCMode = OC_Mode.Mode1;
            AM1_Margin = new RGB_Double();
            AM1_Margin.double_R = 0.3;
            AM1_Margin.double_G = 0.2;
            AM1_Margin.double_B = 0.2;


            //AOD
            IsAODCompensationApply = true;
            AOD_LeftTopPos = new int[2] {326,1062};
            AOD_RightBottomPos = new int[2] {902,1638};

            //Main123456
            IsMain123CompensationApply = true;
            IsMain456CompensationApply = true;
            mode456_max_skip_band = 4;
            
            Initialized = true;
        }
    }
}
