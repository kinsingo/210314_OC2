using BSQH_Csharp_Library;

namespace LGD_OC_AstractPlatForm.OpticCompensation
{
    public interface IOCparamters
    {
        //ELVSS and Vinit2
        double Get_OC_Mode_ELVSS_Voltage(OC_Mode mode, int band);
        double[] Get_OC_Mode_ELVSS_Voltages(OC_Mode mode);
        void Set_OC_Mode_ELVSS_Voltage(double ELVSS_Voltage, OC_Mode mode, int band);
        double Get_OC_Mode_Vinit2_Voltage(OC_Mode mode, int band);
        double[] Get_OC_Mode_Vinit2_Voltages(OC_Mode mode);
        void Set_OC_Mode_Vinit2_Voltage(double Vinit2_Voltage, OC_Mode mode, int band);
        double Get_OC_Mode_Cold_ELVSS_Voltage(OC_Mode mode, int band);
        double[] Get_OC_Mode_Cold_ELVSS_Voltages(OC_Mode mode);
        void Set_OC_Mode_Cold_ELVSS_Voltage(double Cold_ELVSS_Voltage, OC_Mode mode, int band);
        double Get_OC_Mode_Cold_Vinit2_Voltage(OC_Mode mode, int band);
        double[] Get_OC_Mode_Cold_Vinit2_Voltages(OC_Mode mode);
        void Set_OC_Mode_Cold_Vinit2_Voltage(double Cold_Vinit2_Voltage, OC_Mode mode, int band);


        //Extension
        XYLv Get_OC_Mode_ExtensionXY(OC_Mode mode, int band, int gray);
        void Set_OC_Mode_ExtensionXY(OC_Mode mode, int band, int gray, XYLv xylv);
        bool Get_OC_Mode_IsExtensionApplied(OC_Mode mode, int band, int gray);
        void Set_OC_Mode_IsExtensionApplied(OC_Mode mode, int band, int gray, bool IsApplied);


        //GrayValue
        int Get_OC_Mode_Gray(OC_Mode mode, int bandindex, int grayindex);
        void Set_OC_Mode_Gray(OC_Mode mode, int bandindex, int grayindex, int GrayValue);


        //Limit
        XYLv Get_OC_Mode_Limit(OC_Mode mode, int band, int gray);
        void Set_OC_Mode_Limit(OC_Mode mode, int band, int gray, XYLv xylv);


        //LoopCount
        int Get_OC_Mode_LoopCount(OC_Mode mode, int band, int gray);
        void Set_OC_Mode_LoopCount(int loopcount, OC_Mode mode, int band, int gray);


        //Measure
        XYLv Get_OC_Mode_Measure(OC_Mode mode, int band, int gray);
        void Set_OC_Mode_Measure(XYLv measured, OC_Mode mode, int band, int gray);


        //Taret
        XYLv Get_OC_Mode_Target(OC_Mode mode, int band, int gray);
        void Set_OC_Mode_Target(OC_Mode mode, int band, int gray, XYLv xylv);


        //DBV
        int GetDBV(int band);


        //REF0REF4095
        byte Get_Normal_REF0();
        double Get_Normal_REF0_Voltage();
        byte Get_Normal_REF4095();
        double Get_Normal_REF4095_Voltage();
        void Set_Normal_REF0(byte _Normal_REF0);
        void Set_Normal_REF4095(byte _Normal_REF4095);


        //Vreg1
        int[] Get_OC_Mode_Vreg1(OC_Mode mode);
        int Get_OC_Mode_Vreg1(OC_Mode mode, int band);
        double Get_OC_Mode_Vreg1_Voltage(OC_Mode mode, int band);
        void Set_OC_Mode_Vreg1(int Vreg1, OC_Mode mode, int band);


        //AM1AM0
        RGB Get_OC_Mode_AM1(OC_Mode mode, int band);
        RGB_Double Get_OC_Mode_AM1_Voltage(OC_Mode mode, int band);
        void Set_OC_Mode_AM1(RGB AM1, OC_Mode mode, int band);
        void Set_OC_Mode_AM1(RGB_Double AM1_Voltage, OC_Mode mode, int band);
        RGB Get_OC_Mode_AM0(OC_Mode mode, int band);
        RGB_Double Get_OC_Mode_AM0_Voltage(OC_Mode mode, int band);
        void Set_OC_Mode_AM0(RGB AM0, OC_Mode mode, int band);
        void Set_OC_Mode_AM0(RGB_Double AM0_Voltage, OC_Mode mode, int band);


        //RGB
        RGB Get_OC_Mode_RGB(OC_Mode mode, int band, int gray);
        RGB_Double Get_OC_Mode_RGB_Voltage(OC_Mode mode, int band, int gray);
        RGB[] Get_OC_Mode_RGB(OC_Mode mode, int band);
        void Set_OC_Mode_RGB(RGB Gamma, OC_Mode mode, int band, int gray);
        void Set_OC_Mode_RGB(RGB[] Gamma, OC_Mode mode, int band);
    }
}
