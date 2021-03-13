
using LGD_OC_AstractPlatForm.CommonAPI;

namespace LGD_OC_AstractPlatForm.OpticCompensation.DP253.WhiteCompensation
{
    internal class DP253_WhiteCompensation : ICompensation
    {
        IBusinessAPI api;
        IOCparamters ocparam;
        OCVars vars;
        int channel_num;
        public DP253_WhiteCompensation(IBusinessAPI _api, IOCparamters _ocparam, int _channel_num, OCVars _vars)
        {
            api = _api;
            ocparam = _ocparam;
            channel_num = _channel_num;
            vars = _vars;
        }

        public void Compensation()
        {
            api.WriteLine("DP253 White Compensation()");
        }
    }
}
