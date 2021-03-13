
using LGD_OC_AstractPlatForm.CommonAPI;
using LGD_OC_AstractPlatForm.OpticCompensation.DP213.Data;
using LGD_OC_AstractPlatForm.Enums;
using BSQH_Csharp_Library;
using System;
using System.Threading;
using System.Collections.Generic;
using LGD_OC_AstractPlatForm.OpticCompensation.DP213.SingleGrayCompensation;

namespace LGD_OC_AstractPlatForm.OpticCompensation.DP213.SingleBandCompensation
{
    public class DP213_SingleBandCompensation : DP213_SingleGrayCompensation
    {   
        IBusinessAPI api;
        IOCparamters ocparam;
        int channel_num;
        OCVars vars;
        DP213CMD cmd;

        public DP213_SingleBandCompensation(IBusinessAPI _api, IOCparamters _ocparam,int _channel_num, OCVars _vars)
            :base(_api, _ocparam, _channel_num, _vars)
        {
            api = _api;
            ocparam = _ocparam;
            channel_num = _channel_num;
            vars = _vars;
            cmd = new DP213CMD(api, channel_num);
            
        }

        protected bool IsNotSkipTarget(int band, int gray)
        {
            double Target_LV = ocparam.Get_OC_Mode_Target(OC_Mode.Mode1, band, gray).double_Lv;
            return (Target_LV >= DP213OCSet.Get_SkipTargetLv());
        }

        protected void SingleBand_RGB_Compensation(OC_Mode mode, int band)
        {
            cmd.DBV_Setting(ocparam.GetDBV(band).ToString("X3"));
            for (int gray = 0; gray < DP213_Static.Max_Gray_Amount && vars.Optic_Compensation_Stop == false; gray++)
            {
                if (IsNotSkipTarget(band, gray))
                    RGBSubCompensation(mode, band, gray);
                else
                    Set_and_Send_Gamma_If_OC_Skip(mode, band, gray);
            }
        }


        protected void SingleBand_RGB_or_RVreg1B_Compensation(OC_Mode mode, int band)
        {
            cmd.DBV_Setting(ocparam.GetDBV(band).ToString("X3"));
            for (int gray = 0; gray < DP213_Static.Max_Gray_Amount && vars.Optic_Compensation_Stop == false; gray++)
            {
                if (IsNotSkipTarget(band, gray))
                {
                    if (band != 0 && gray == 0) RVreg1BSubCompensation(mode, band, gray);
                    else RGBSubCompensation(mode, band, gray);
                }
                else
                {
                    Set_and_Send_Gamma_If_OC_Skip(mode, band, gray);
                }
            }
        }

        protected void Set_and_Send_Gamma_If_OC_Skip(OC_Mode mode,int band, int gray)
        {
            if (cmd.IsBand0orAOD0(band) == false)
            {
                RGB PrevGamma = ocparam.Get_OC_Mode_RGB(mode, band - 1, gray);
                ocparam.Set_OC_Mode_RGB(PrevGamma, mode, band, gray);
            }
            ApplyGamma(mode, band);
        }


    }
}
