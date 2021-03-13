using System;
using LGD_OC_AstractPlatForm.Enums;
using LGD_OC_AstractPlatForm.CommonAPI;
using BSQH_Csharp_Library;
using System.Drawing;

namespace LGD_OC_AstractPlatForm.OpticCompensation
{
    public class OCVars : OCInfiniteLoopCheck
    {
        public bool Within_Spec_Limit;
        public bool Gamma_Out_Of_Register_Limit;
        public bool Optic_Compensation_Succeed;
        public bool Optic_Compensation_Stop; //Only false when OC is started. this should not be initialized all the time

        
        public OCVars(IBusinessAPI _api)
            : base(_api)
        {
            InitializeForEveryOC();
            Optic_Compensation_Stop = false;
        }

        public void InitializeForEveryOC()
        {
            Within_Spec_Limit = false;
            Gamma_Out_Of_Register_Limit = false;
            Optic_Compensation_Succeed = false;
            base.Initialize();
        }
    }
}
