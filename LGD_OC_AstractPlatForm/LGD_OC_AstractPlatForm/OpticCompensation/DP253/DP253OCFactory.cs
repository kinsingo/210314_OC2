using System;
using LGD_OC_AstractPlatForm.Enums;
using LGD_OC_AstractPlatForm.CommonAPI;
using LGD_OC_AstractPlatForm.OpticCompensation.DP253.AODCompensation;
using LGD_OC_AstractPlatForm.OpticCompensation.DP253.BlackCompensation;
using LGD_OC_AstractPlatForm.OpticCompensation.DP253.ELVSSCompensation;
using LGD_OC_AstractPlatForm.OpticCompensation.DP253.GrayLowReferenceCompensation;
using LGD_OC_AstractPlatForm.OpticCompensation.DP253.MainCompensation;
using LGD_OC_AstractPlatForm.OpticCompensation.DP253.WhiteCompensation;
using BSQH_Csharp_Library;
using LGD_OC_AstractPlatForm.OpticCompensation.DP213.Data;

namespace LGD_OC_AstractPlatForm.OpticCompensation.DP253
{
    internal class DP253OCFactory : IOCFactory
    {
        IBusinessAPI api;
        IOCparamters ocparam;
        int channel_num;
        OCVars vars;

        public DP253OCFactory(IBusinessAPI _api, IOCparamters _ocparam, int _channel_num, OCVars _vars)
        {
            api = _api;
            ocparam = _ocparam;
            channel_num = _channel_num;
            vars = _vars;
        }

        public ICompensation GetBlackCompensation() => new DP253_BlackCompensation(api, ocparam, channel_num, vars);
        public ICompensation GetAODCompensation() => new DP253_AODCompensation(api, ocparam, channel_num, vars);
        public ICompensation GetELVSSCompensation() => new DP253_ELVSSCompensation(api, ocparam, channel_num, vars);
        public ICompensation GetGrayLowRefCompensation() => new DP253_GrayLowRefCompensation(api, ocparam, channel_num, vars);
        public ICompensation GetMainCompensation() => new DP253_MainCompensation(api, ocparam, channel_num, vars);
        public ICompensation GetWhiteCompensation() => new DP253_WhiteCompensation(api, ocparam, channel_num, vars);
    }
}
