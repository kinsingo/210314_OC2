
using BSQH_Csharp_Library;
using LGD_OC_AstractPlatForm.CommonAPI;
using LGD_OC_AstractPlatForm.OpticCompensation.DP213.Data;
using LGD_OC_AstractPlatForm.OpticCompensation.DP213.SingleBandCompensation;
using System;
using System.Drawing;
using System.Threading;

namespace LGD_OC_AstractPlatForm.OpticCompensation.DP213.MainCompensation
{
    public class DP213_MainCompensation : ICompensation
    {
        DP213_Mode123_Main_Compensation main123OC;
        DP213_Mode456_Main_Compensation main456OC;

        public DP213_MainCompensation(IBusinessAPI _api, IOCparamters _ocparam, int _channel_num, OCVars _vars)
        {   
            main123OC = new DP213_Mode123_Main_Compensation(_api, _ocparam, _channel_num, _vars);
            main456OC = new DP213_Mode456_Main_Compensation(_api, _ocparam, _channel_num, _vars);
        }

        public void Compensation()
        {
            main123OC.Compensation();
            main456OC.Compensation();
        }
    }
}
