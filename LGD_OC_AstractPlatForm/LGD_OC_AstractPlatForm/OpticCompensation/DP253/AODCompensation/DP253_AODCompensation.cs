
using LGD_OC_AstractPlatForm.CommonAPI;
using BSQH_Csharp_Library;

namespace LGD_OC_AstractPlatForm.OpticCompensation.DP253.AODCompensation
{
    internal class DP253_AODCompensation : ICompensation
    {
        IBusinessAPI api;
        IOCparamters ocparam;
        OCVars vars;
        int channel_num;
        public DP253_AODCompensation(IBusinessAPI _api, IOCparamters _ocparam, int _channel_num, OCVars _vars)
        {
            api = _api;
            ocparam = _ocparam;
            channel_num = _channel_num;
            vars = _vars;
        }

        public void Compensation()
        {
            api.WriteLine("DP253 AOD Compensation()");

        }
    }
}
