
using BSQH_Csharp_Library;
using LGD_OC_AstractPlatForm.CommonAPI;
using LGD_OC_AstractPlatForm.OpticCompensation.DP213.Data;
using LGD_OC_AstractPlatForm.OpticCompensation.DP213.SingleBandCompensation;
using System;
using System.Drawing;
using System.Threading;


namespace LGD_OC_AstractPlatForm.OpticCompensation.DP213.GrayLowReferenceCompensation
{
    public class DP213_GrayLowRefCompensation : DP213_SingleBandCompensation, ICompensation
    {
        IBusinessAPI api;
        IOCparamters ocparam;
        OCVars vars;
        DP213CMD cmd;
        int channel_num;
        public DP213_GrayLowRefCompensation(IBusinessAPI _api, IOCparamters _ocparam, int _channel_num, OCVars _vars)
            :base(_api, _ocparam, _channel_num, _vars)
        {
            api = _api;
            ocparam = _ocparam;
            channel_num = _channel_num;
            vars = _vars;
            cmd = new DP213CMD(api, _channel_num);
        }
        
        public void Compensation()
        {
            if (vars.Optic_Compensation_Stop == false && DP213OCSet.Get_IsGrayLowRefCompensationApply() == true)
            {
                api.WriteLine("DP213 GrayLowRef Compensation() Start",Color.Blue);
                GrayLowRefCompensation();
                api.WriteLine("DP213 GrayLowRef Compensation() Finish", Color.Green);
            }
            else
            {
                api.WriteLine("DP213 GrayLowRef Compensation() Skip", Color.Red);
            }
        }
        
        private void GrayLowRefCompensation()
        {
            OC_Mode mode = DP213OCSet.Get_LowGrayRefCompensastion_OCMode();
            cmd.SendGammaSetApplyCMD(DP213OCSet.GetGammaSet(mode));
            base.SingleBand_RGB_Compensation(mode, band: 0);
            Sub_AM1_Compensation(mode, band: 0);
        }


        private void Sub_AM1_Compensation(OC_Mode mode,int band)
        {
            if (vars.Optic_Compensation_Stop == false)
            {
                RGB_Double HBM_GR1_Voltage = ocparam.Get_OC_Mode_RGB_Voltage(mode, band, gray: 10);
                RGB_Double AM1_Margin = DP213OCSet.Get_AM1_Margin();

                RGB_Double New_AM1_Voltage = Get_New_AM1_Voltage(HBM_GR1_Voltage, AM1_Margin);
                RGB_Double AM0_Voltage = ocparam.Get_OC_Mode_AM0_Voltage(mode, band);

                if (Is_All_AM1_Voltages_Lower_Than_AM0_Voltages(New_AM1_Voltage, AM0_Voltage))
                {
                    ocparam.Set_OC_Mode_AM1(New_AM1_Voltage, mode, band);
                    Set_All_AM1_WithSameValues(ocparam.Get_OC_Mode_AM1(mode, band));
                }
                else
                {
                    vars.Optic_Compensation_Stop = true;
                    vars.Optic_Compensation_Succeed = false;
                    api.WriteLine("(Out of Range)At lease One AM1 > AM0, AM1 Compensation NG", Color.Red);

                }
            }
        }

         void Set_All_AM1_WithSameValues(RGB AM1)
        {
            for (int band = 0; band < DP213_Static.Max_Band_Amount; band++)
            {
                ocparam.Set_OC_Mode_AM1(AM1, OC_Mode.Mode1, band);
                ocparam.Set_OC_Mode_AM1(AM1, OC_Mode.Mode2, band);
                ocparam.Set_OC_Mode_AM1(AM1, OC_Mode.Mode3, band);
                ocparam.Set_OC_Mode_AM1(AM1, OC_Mode.Mode4, band);
                ocparam.Set_OC_Mode_AM1(AM1, OC_Mode.Mode5, band);
                ocparam.Set_OC_Mode_AM1(AM1, OC_Mode.Mode6, band);
            }
        }




        private void Show_AM1_AM0_Voltages(RGB_Double New_AM1_Voltage, RGB_Double AM0_Voltage)
        {
            api.WriteLine("New_AM1_Voltage_R/G/B : " + New_AM1_Voltage.double_R.ToString() + "/" + New_AM1_Voltage.double_G.ToString() + "/" + New_AM1_Voltage.double_B.ToString(), Color.Blue);
            api.WriteLine("AM0_Voltage/G/B : " + AM0_Voltage.double_R.ToString() + "/" + AM0_Voltage.double_G.ToString() + "/" + AM0_Voltage.double_B.ToString(), Color.Blue);
        }

    

        private RGB_Double Get_New_AM1_Voltage(RGB_Double HBM_GR1_Voltage, RGB_Double AM1_Margin)
        {
            RGB_Double New_AM1_Voltage = new RGB_Double();

            New_AM1_Voltage.double_R = (HBM_GR1_Voltage.double_R + AM1_Margin.double_R);
            New_AM1_Voltage.double_G = (HBM_GR1_Voltage.double_G + AM1_Margin.double_G);
            New_AM1_Voltage.double_B = (HBM_GR1_Voltage.double_B + AM1_Margin.double_B);

            return New_AM1_Voltage;
        }

        bool Is_All_AM1_Voltages_Lower_Than_AM0_Voltages(RGB_Double AM1_Voltage, RGB_Double AM0_Voltage)
        {
            Show_AM1_AM0_Voltages(AM1_Voltage, AM0_Voltage);

            if ((AM1_Voltage.double_R < AM0_Voltage.double_R)
                && (AM1_Voltage.double_G < AM0_Voltage.double_G)
                && (AM1_Voltage.double_B < AM0_Voltage.double_B))
                return true;
            else
            {
                return false;
            }
        }
    }
}
