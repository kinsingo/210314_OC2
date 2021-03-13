
using LGD_OC_AstractPlatForm.CommonAPI;

namespace LGD_OC_AstractPlatForm.OpticCompensation.DP253.BlackCompensation
{
    internal class DP253_BlackCompensation : ICompensation
    {
        IBusinessAPI api;
        IOCparamters ocparam;
        OCVars vars;
        int channel_num;
        public DP253_BlackCompensation(IBusinessAPI _api, IOCparamters _ocparam, int _channel_num, OCVars _vars)
        {
            api = _api;
            ocparam = _ocparam;
            channel_num = _channel_num;
            vars = _vars;
        }

        public void Compensation()
        {
            api.WriteLine("DP253 Black Compensation()");
        }
    }
}
