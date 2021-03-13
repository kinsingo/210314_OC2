
using BSQH_Csharp_Library;
using LGD_OC_AstractPlatForm.CommonAPI;
using LGD_OC_AstractPlatForm.OpticCompensation.DP213.Data;
using LGD_OC_AstractPlatForm.OpticCompensation.DP213.SingleBandCompensation;
using System;
using System.Drawing;
using System.Threading;

namespace LGD_OC_AstractPlatForm.OpticCompensation.DP213.MainCompensation
{
    public class DP213_Mode456_Main_Compensation: DP213_SingleBandCompensation, ICompensation
    {
        IBusinessAPI api;
        IOCparamters ocparam;
        OCVars vars;
        DP213CMD cmd;
        public DP213_Mode456_Main_Compensation(IBusinessAPI _api, IOCparamters _ocparam, int _channel_num, OCVars _vars)
            : base(_api, _ocparam, _channel_num, _vars)
        {
            api = _api;
            ocparam = _ocparam;
            vars = _vars;
            cmd = new DP213CMD(api, _channel_num);
        }

        public void Compensation()
        {
            if (vars.Optic_Compensation_Stop == false && DP213OCSet.Get_IsMain456CompensationApply() == true)
            {
                api.WriteLine("DP213 Main456 Compensation() Start", Color.Blue);
                MainCompensation();
                api.WriteLine("DP213 Main456 Compensation() Finish", Color.Green);
            }
            else
            {
                api.WriteLine("DP213 Main456 Compensation() Skip", Color.Red);
            }
        }

        private void MainCompensation()
        {
            SingleModeOC(OC_Mode.Mode4);
            SingleModeOC(OC_Mode.Mode5);
            SingleModeOC(OC_Mode.Mode6);
        }

        private void SingleModeOC(OC_Mode mode)
        {
            cmd.SendGammaSetApplyCMD(DP213OCSet.GetGammaSet(mode));
            for (int band = 0; band < DP213_Static.Max_HBM_and_Normal_Band_Amount && vars.Optic_Compensation_Stop == false; band++)
            {
                if (band > DP213OCSet.Get_mode456_max_skip_band())
                    SingleBand_RGB_or_RVreg1B_Compensation(mode, band);
                else
                    ApplyGamma(mode, band);
            }
        }
    }
}
