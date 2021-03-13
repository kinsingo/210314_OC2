
using LGD_OC_AstractPlatForm.CommonAPI;
using LGD_OC_AstractPlatForm.OpticCompensation.DP213.Data;
using BSQH_Csharp_Library;
using System;
using System.Threading;
using System.Collections.Generic;
using LGD_OC_AstractPlatForm.OpticCompensation.DP213.SingleGrayCompensation;
using System.Drawing;

namespace LGD_OC_AstractPlatForm.OpticCompensation.DP213.ELVSSCompensation
{
    public class DP213_ELVSSCompensation : DP213_SingleGrayCompensation,ICompensation
    {
        IBusinessAPI api;
        IOCparamters ocparam;
        int channel_num;
        OCVars vars;
        DP213CMD cmd;
        ELVSS_Compensation elvss_oc_obj;
        public DP213_ELVSSCompensation(IBusinessAPI _API, IOCparamters _ocparam, int _channel_num, OCVars _vars)
            : base(_API, _ocparam, _channel_num, _vars)
        {
            api = _API;
            ocparam = _ocparam;
            channel_num = _channel_num;
            vars = _vars;
            cmd = new DP213CMD(api, channel_num);
            elvss_oc_obj = new ELVSS_Compensation();
        }

        public void Compensation()
        {
            if (vars.Optic_Compensation_Stop == false && DP213OCSet.Get_IsELVSSCompensationApply() == true)
            {
                api.WriteLine("DP213 ELVSS Compensation() Start",Color.Blue);
                ELVSSCompensation();
                api.WriteLine("DP213 ELVSS Compensation() Finish", Color.Green);
            }
            else
            {
                api.WriteLine("DP213 ELVSS Compensation() Skip", Color.Red);
            }
        }

        private void ELVSSCompensation()
        {
            OC_Mode mode = DP213OCSet.Get_ELVSS_OCMode();
            cmd.SendGammaSetApplyCMD(DP213OCSet.GetGammaSet(mode));
            Band_Gray255_RGB_Compensation(mode, band: 0);
            Sub_ELVSS_and_Vinit2_Compensation(mode, band: 0, DP213OCSet.Get_ELVSS_Voltage_Max(), DP213OCSet.Get_ELVSS_Voltage_Min());

        }

        private void Sub_ELVSS_and_Vinit2_Compensation(OC_Mode mode, int band, double ELVSS_Voltage_Max, double ELVSS_Voltage_Min)
        {
            if (vars.Optic_Compensation_Stop == false)
            {
                double[] First_Five_Lv = new double[5];
                double Found_ELVSS_Voltage = Double.MinValue;

                List<double> ELVSS_list = new List<double>();
                List<double> Lv_list = new List<double>();

                int index = 0;
                for (double elvss_voltage = elvss_oc_obj.Get_First_ELVSS_Voltage(); elvss_voltage < elvss_oc_obj.Get_Last_ELVSS_Voltage(); elvss_voltage += elvss_oc_obj.ELVSS_Minimum_Step())
                {
                    Set_and_Send_ELVSS_CMD(mode, band, Imported_my_cpp_dll.DP213_ELVSS_Voltage_to_Dec(elvss_voltage));
                    Thread.Sleep(20);
                    ELVSS_list.Add(elvss_voltage);

                    XYLv[] MultiMeasured = Get_Multitimes_Measured_Values(how_many_times: 5);
                    double final_lv = elvss_oc_obj.Get_Average_XYLv_After_Remove_Min_Max(MultiMeasured).double_Lv;
                    Lv_list.Add(final_lv);
                    api.WriteLine(index + ")elvss_voltage :" + elvss_voltage);


                    if (index < 5)
                    {
                        First_Five_Lv[index] = final_lv;
                        api.WriteLine(index + ") First_Five_Lv[index] :" + First_Five_Lv[index]);
                    }
                    else if (Math.Round(elvss_voltage - (elvss_oc_obj.Get_ELVSSArrayLength() - 2) * 0.1, 1) >= ELVSS_Voltage_Min)
                    {
                        api.WriteLine("elvss_voltage / ELVSS_Voltage_Min : " + elvss_voltage + " / " + ELVSS_Voltage_Min);
                        api.WriteLine(Math.Round((elvss_voltage - (elvss_oc_obj.Get_ELVSSArrayLength() - 2) * 0.1), 1) + " >= " + ELVSS_Voltage_Min);
                        double[] ELVSS;
                        double[] Lv;
                        Update_ELVSS_LV_Array(out ELVSS, out Lv, ELVSS_list, Lv_list);

                        for (int k = 0; k < ELVSS.Length; k++)
                            api.WriteLine(ELVSS[k] + " / " + Lv[k]);

                        Found_ELVSS_Voltage = elvss_oc_obj.FindELVSS(First_Five_Lv, ELVSS, Lv);
                        api.WriteLine("Found_ELVSS_Voltage / ELVSS_Voltage_Max :  " + Found_ELVSS_Voltage + " / " + ELVSS_Voltage_Max);
                        api.WriteLine("elvss_compensation_obj.Is_ELVSS_Found() : " + elvss_oc_obj.Is_ELVSS_Found());

                        if (elvss_oc_obj.Is_ELVSS_Found() || Found_ELVSS_Voltage >= ELVSS_Voltage_Max)
                            break;
                    }
                    index++;
                }
                Set_and_Send_ELVSS_and_Vinit2(Found_ELVSS_Voltage);
                Set_and_Send_Cold_ELVSS_and_Vinit2();
            }
        }

        void Set_and_Send_ELVSS_CMD(OC_Mode SelectedMode, int band, double ELVSS_Voltages)
        {
            ocparam.Set_OC_Mode_ELVSS_Voltage(ELVSS_Voltages, SelectedMode, band);
            byte[][] output_cmd = ModelFactory.Get_DP213_Instance().Get_ELVSS_CMD(DP213OCSet.GetGammaSet(SelectedMode), ocparam.Get_OC_Mode_Cold_ELVSS_Voltages(SelectedMode));
            cmd.SendMipiCMD(output_cmd);
            api.WriteLine("(Set and Send and update_textboxes) set/band ELVSS_Voltages : (" + DP213OCSet.GetGammaSet(SelectedMode).ToString() + "/" + band.ToString() + ")" + ELVSS_Voltages.ToString());
        }

        private void Set_and_Send_Cold_ELVSS_and_Vinit2()
        {
            Set_Cold_ELVSS_By_Adding_Offset(OC_Mode.Mode1);
            Set_Cold_ELVSS_By_Adding_Offset(OC_Mode.Mode2);
            Set_Cold_ELVSS_By_Adding_Offset(OC_Mode.Mode3);
            Set_Cold_ELVSS_By_Adding_Offset(OC_Mode.Mode4);
            Set_Cold_ELVSS_By_Adding_Offset(OC_Mode.Mode5);
            Set_Cold_ELVSS_By_Adding_Offset(OC_Mode.Mode6);

            Set_Cold_Vinit2_By_Adding_Offset(OC_Mode.Mode1);
            Set_Cold_Vinit2_By_Adding_Offset(OC_Mode.Mode2);
            Set_Cold_Vinit2_By_Adding_Offset(OC_Mode.Mode3);
            Set_Cold_Vinit2_By_Adding_Offset(OC_Mode.Mode4);
            Set_Cold_Vinit2_By_Adding_Offset(OC_Mode.Mode5);
            Set_Cold_Vinit2_By_Adding_Offset(OC_Mode.Mode6);
        }

        private void Set_Cold_Vinit2_By_Adding_Offset(OC_Mode SelectedMode)
        {
            for (int band = 0; band < DP213_Static.Max_HBM_and_Normal_Band_Amount; band++)
            {
                double Cold_Vinit2 = (ocparam.Get_OC_Mode_Vinit2_Voltage(SelectedMode, band) - 0.5);
                ocparam.Set_OC_Mode_Cold_Vinit2_Voltage(Cold_Vinit2, SelectedMode, band);
            }
            Send_Cold_Vinit2_Voltages(SelectedMode);
        }

        private void Send_Cold_Vinit2_Voltages(OC_Mode SelectedMode)
        {
            double[] Cold_Vinit2_Voltages = ocparam.Get_OC_Mode_Cold_Vinit2_Voltages(SelectedMode);
            byte[][] output_cmd = ModelFactory.Get_DP213_Instance().Get_Cold_Vinit2_CMD(DP213OCSet.GetGammaSet(SelectedMode), Cold_Vinit2_Voltages);
            cmd.SendMipiCMD(output_cmd);
            api.WriteLine("Send_Cold_Vinit2_Voltages, OC_Mode : " + SelectedMode.ToString());
        }


        private void Set_Cold_ELVSS_By_Adding_Offset(OC_Mode SelectedMode)
        {
            for (int band = 0; band < DP213_Static.Max_HBM_and_Normal_Band_Amount; band++)
            {
                double Cold_ELVSS_Voltage = (ocparam.Get_OC_Mode_ELVSS_Voltage(SelectedMode, band) - 0.5);
                ocparam.Set_OC_Mode_Cold_ELVSS_Voltage(Cold_ELVSS_Voltage, SelectedMode, band);
            }
            Send_Cold_ELVSS_Voltages(SelectedMode);
        }

        private void Send_Cold_ELVSS_Voltages(OC_Mode SelectedMode)
        {
            double[] Cold_ELVSS_Voltages = ocparam.Get_OC_Mode_Cold_ELVSS_Voltages(SelectedMode);
            byte[][] output_cmd = ModelFactory.Get_DP213_Instance().Get_Cold_ELVSS_CMD(DP213OCSet.GetGammaSet(SelectedMode), Cold_ELVSS_Voltages);
            cmd.SendMipiCMD(output_cmd);
            api.WriteLine("Send_Cold_ELVSS_Voltages, OC_Mode : " + SelectedMode.ToString());
        }


        private void Set_and_Send_ELVSS_and_Vinit2(double Found_ELVSS_Voltage)
        {
            //Set and send ELVSS 
            Set_ELVSS_By_Adding_Offset(OC_Mode.Mode1, Found_ELVSS_Voltage);
            Set_ELVSS_By_Adding_Offset(OC_Mode.Mode2, Found_ELVSS_Voltage);
            Set_ELVSS_By_Adding_Offset(OC_Mode.Mode3, Found_ELVSS_Voltage);
            Set_ELVSS_By_Adding_Offset(OC_Mode.Mode4, Found_ELVSS_Voltage);
            Set_ELVSS_By_Adding_Offset(OC_Mode.Mode5, Found_ELVSS_Voltage);
            Set_ELVSS_By_Adding_Offset(OC_Mode.Mode6, Found_ELVSS_Voltage);

            //Set and send Vinit2
            Set_Vinit2_By_Adding_Offset(OC_Mode.Mode1, Found_ELVSS_Voltage);
            Set_Vinit2_By_Adding_Offset(OC_Mode.Mode2, Found_ELVSS_Voltage);
            Set_Vinit2_By_Adding_Offset(OC_Mode.Mode3, Found_ELVSS_Voltage);
            Set_Vinit2_By_Adding_Offset(OC_Mode.Mode4, Found_ELVSS_Voltage);
            Set_Vinit2_By_Adding_Offset(OC_Mode.Mode5, Found_ELVSS_Voltage);
            Set_Vinit2_By_Adding_Offset(OC_Mode.Mode6, Found_ELVSS_Voltage);
        }

        private void Set_ELVSS_By_Adding_Offset(OC_Mode SelectedMode, double Found_ELVSS_Voltage)
        {
            Gamma_Set Set = DP213OCSet.GetGammaSet(SelectedMode);
            int Set_Index = Convert.ToInt16(Set);

            for (int band = 0; band < DP213_Static.Max_HBM_and_Normal_Band_Amount; band++)
            {
                double ELVSS_Offset = DP213OCSet.Get_ELVSS_Offset(SelectedMode, band);
                double ELVSS_Voltage = Found_ELVSS_Voltage + ELVSS_Offset;
                ocparam.Set_OC_Mode_ELVSS_Voltage(ELVSS_Voltage, SelectedMode, band);
            }

            Send_ELVSS_Voltages(SelectedMode);
        }

        private void Send_ELVSS_Voltages(OC_Mode SelectedMode)
        {
            double[] ELVSS_Voltages = ocparam.Get_OC_Mode_ELVSS_Voltages(SelectedMode);
            byte[][] output_cmd = ModelFactory.Get_DP213_Instance().Get_ELVSS_CMD(DP213OCSet.GetGammaSet(SelectedMode), ELVSS_Voltages);
            cmd.SendMipiCMD(output_cmd);
            api.WriteLine("Send_ELVSS_Voltages, OC_Mode : " + SelectedMode.ToString());
        }

        private void Set_Vinit2_By_Adding_Offset(OC_Mode SelectedMode, double Found_ELVSS_Voltage)
        {
            Gamma_Set Set = DP213OCSet.GetGammaSet(SelectedMode);
            int Set_Index = Convert.ToInt16(Set);

            for (int band = 0; band < DP213_Static.Max_HBM_and_Normal_Band_Amount; band++)
            {
                double ELVSS_Offset = DP213OCSet.Get_ELVSS_Offset(SelectedMode, band);
                double Vinit2_Offset = DP213OCSet.Get_Vinit2_Offset(SelectedMode, band);
                double Vinit2_Voltage = Found_ELVSS_Voltage + ELVSS_Offset + Vinit2_Offset;
                ocparam.Set_OC_Mode_Vinit2_Voltage(Vinit2_Voltage, SelectedMode, band);
            }
            Send_Vinit2_Voltages(SelectedMode);
        }

        private void Send_Vinit2_Voltages(OC_Mode SelectedMode)
        {
            double[] Vinit2_Voltages = ocparam.Get_OC_Mode_Vinit2_Voltages(SelectedMode);
            byte[][] output_cmd = ModelFactory.Get_DP213_Instance().Get_Vinit2_CMD(DP213OCSet.GetGammaSet(SelectedMode), Vinit2_Voltages);
            cmd.SendMipiCMD(output_cmd);
            api.WriteLine("Send_Vinit2_Voltages, OC_Mode : " + SelectedMode.ToString());
        }


        private void Update_ELVSS_LV_Array(out double[] ELVSS, out double[] Lv, List<double> ELVSS_list, List<double> Lv_list)
        {
            int size = elvss_oc_obj.Get_ELVSSArrayLength();
            ELVSS = new double[size];
            Lv = new double[size];

            for (int i = 0; i < size; i++)
            {
                int temp_index = (ELVSS_list.Count - 1) - i;
                ELVSS[i] = ELVSS_list[temp_index];
                Lv[i] = Lv_list[temp_index];
            }

            Array.Reverse(ELVSS);
            Array.Reverse(Lv);
        }

        private XYLv[] Get_Multitimes_Measured_Values(int how_many_times)
        {
            XYLv[] Temp_Measures = new XYLv[how_many_times];

            for (int i = 0; i < Temp_Measures.Length; i++)
            {
                double[] measured = api.measure_XYL(channel_num);

                Temp_Measures[i].double_X = measured[0];
                Temp_Measures[i].double_Y = measured[1];
                Temp_Measures[i].double_Lv = measured[2];
            }
            return Temp_Measures;
        }
    }
}
