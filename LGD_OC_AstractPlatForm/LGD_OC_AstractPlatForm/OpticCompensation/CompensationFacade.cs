using System;
using LGD_OC_AstractPlatForm.CommonAPI;
using LGD_OC_AstractPlatForm.Enums;
using BSQH_Csharp_Library;
using LGD_OC_AstractPlatForm.OpticCompensation.DP213.Data;
using LGD_OC_AstractPlatForm.OpticCompensation.DP213;
using LGD_OC_AstractPlatForm.OpticCompensation.DP253;
using LGD_OC_AstractPlatForm.OpticCompensation.DP213.FlashMemory;
using LGD_OC_AstractPlatForm.OpticCompensation.DP253.FlashMemory;

namespace LGD_OC_AstractPlatForm.OpticCompensation
{
    public class CompensationFacade
    {
        IBusinessAPI api;
        IOCFactory factory;
        IOCparamters parameters;
        IFlashMemory flash;
        int channel_num;
        public CompensationFacade(ModelName model,IBusinessAPI _api, IOCparamters _parameters, int _channel_num)
        {
            api = _api;
            parameters = _parameters;
            channel_num = _channel_num;
            factory = GetFactory(model);
            flash = GetFlashObject(model);
        }

        private IOCFactory GetFactory(ModelName model)
        {
            if (model == ModelName.DP213)
                return new DP213OCFactory(api, parameters, channel_num, new OCVars(api));
            if (model == ModelName.DP253)
                return new DP253OCFactory(api, parameters, channel_num, new OCVars(api));

            return null;
        }

        private IFlashMemory GetFlashObject(ModelName model)
        {
            if (model == ModelName.DP213)
                return new DP213Flash(api, parameters, channel_num, new OCVars(api));
            if (model == ModelName.DP253)
                return new DP253Flash();

            return null;
        }

        public void OpticCompensation()
        {
            Sub_Module_Compensation(CompensationMode.ELVSS);
            Sub_Module_Compensation(CompensationMode.White);
            Sub_Module_Compensation(CompensationMode.Black);
            Sub_Module_Compensation(CompensationMode.GrayLowRef);
            Sub_Module_Compensation(CompensationMode.AOD);
            Sub_Module_Compensation(CompensationMode.Main);
            flash.FlashEraseAndWrite();
        }

        private ICompensation GetICompensation(CompensationMode comp)
        {
            if (comp == CompensationMode.ELVSS) return factory.GetELVSSCompensation();
            else if (comp == CompensationMode.AOD) return factory.GetAODCompensation();
            else if (comp == CompensationMode.Black) return factory.GetBlackCompensation();
            else if (comp == CompensationMode.White) return factory.GetWhiteCompensation();
            else if (comp == CompensationMode.GrayLowRef) return factory.GetGrayLowRefCompensation();
            else if (comp == CompensationMode.Main) return factory.GetMainCompensation();
            return null;
        }

        private void Sub_Module_Compensation(CompensationMode comp)
        {
            try { GetICompensation(comp).Compensation(); }
            catch (Exception ex) { api.WriteLine("[Sub_Module_Compensation] : " + ex.Message); }
        }
    }
}
