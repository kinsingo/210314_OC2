
using BSQH_Csharp_Library;
using LGD_OC_AstractPlatForm.CommonAPI;
using System;
using System.Drawing;
using System.Threading;

namespace LGD_OC_AstractPlatForm.OpticCompensation.DP213.SingleGrayCompensation
{
    public class DP213_SingleGrayCompensation
    {
        IBusinessAPI api;
        IOCparamters ocparam;
        int channel_num;
        OCVars vars;
        DP213CMD cmd;

        public DP213_SingleGrayCompensation(IBusinessAPI _API, IOCparamters _ocparam,int _channel_num, OCVars _vars)
        {
            api = _API;
            ocparam = _ocparam;
            channel_num = _channel_num;
            vars = _vars;
            cmd = new DP213CMD(api, channel_num);
            
        }

        protected void Band_Gray255_RGB_Compensation(OC_Mode mode,int band)
        {
            cmd.DBV_Setting(ocparam.GetDBV(band).ToString("X3"));
            RGBSubCompensation(mode, band, gray: 0);
        }

        private void DisplayPattern(int band,byte GrayVal)
        {
            if (band < DP213_Static.Max_HBM_and_Normal_Band_Amount)
                api.DisplayMonoPattern(new byte[3] { GrayVal, GrayVal, GrayVal }, channel_num);
            else
                api.DisplayBoxPattern(new byte[3] { GrayVal, GrayVal, GrayVal }, new byte[3] { 0, 0, 0 }, DP213OCSet.Get_AOD_LeftTopPos(), DP213OCSet.Get_AOD_RightBottomPos(), channel_num);
            
            api.WriteLine("G" + GrayVal + " is applied", Color.Blue);
        }



        protected void RGBSubCompensation(OC_Mode mode, int band,int gray)
        {
            InitializeForCompensation(mode, band, gray);
            Apply_RGB_And_Measure(mode, band, gray);

            while (vars.Optic_Compensation_Succeed == false && vars.Optic_Compensation_Stop == false)
            {
                RGB Gamma = ocparam.Get_OC_Mode_RGB(mode, band, gray);
                RGBOpticCompensation(ref Gamma, mode, band, gray);

                if (Is_OC_Infished(ocparam.Get_OC_Mode_LoopCount(mode, band, gray), DP213OCSet.Get_MaxLoopCount()))
                    break;

                Set_RGB_LoopCount(Gamma, mode, band, gray);
                Apply_RGB_And_Measure(mode, band, gray);
            }
        }

        private void Set_RGB_LoopCount(RGB Gamma,OC_Mode mode, int band, int gray)
        {
            ocparam.Set_OC_Mode_RGB(Gamma, mode, band, gray);
            ocparam.Set_OC_Mode_LoopCount((ocparam.Get_OC_Mode_LoopCount(mode, band, gray) + 1), mode, band, gray);
        }

        private void Apply_RGB_And_Measure(OC_Mode mode, int band, int gray)
        {
            ApplyGamma(mode, band);
            MeasureAndUpdate(mode, band, gray);
        }

        private void InitializeForCompensation(OC_Mode mode, int band, int gray)
        {
            byte GrayVal = Convert.ToByte(ocparam.Get_OC_Mode_Gray(mode, band, gray));
            DisplayPattern(band, GrayVal);
            Thread.Sleep(300); //Pattern 안정화 Time
            vars.InitializeForEveryOC();
            ocparam.Set_OC_Mode_LoopCount(loopcount: 0, mode, band, gray);
        }


        protected void RVreg1BSubCompensation(OC_Mode mode, int band, int gray)
        {
            InitializeForCompensation(mode, band, gray);
            Apply_Vreg1_RGB_And_Measure(mode, band, gray);

            while (vars.Optic_Compensation_Succeed == false && vars.Optic_Compensation_Stop == false)
            {
                RGB Gamma = ocparam.Get_OC_Mode_RGB(mode, band, gray);
                int vreg1 = ocparam.Get_OC_Mode_Vreg1(mode, band);
                RVreg1BOpticCompensation(ref Gamma,ref vreg1, mode, band, gray);

                if (Is_OC_Infished(ocparam.Get_OC_Mode_LoopCount(mode, band, gray), DP213OCSet.Get_MaxLoopCount()))
                    break;

                Set_Vreg1_RGB_LoopCount(vreg1, Gamma, mode, band, gray);
                Apply_Vreg1_RGB_And_Measure(mode, band, gray);
            }
        }


        private void Set_Vreg1_RGB_LoopCount(int vreg1,RGB Gamma, OC_Mode mode, int band, int gray)
        {
            ocparam.Set_OC_Mode_Vreg1(vreg1, mode, band);
            ocparam.Set_OC_Mode_RGB(Gamma, mode, band, gray);
            ocparam.Set_OC_Mode_LoopCount((ocparam.Get_OC_Mode_LoopCount(mode, band, gray) + 1), mode, band, gray);
        }

        private void Apply_Vreg1_RGB_And_Measure(OC_Mode mode, int band, int gray)
        {
            ApplyVreg1(mode);
            ApplyGamma(mode, band);
            MeasureAndUpdate(mode, band, gray);
        }



        private void RVreg1BOpticCompensation(ref RGB Gamma,ref int vreg1, OC_Mode mode, int band, int gray)
        {
            XYLv Measured = ocparam.Get_OC_Mode_Measure(mode, band, gray);
            XYLv Target = ocparam.Get_OC_Mode_Target(mode, band, gray);
            XYLv Limit = ocparam.Get_OC_Mode_Limit(mode, band, gray);
            XYLv ExtensionXY = ocparam.Get_OC_Mode_ExtensionXY(mode, band, gray);
            int loopCount = ocparam.Get_OC_Mode_LoopCount(mode, band, gray);
           
            Imported_my_cpp_dll.Vreg1_Compensation(loopCount,Vreg1_Infinite_Loop: false, Vreg1_Infinite_Loop_Count : 0, ref Gamma.int_R, ref vreg1, ref Gamma.int_B, Measured.double_X, Measured.double_Y, Measured.double_Lv,
                Target.double_X, Target.double_Y, Target.double_Lv, Limit.double_X, Limit.double_Y, Limit.double_Lv, ExtensionXY.double_X, ExtensionXY.double_Y, DP213_Static.Gamma_Register_Max, 
                DP213_Static.Vreg1_Register_Max,ref vars.Gamma_Out_Of_Register_Limit, ref vars.Within_Spec_Limit);
        }



        private void RGBOpticCompensation(ref RGB Gamma,OC_Mode mode, int band, int gray)
        {
            XYLv Measured = ocparam.Get_OC_Mode_Measure(mode, band, gray);
            XYLv Target = ocparam.Get_OC_Mode_Target(mode, band, gray);
            XYLv Limit = ocparam.Get_OC_Mode_Limit(mode, band, gray);
            XYLv ExtensionXY = ocparam.Get_OC_Mode_ExtensionXY(mode, band, gray);
            int loopCount = ocparam.Get_OC_Mode_LoopCount(mode, band, gray);
            
            vars.Check_InfiniteLoop(loopCount,Gamma);
            Imported_my_cpp_dll.Sub_Compensation(loopCount, vars.IsInfiniteLoop, ref vars.InfiniteLoopCount, ref Gamma.int_R, ref Gamma.int_G, ref Gamma.int_B, Measured.double_X, Measured.double_Y, Measured.double_Lv, Target.double_X, Target.double_Y, Target.double_Lv, Limit.double_X, Limit.double_Y, Limit.double_Lv, ExtensionXY.double_X, ExtensionXY.double_Y, DP213_Static.Gamma_Register_Max, ref vars.Gamma_Out_Of_Register_Limit, ref vars.Within_Spec_Limit);
        }

        protected void ApplyVreg1(OC_Mode mode)
        {
            byte[][] output_cmds = ModelFactory.Get_DP213_Instance().Get_Vreg1_CMD(DP213OCSet.GetGammaSet(mode), ocparam.Get_OC_Mode_Vreg1(mode));
            cmd.SendMipiCMD(output_cmds);
            Thread.Sleep(20);
        }

        protected void ApplyGamma(OC_Mode mode, int band)
        {
            RGB AM0 = ocparam.Get_OC_Mode_AM0(mode, band);
            RGB AM1 = ocparam.Get_OC_Mode_AM1(mode, band);
            byte[][] output_cmds = ModelFactory.Get_DP213_Instance().Get_Gamma_AM1_AM0_CMD(DP213OCSet.GetGammaSet(mode), band, ocparam.Get_OC_Mode_RGB(mode, band), AM1, AM0);
            cmd.SendMipiCMD(output_cmds);
            Thread.Sleep(20);
        }

        protected void MeasureAndUpdate(OC_Mode mode, int band,int gray)
        {
            double[] MeasuredXYLv = api.measure_XYL(channel_num);
            ocparam.Set_OC_Mode_Measure(new XYLv(MeasuredXYLv[0], MeasuredXYLv[1], MeasuredXYLv[2]), mode, band, gray);

            XYLv TargetXYLv = ocparam.Get_OC_Mode_Target(mode, band, gray);
            api.WriteLine($"Measured X / Y / Lv : {MeasuredXYLv[0]} / {MeasuredXYLv[1]} / {MeasuredXYLv[2]}");
            api.WriteLine($"Target X / Y / Lv : {TargetXYLv.double_X} / {TargetXYLv.double_Y} / {TargetXYLv.double_Lv}");
        }


        protected bool Is_OC_Infished(int loopCount, int max_loop_count)
        {
            if (vars.Within_Spec_Limit)
            {
                vars.Optic_Compensation_Succeed = true;
                return true;
            }

            if (vars.Gamma_Out_Of_Register_Limit)
            {
                System.Windows.Forms.MessageBox.Show("Gamma or Vreg1 is out of Limit");
                vars.Optic_Compensation_Stop = true;
                return true;
            }

            if (loopCount == max_loop_count)
            {
                System.Windows.Forms.MessageBox.Show("Loop Count Over");
                vars.Optic_Compensation_Stop = true;
                return true;
            }
            return false;
        }
    }
}
