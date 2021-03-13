
using LGD_OC_AstractPlatForm.CommonAPI;

namespace LGD_OC_AstractPlatForm.OpticCompensation
{
    public interface IOCFactory
    {
        ICompensation GetAODCompensation();
        ICompensation GetELVSSCompensation();
        ICompensation GetBlackCompensation();
        ICompensation GetWhiteCompensation();
        ICompensation GetGrayLowRefCompensation();
        ICompensation GetMainCompensation();

    }
}
