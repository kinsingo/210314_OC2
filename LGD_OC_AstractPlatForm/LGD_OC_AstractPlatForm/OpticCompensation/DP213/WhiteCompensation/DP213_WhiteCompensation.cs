
using LGD_OC_AstractPlatForm.CommonAPI;
using LGD_OC_AstractPlatForm.OpticCompensation.DP213.SingleGrayCompensation;
using LGD_OC_AstractPlatForm.OpticCompensation.DP213.Data;
using BSQH_Csharp_Library;
using System;
using System.Drawing;

namespace LGD_OC_AstractPlatForm.OpticCompensation.DP213.WhiteCompensation
{
    public class DP213_WhiteCompensation : DP213_SingleGrayCompensation, ICompensation
    {
        IBusinessAPI api;
        IOCparamters ocparam;
        OCVars vars;
        DP213CMD cmd;
        public DP213_WhiteCompensation(IBusinessAPI _api, IOCparamters _ocparam, int _channel_num, OCVars _vars)
            :base(_api, _ocparam, _channel_num, _vars)
        {
            api = _api;
            ocparam = _ocparam;
            vars = _vars;
            cmd = new DP213CMD(api, _channel_num);
        }

        public void Compensation()
        {
            if (vars.Optic_Compensation_Stop == false)
            {
                api.WriteLine("DP213 White(REF0) Compensation() Start",Color.Blue);
                WhiteCompensation();
                api.WriteLine("DP213 White(REF0) Compensation() Finish", Color.Green);
            }
            else
            {
                api.WriteLine("DP213 White(REF0) Compensation() Skip", Color.Red);
            }
        }

        private void WhiteCompensation()
        {
            OC_Mode mode = DP213OCSet.Get_WhiteCompensation_OCMode();
            cmd.SendGammaSetApplyCMD(DP213OCSet.GetGammaSet(mode));
            Band_Gray255_RGB_Compensation(mode, band: 0);
            Sub_VREF0_Compensation(Get_HBM_RGB_Min_AM2_Voltage());
        }

        private double Get_HBM_RGB_Min_AM2_Voltage()
        {
            RGB AM2 = ocparam.Get_OC_Mode_RGB(DP213OCSet.Get_WhiteCompensation_OCMode(), band: 0, gray: 0);
            int Vreg1 = ocparam.Get_OC_Mode_Vreg1(DP213OCSet.Get_WhiteCompensation_OCMode(), band: 0);
            int REF0 = ocparam.Get_Normal_REF0();
            int REF4095 = ocparam.Get_Normal_REF4095() ;

            double REF0_Voltage = Imported_my_cpp_dll.DP213_VREF0_Dec_to_Voltage(REF0);
            double REF4095_Voltage = Imported_my_cpp_dll.DP213_VREF4095_Dec_to_Voltage(REF4095);
            double Vreg1_Voltage = Imported_my_cpp_dll.DP213_Get_Vreg1_Voltage(REF4095_Voltage, REF0_Voltage, Vreg1);
            
            RGB_Double AM2_Voltage = new RGB_Double();
            AM2_Voltage.double_R = Imported_my_cpp_dll.DP213_Get_AM2_Gamma_Voltage(REF4095_Voltage, Vreg1_Voltage, AM2.int_R);
            AM2_Voltage.double_G = Imported_my_cpp_dll.DP213_Get_AM2_Gamma_Voltage(REF4095_Voltage, Vreg1_Voltage, AM2.int_G);
            AM2_Voltage.double_B = Imported_my_cpp_dll.DP213_Get_AM2_Gamma_Voltage(REF4095_Voltage, Vreg1_Voltage, AM2.int_B);
            api.WriteLine("AM2_Voltage R/G/B : " + AM2_Voltage.double_R + "/" + AM2_Voltage.double_G + "/" + AM2_Voltage.double_B);
            api.WriteLine("Old VREF0 Voltage : " + REF0_Voltage);

            return Get_Min_RGB_Voltage(AM2_Voltage);
        }

        private double Get_Min_RGB_Voltage(RGB_Double HBM_RGB_AM2_Voltage)
        {
            double HBM_RGB_Min_AM2_Voltage = HBM_RGB_AM2_Voltage.double_R;
            if (HBM_RGB_Min_AM2_Voltage > HBM_RGB_AM2_Voltage.double_G) HBM_RGB_Min_AM2_Voltage = HBM_RGB_AM2_Voltage.double_G;
            if (HBM_RGB_Min_AM2_Voltage > HBM_RGB_AM2_Voltage.double_B) HBM_RGB_Min_AM2_Voltage = HBM_RGB_AM2_Voltage.double_B;

            return HBM_RGB_Min_AM2_Voltage;
        }

        private void Sub_VREF0_Compensation(double HBM_RGB_Min_AM2_Voltage)
        {
            if(vars.Optic_Compensation_Stop == false)
            {
                double VREF0_Margin = DP213OCSet.Get_VREF0_Margin();
                double New_VREF0_Voltage = (HBM_RGB_Min_AM2_Voltage - VREF0_Margin);
                api.WriteLine("New VREF0 Voltage : " + New_VREF0_Voltage);

                if (Is_New_VREF0_Overflow(New_VREF0_Voltage))
                    vars.Optic_Compensation_Stop = true;
                else
                    Set_and_Send_VREF0(New_VREF0_Voltage);
            }
        }

        public void Set_and_Send_VREF0(double New_VREF0_Voltage)
        {
            ocparam.Set_Normal_REF0(Convert.ToByte(Imported_my_cpp_dll.DP213_VREF0_Voltage_to_Dec(New_VREF0_Voltage)));

            api.WriteLine("New VREF0 :" + ocparam.Get_Normal_REF0());

            byte[][] Output_CMD = ModelFactory.Get_DP213_Instance().Get_REF4095_REF0_CMD(ocparam.Get_Normal_REF4095(), ocparam.Get_Normal_REF0());
            cmd.SendMipiCMD(Output_CMD);
        }



        private bool Is_New_VREF0_Overflow(double New_VREF0_Voltage)
        {
            if (New_VREF0_Voltage > 5.25)
            {
                api.WriteLine("VREF0 Upper Overflow NG(>5.25)");
                return true;
            }
            else if (New_VREF0_Voltage < 0.25)
            {
                api.WriteLine("VREF0 Lower Overflow NG(<0.25)");
                return true;
            }
            return false;

        }
    }
}
