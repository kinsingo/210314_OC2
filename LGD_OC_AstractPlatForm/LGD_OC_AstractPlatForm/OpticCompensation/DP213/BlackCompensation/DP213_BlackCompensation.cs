
using BSQH_Csharp_Library;
using LGD_OC_AstractPlatForm.CommonAPI;
using LGD_OC_AstractPlatForm.OpticCompensation;
using System;
using System.Threading;
using System.Drawing;


namespace LGD_OC_AstractPlatForm.OpticCompensation.DP213.BlackCompensation
{
    public class DP213_BlackCompensation : ICompensation
    {
        IBusinessAPI api;
        IOCparamters ocparam;
        OCVars vars;
        DP213CMD cmd;
        int channel_num;
        public DP213_BlackCompensation(IBusinessAPI _api, IOCparamters _ocparam, int _channel_num, OCVars _vars)
        {
            api = _api;
            ocparam = _ocparam;
            channel_num = _channel_num;
            vars = _vars;
            cmd = new DP213CMD(api, _channel_num);
        }

        public void Compensation()
        {
            if (vars.Optic_Compensation_Stop == false && DP213OCSet.Get_IsBlackCompensationApply() == true)
            {
                api.WriteLine("Black Compensation() Start", Color.Blue);
                BlackCompensation();
                api.WriteLine("Black Compensation() Finish", Color.Green);
            }
            else
            {
                api.WriteLine("Black Compensation() Skip", Color.Red);
            }
        }

        private void BlackCompensation()
        {
            if (Sub_Black_Compensation() == false)
                vars.Optic_Compensation_Stop = true;
        }

        private void Black_OC_Initalize(int band)
        {
            cmd.DBV_Setting(ocparam.GetDBV(band).ToString("X3"));
            api.DisplayMonoPattern(new byte[] { 0, 0, 0 }, channel_num);
            Set_and_Send_AM0(DP213OCSet.Get_BlackCompensation_OCMode(), band, new RGB(0));
            Thread.Sleep(300);
        }

        private void Set_and_Send_AM0(OC_Mode mode, int band, RGB New_AM0)
        {
            ocparam.Set_OC_Mode_AM0(New_AM0, mode, band);
            Send_Gamma_AM1_AM0(mode, band);
            api.WriteLine("[Set_and_Send_AM0]OCMode/band AM0_R/G/B : " + mode + "/" + band + " " + New_AM0.int_R + "/" + New_AM0.int_G + "/" + New_AM0.int_B);
        }

        private void Send_Gamma_AM1_AM0(OC_Mode mode, int band)
        {
            RGB[] RGBs = ocparam.Get_OC_Mode_RGB(mode, band);
            RGB AM0 = ocparam.Get_OC_Mode_AM0(mode, band);
            RGB AM1 = ocparam.Get_OC_Mode_AM1(mode, band);

            byte[][] output_cmds = ModelFactory.Get_DP213_Instance().Get_Gamma_AM1_AM0_CMD(DP213OCSet.GetGammaSet(mode), band, RGBs, AM1, AM0);
            cmd.SendMipiCMD(output_cmds);
            Thread.Sleep(20);
        }

        private bool Sub_Black_Compensation()
        {
            Black_OC_Initalize(band: 0);
            bool Black_OC_OK = REF4095_Compensation();
            if (Black_OC_OK) Black_OC_OK = AM0_Compensation();
            return Black_OC_OK;
        }


        public void Set_and_Send_VREF0_VREF4095(byte Dec_REF4095)
        {
            ocparam.Set_Normal_REF4095(Dec_REF4095);
            api.WriteLine("[Set and Send]REF4095 : " + ocparam.Get_Normal_REF4095());

            byte[][] Output_CMD = ModelFactory.Get_DP213_Instance().Get_REF4095_REF0_CMD(ocparam.Get_Normal_REF4095(), ocparam.Get_Normal_REF0());
            cmd.SendMipiCMD(Output_CMD);
        }

        private bool REF4095_Compensation()
        {
            if (vars.Optic_Compensation_Stop == false)
            {
                const double REF4095_Resolution = 0.04;
                double REF4095_Margin = DP213OCSet.Get_REF4095_Margin();
                double Black_Limit_Lv = DP213OCSet.Get_Black_Limit_Lv();
                byte REF4095 = (byte)DP213_Static.REF4095_REF0_Max;

                Set_and_Send_VREF0_VREF4095(REF4095);

                while (REF4095 > 0)
                {
                    if (REF4095 == 0)
                    {
                        REF4095 += Convert.ToByte(REF4095_Margin / REF4095_Resolution);
                        Set_and_Send_VREF0_VREF4095(REF4095);
                        api.WriteLine("Black(REF4095) Compensation OK (Case 1)");
                        return true;
                    }
                    else
                    {
                        double[] measured = api.measure_XYL(channel_num);
                        api.WriteLine("REF4095 / Lv : " + REF4095 + "/" + measured[2]);

                        if (measured[2] < Black_Limit_Lv)
                        {
                            REF4095--;
                            Set_and_Send_VREF0_VREF4095(REF4095);
                            continue;
                        }
                        else
                        {
                            REF4095 += Convert.ToByte(REF4095_Margin / REF4095_Resolution);
                            if (REF4095 > DP213_Static.REF4095_REF0_Max)
                            {
                                api.WriteLine("Black(REF4095) Compensation Fail (Black Margin Is Not Enough)");
                                return false;
                            }
                            else
                            {
                                Set_and_Send_VREF0_VREF4095(REF4095);
                                api.WriteLine("Black(REF4095) Compensation OK (Case 2)");
                                return true;
                            }
                        }
                    }
                }
                return true;
            }
            return false;
        }

        private bool AM0_Compensation()
        {
            RGB_Double AM0_Margin = DP213OCSet.Get_AM0_Margin();
            double AM0_Resolution = Imported_my_cpp_dll.Get_DP213_EA9155_AM0_Resolution(ocparam.Get_Normal_REF0(), ocparam.Get_Normal_REF4095());

            RGB New_AM0 = new RGB();
            New_AM0.int_R = Convert.ToInt32(AM0_Margin.double_R / AM0_Resolution);
            New_AM0.int_G = Convert.ToInt32(AM0_Margin.double_G / AM0_Resolution);
            New_AM0.int_B = Convert.ToInt32(AM0_Margin.double_B / AM0_Resolution);
            
            bool AM0_OC_Ok = (New_AM0.int_R <= DP213_Static.AM1_AM0_Max) && (New_AM0.int_G <= DP213_Static.AM1_AM0_Max) && (New_AM0.int_B <= DP213_Static.AM1_AM0_Max);
            api.WriteLine("DP213_Static.AM1_AM0_Max : " + DP213_Static.AM1_AM0_Max);
            api.WriteLine("New_AM0 R/G/B : " + New_AM0.int_R + "/" + New_AM0.int_G + "/" + New_AM0.int_B);

            if (AM0_OC_Ok)
                Set_All_AM0_WithSameValues(New_AM0);
            
            return AM0_OC_Ok;
        }


        private void Set_All_AM0_WithSameValues(RGB New_AM0)
        {
            api.WriteLine("Only Set AM0 and Not Send AM0...");
            for (int band = 0; band < DP213_Static.Max_Band_Amount; band++)
            {
                ocparam.Set_OC_Mode_AM0(New_AM0, OC_Mode.Mode1,band);
                ocparam.Set_OC_Mode_AM0(New_AM0, OC_Mode.Mode2, band);
                ocparam.Set_OC_Mode_AM0(New_AM0, OC_Mode.Mode3, band);
                ocparam.Set_OC_Mode_AM0(New_AM0, OC_Mode.Mode4, band);
                ocparam.Set_OC_Mode_AM0(New_AM0, OC_Mode.Mode5, band);
                ocparam.Set_OC_Mode_AM0(New_AM0, OC_Mode.Mode6, band);
            }   
        }
    }
}
