
using BSQH_Csharp_Library;
using LGD_OC_AstractPlatForm.CommonAPI;
using LGD_OC_AstractPlatForm.OpticCompensation.DP213.Data;
using LGD_OC_AstractPlatForm.OpticCompensation.DP213.SingleBandCompensation;
using System;
using System.Drawing;
using System.Threading;
using LGD_OC_AstractPlatForm.OpticCompensation.DP213.SingleGrayCompensation;

namespace LGD_OC_AstractPlatForm.OpticCompensation.DP213.MainCompensation
{
    public class DP213_Mode123_Main_Compensation : DP213_SingleBandCompensation, ICompensation
    {
        IBusinessAPI api;
        IOCparamters ocparam;
        OCVars vars;
        DP213CMD cmd;
        public DP213_Mode123_Main_Compensation(IBusinessAPI _api, IOCparamters _ocparam, int _channel_num, OCVars _vars)
            : base(_api, _ocparam, _channel_num, _vars)
        {
            api = _api;
            ocparam = _ocparam;
            vars = _vars;
            cmd = new DP213CMD(api, _channel_num);
        }
        public void Compensation()
        {
            if (vars.Optic_Compensation_Stop == false && DP213OCSet.Get_IsMain123CompensationApply() == true)
            {
                api.WriteLine("DP213 Main123 Compensation() Start", Color.Blue);
                MainCompensation();
                api.WriteLine("DP213 Main123 Compensation() Finish", Color.Green);
            }
            else
            {
                api.WriteLine("DP213 Main123 Compensation() Skip", Color.Red);
            }
        }


        private void MainCompensation()
        {
            for (int band = 0; band < DP213_Static.Max_HBM_and_Normal_Band_Amount && vars.Optic_Compensation_Stop == false; band++)
                    Mode123_Gray_OC(band);
        }


        private void Mode123_Gray_OC(int band)
        {
            cmd.DBV_Setting(ocparam.GetDBV(band).ToString("X3"));

            for (int gray = 0; gray < DP213_Static.Max_Gray_Amount && vars.Optic_Compensation_Stop == false; gray++)
            {
                if (IsNotSkipTarget(band, gray))
                    SubMode123Compensation(band, gray);
                else
                    Set_and_Send_Gamma_If_OC_Skip(band, gray);
            }

            Ifneeded_CopyAndSend_Mode123toMode456(band);
        }

        private void SubMode123Compensation(int band, int gray)
        {
            //Mode1
            cmd.SendGammaSetApplyCMD(DP213OCSet.GetGammaSet(OC_Mode.Mode1));
            if (band != 0 && gray == 0)
            {
                RVreg1BSubCompensation(OC_Mode.Mode1, band, gray);
                CopyAndSendVreg1_OCMode1_to_OCMode23(band, gray);
            }
            else
            {
                RGBSubCompensation(OC_Mode.Mode1, band, gray);
            }
            UpdateOCMode23Target(band, gray);
            UpdateOCMode23InitGamma(band, gray);
            

            //Mode2
            cmd.SendGammaSetApplyCMD(DP213OCSet.GetGammaSet(OC_Mode.Mode2));
            RGBSubCompensation(OC_Mode.Mode2, band, gray);

            //Mode3
            cmd.SendGammaSetApplyCMD(DP213OCSet.GetGammaSet(OC_Mode.Mode3));
            RGBSubCompensation(OC_Mode.Mode3, band, gray);
        }

        private void Ifneeded_CopyAndSend_Mode123toMode456(int band)
        {
            if (band <= DP213OCSet.Get_mode456_max_skip_band())
            {
                CopyAndSend_RGBVreg1ModetoMode(OC_Mode.Mode1, OC_Mode.Mode4, band);
                CopyAndSend_RGBVreg1ModetoMode(OC_Mode.Mode2, OC_Mode.Mode5, band);
                CopyAndSend_RGBVreg1ModetoMode(OC_Mode.Mode3, OC_Mode.Mode6, band);
            }
        }

        private void Set_and_Send_Gamma_If_OC_Skip(int band,int gray)
        {
            Set_and_Send_Gamma_If_OC_Skip(OC_Mode.Mode1, band, gray);
            Set_and_Send_Gamma_If_OC_Skip(OC_Mode.Mode2, band, gray);
            Set_and_Send_Gamma_If_OC_Skip(OC_Mode.Mode3, band, gray);
        }



        private void CopyAndSend_RGBVreg1ModetoMode(OC_Mode from, OC_Mode to, int band)
        {
            int vreg1 = ocparam.Get_OC_Mode_Vreg1(from, band);
            ocparam.Set_OC_Mode_Vreg1(vreg1, to, band);

            RGB[] rgb = ocparam.Get_OC_Mode_RGB(from, band);
            ocparam.Set_OC_Mode_RGB(rgb, to, band);

            ApplyVreg1(to);
            ApplyGamma(to, band);
        }


  


        private void UpdateOCMode23Target(int band,int gray)
        {
            XYLv OCMode1FinalMeas = ocparam.Get_OC_Mode_Measure(OC_Mode.Mode1, band, gray);
            ocparam.Set_OC_Mode_Target(OC_Mode.Mode2, band, gray, OCMode1FinalMeas);
            ocparam.Set_OC_Mode_Target(OC_Mode.Mode3, band, gray, OCMode1FinalMeas);
        }

        private void UpdateOCMode23InitGamma(int band, int gray)
        {
            RGB OCMode1FinalRGB = ocparam.Get_OC_Mode_RGB(OC_Mode.Mode1,band, gray);
            ocparam.Set_OC_Mode_RGB(OCMode1FinalRGB,OC_Mode.Mode2, band, gray);
            ocparam.Set_OC_Mode_RGB(OCMode1FinalRGB,OC_Mode.Mode3, band, gray);
        }

        private void CopyAndSendVreg1_OCMode1_to_OCMode23(int band,int gray)
        {
            if (gray == 0)
            {
                int OCMode1FinalVreg1 = ocparam.Get_OC_Mode_Vreg1(OC_Mode.Mode1,band);
                ocparam.Set_OC_Mode_Vreg1(OCMode1FinalVreg1, OC_Mode.Mode2, band);
                ocparam.Set_OC_Mode_Vreg1(OCMode1FinalVreg1, OC_Mode.Mode3, band);

                ApplyVreg1(OC_Mode.Mode2);
                ApplyVreg1(OC_Mode.Mode3);
            }
        }
    }
}
