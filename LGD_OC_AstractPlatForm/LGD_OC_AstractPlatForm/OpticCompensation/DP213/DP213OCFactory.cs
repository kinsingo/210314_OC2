using System;
using LGD_OC_AstractPlatForm.Enums;
using LGD_OC_AstractPlatForm.CommonAPI;
using LGD_OC_AstractPlatForm.OpticCompensation.DP213.AODCompensation;
using LGD_OC_AstractPlatForm.OpticCompensation.DP213.BlackCompensation;
using LGD_OC_AstractPlatForm.OpticCompensation.DP213.ELVSSCompensation;
using LGD_OC_AstractPlatForm.OpticCompensation.DP213.GrayLowReferenceCompensation;
using LGD_OC_AstractPlatForm.OpticCompensation.DP213.MainCompensation;
using LGD_OC_AstractPlatForm.OpticCompensation.DP213.WhiteCompensation;
using BSQH_Csharp_Library;
using LGD_OC_AstractPlatForm.OpticCompensation.DP213.Data;

namespace LGD_OC_AstractPlatForm.OpticCompensation.DP213
{
    public class DP213OCFactory : IOCFactory
    {
        IBusinessAPI api;
        IOCparamters ocparam;
        int channel_num;
        OCVars vars;

        public DP213OCFactory(IBusinessAPI _api, IOCparamters _ocparam, int _channel_num, OCVars _vars)
        {
            api = _api;
            ocparam = _ocparam;
            channel_num = _channel_num;
            vars = _vars;
        }

        public ICompensation GetBlackCompensation() => new DP213_BlackCompensation(api, ocparam, channel_num, vars);
        public ICompensation GetAODCompensation() => new DP213_AODCompensation(api, ocparam, channel_num, vars);
        public ICompensation GetELVSSCompensation() {return new DP213_ELVSSCompensation(api, ocparam, channel_num, vars); }
        public ICompensation GetGrayLowRefCompensation() => new DP213_GrayLowRefCompensation(api, ocparam, channel_num, vars);
        public ICompensation GetMainCompensation() => new DP213_MainCompensation(api, ocparam, channel_num, vars);
        public ICompensation GetWhiteCompensation() => new DP213_WhiteCompensation(api, ocparam, channel_num, vars);
    }
}
