
using BSQH_Csharp_Library;
using LGD_OC_AstractPlatForm.CommonAPI;
using LGD_OC_AstractPlatForm.OpticCompensation.DP213.Data;
using LGD_OC_AstractPlatForm.OpticCompensation.DP213.SingleBandCompensation;
using System;
using System.Drawing;
using System.Threading;

namespace LGD_OC_AstractPlatForm.OpticCompensation.DP213.AODCompensation
{
    public class DP213_AODCompensation : DP213_SingleBandCompensation, ICompensation
    {
        IBusinessAPI api;
        DP213CMD cmd;
        OCVars vars;
        public DP213_AODCompensation(IBusinessAPI _api, IOCparamters _ocparam, int _channel_num, OCVars _vars)
            : base(_api, _ocparam, _channel_num, _vars)
        {
            api = _api;
            vars = _vars;
            cmd = new DP213CMD(api, _channel_num);
        }

        public void Compensation()
        {
            if (vars.Optic_Compensation_Stop == false && DP213OCSet.Get_IsAODCompensationApply() == true)
            {
                api.WriteLine("DP213 AOD Compensation() Start", Color.Blue);
                AODCompensation();
                api.WriteLine("DP213 AOD Compensation() Finished", Color.Green);
            }
            else
            {
                api.WriteLine("DP213 AOD Compensation() Skip", Color.Red);
            }
        }


        private void AODCompensation()
        {
            cmd.AODOn();
            Thread.Sleep(100);

            for (int band = DP213_Static.Max_HBM_and_Normal_Band_Amount; band < DP213_Static.Max_Band_Amount; band++)
                base.SingleBand_RGB_Compensation(OC_Mode.Mode1, band);//AOD꺼는 Mode1쪽의 Param으로 처리

            cmd.AODOff();
            Thread.Sleep(100);
        }
    }
}
