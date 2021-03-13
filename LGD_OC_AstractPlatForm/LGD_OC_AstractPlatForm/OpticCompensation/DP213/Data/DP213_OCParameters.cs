using System;
using System.IO;
using System.Data;
using System.Linq;
using BSQH_Csharp_Library;
using System.Windows.Forms;
using System.Text;
using LGD_OC_AstractPlatForm.CommonAPI;
using System.Collections.Generic;

namespace LGD_OC_AstractPlatForm.OpticCompensation.DP213.Data
{
    public class DP213_OCParameters : IOCparamters
    {
        DataProtocal dprotocal;
        DP213_OCDBV dbv;
        DP213_OCELVSSVinit2 elvssVini2;
        DP213_OCExtensionXY extensionXY;
        DP213_OCGray grayValue;
        DP213_OCLimit limit;
        DP213_OCLoopCount loopCount;
        DP213_OCMeasure measure;
        DP213_OCTarget target;

        DP213_OCREF0REF4095 ref0ref4095;
        DP213_OCVreg1 vreg1;
        DP213_OCAM1AM0 am1am0;
        DP213_OCMode_RGB rgb;

        public DP213_OCParameters(IBusinessAPI _api, int _channel_num, RichTextBox _richTextBox)
        {
            elvssVini2 = new DP213_OCELVSSVinit2();
            extensionXY = new DP213_OCExtensionXY();
            grayValue = new DP213_OCGray();
            limit = new DP213_OCLimit();
            loopCount = new DP213_OCLoopCount();
            measure = new DP213_OCMeasure();
            target = new DP213_OCTarget();
            dprotocal = new DataProtocal(_api, _channel_num, _richTextBox);
            Read_And_Initailze_Vals_And_Voltages(dprotocal);
            Update_OC_Param_from_CSV();
        }

        private void Read_And_Initailze_Vals_And_Voltages(DataProtocal dprotocal)
        {
            //REF0REF4095 --> Vreg1 --> AM1AM0 --> RGB ()
            dbv = new DP213_OCDBV(dprotocal);
            ref0ref4095 = new DP213_OCREF0REF4095(dprotocal);
            vreg1 = new DP213_OCVreg1(ref0ref4095, dprotocal);
            am1am0 = new DP213_OCAM1AM0(ref0ref4095, vreg1);
            rgb = new DP213_OCMode_RGB(ref0ref4095, vreg1, am1am0, dprotocal);
        }


        //ELVSS and Vinit2
        public double Get_OC_Mode_ELVSS_Voltage(OC_Mode mode, int band) => elvssVini2.Get_OC_Mode_ELVSS_Voltage(mode, band); 
        public double[] Get_OC_Mode_ELVSS_Voltages(OC_Mode mode) => elvssVini2.Get_OC_Mode_ELVSS_Voltages(mode); 
        public void Set_OC_Mode_ELVSS_Voltage(double ELVSS_Voltage, OC_Mode mode, int band) => elvssVini2.Set_OC_Mode_ELVSS_Voltage(ELVSS_Voltage, mode, band); 
        public double Get_OC_Mode_Vinit2_Voltage(OC_Mode mode, int band)=> elvssVini2.Get_OC_Mode_Vinit2_Voltage(mode, band);  
        public double[] Get_OC_Mode_Vinit2_Voltages(OC_Mode mode) => elvssVini2.Get_OC_Mode_Vinit2_Voltages(mode); 
        public void Set_OC_Mode_Vinit2_Voltage(double Vinit2_Voltage, OC_Mode mode, int band) => elvssVini2.Set_OC_Mode_Vinit2_Voltage(Vinit2_Voltage, mode, band);  
        public double Get_OC_Mode_Cold_ELVSS_Voltage(OC_Mode mode, int band) => elvssVini2.Get_OC_Mode_Cold_ELVSS_Voltage(mode, band); 
        public double[] Get_OC_Mode_Cold_ELVSS_Voltages(OC_Mode mode)=> elvssVini2.Get_OC_Mode_Cold_ELVSS_Voltages(mode); 
        public void Set_OC_Mode_Cold_ELVSS_Voltage(double Cold_ELVSS_Voltage, OC_Mode mode, int band) => elvssVini2.Set_OC_Mode_Cold_ELVSS_Voltage(Cold_ELVSS_Voltage, mode, band); 
        public double Get_OC_Mode_Cold_Vinit2_Voltage(OC_Mode mode, int band) => elvssVini2.Get_OC_Mode_Cold_Vinit2_Voltage(mode, band); 
        public double[] Get_OC_Mode_Cold_Vinit2_Voltages(OC_Mode mode)=> elvssVini2.Get_OC_Mode_Cold_Vinit2_Voltages(mode); 
        public void Set_OC_Mode_Cold_Vinit2_Voltage(double Cold_Vinit2_Voltage, OC_Mode mode, int band) => elvssVini2.Set_OC_Mode_Cold_Vinit2_Voltage(Cold_Vinit2_Voltage, mode, band); 


        //Extension
        public XYLv Get_OC_Mode_ExtensionXY(OC_Mode mode, int band, int gray) => extensionXY.Get_OC_Mode_ExtensionXY(mode, band, gray); 
        public void Set_OC_Mode_ExtensionXY(OC_Mode mode, int band, int gray, XYLv xylv) => extensionXY.Set_OC_Mode_ExtensionXY(mode, band, gray, xylv); 
        public bool Get_OC_Mode_IsExtensionApplied(OC_Mode mode, int band, int gray) => extensionXY.Get_OC_Mode_IsExtensionApplied(mode, band, gray); 
        public void Set_OC_Mode_IsExtensionApplied(OC_Mode mode, int band, int gray, bool IsApplied) => extensionXY.Set_OC_Mode_IsExtensionApplied(mode, band, gray, IsApplied); 


        //GrayValue
        public int Get_OC_Mode_Gray(OC_Mode mode, int bandindex, int grayindex) => grayValue.Get_OC_Mode_Gray( mode,  bandindex,  grayindex); 
        public void Set_OC_Mode_Gray(OC_Mode mode, int bandindex, int grayindex, int GrayValue) => grayValue.Set_OC_Mode_Gray( mode,  bandindex,  grayindex,  GrayValue); 

        //Limit
        public XYLv Get_OC_Mode_Limit(OC_Mode mode, int band, int gray) => limit.Get_OC_Mode_Limit( mode,  band,  gray); 
        public void Set_OC_Mode_Limit(OC_Mode mode, int band, int gray, XYLv xylv) => limit.Set_OC_Mode_Limit( mode,  band,  gray,  xylv); 

        //LoopCount
        public int Get_OC_Mode_LoopCount(OC_Mode mode, int band, int gray) => loopCount.Get_OC_Mode_LoopCount( mode,  band,  gray); 
        public void Set_OC_Mode_LoopCount(int loopcount, OC_Mode mode, int band, int gray) => loopCount.Set_OC_Mode_LoopCount( loopcount,  mode,  band,  gray); 

        //Measure
        public XYLv Get_OC_Mode_Measure(OC_Mode mode, int band, int gray) => measure.Get_OC_Mode_Measure( mode,  band,  gray); 
        public void Set_OC_Mode_Measure(XYLv measured, OC_Mode mode, int band, int gray) => measure.Set_OC_Mode_Measure(measured, mode, band, gray);

        //Target
        public XYLv Get_OC_Mode_Target(OC_Mode mode, int band, int gray) => target.Get_OC_Mode_Target(mode, band, gray);
        public void Set_OC_Mode_Target(OC_Mode mode, int band, int gray, XYLv xylv) => target.Set_OC_Mode_Target(mode, band, gray, xylv);

        //DBV
        public int GetDBV(int band) => dbv.GetDBV(band);

        //REF0REF4095
        public byte Get_Normal_REF0() => ref0ref4095.Get_Normal_REF0();
        public double Get_Normal_REF0_Voltage() => ref0ref4095.Get_Normal_REF0_Voltage();
        public byte Get_Normal_REF4095() => ref0ref4095.Get_Normal_REF4095();
        public double Get_Normal_REF4095_Voltage() => ref0ref4095.Get_Normal_REF4095_Voltage();
        public void Set_Normal_REF0(byte _Normal_REF0) => ref0ref4095.Set_Normal_REF0(_Normal_REF0);
        public void Set_Normal_REF4095(byte _Normal_REF4095) => ref0ref4095.Set_Normal_REF4095(_Normal_REF4095);

        //Vreg1
        public int[] Get_OC_Mode_Vreg1(OC_Mode mode) => vreg1.Get_OC_Mode_Vreg1(mode);
        public int Get_OC_Mode_Vreg1(OC_Mode mode, int band) => vreg1.Get_OC_Mode_Vreg1(mode, band);
        public double Get_OC_Mode_Vreg1_Voltage(OC_Mode mode, int band) => vreg1.Get_OC_Mode_Vreg1_Voltage(mode, band);
        public void Set_OC_Mode_Vreg1(int Vreg1, OC_Mode mode, int band) => vreg1.Set_OC_Mode_Vreg1(Vreg1, mode, band);

       

        //AM1AM0
        public RGB Get_OC_Mode_AM1(OC_Mode mode, int band) => am1am0.Get_OC_Mode_AM1(mode, band); 
        public RGB_Double Get_OC_Mode_AM1_Voltage(OC_Mode mode, int band) => am1am0.Get_OC_Mode_AM1_Voltage(mode, band); 
        public void Set_OC_Mode_AM1(RGB AM1, OC_Mode mode, int band) => am1am0.Set_OC_Mode_AM1(AM1, mode, band);
        public void Set_OC_Mode_AM1(RGB_Double AM1_Voltage, OC_Mode mode, int band) => am1am0.Set_OC_Mode_AM1(AM1_Voltage, mode, band);


        public RGB Get_OC_Mode_AM0(OC_Mode mode, int band) => am1am0.Get_OC_Mode_AM0(mode, band); 
        public RGB_Double Get_OC_Mode_AM0_Voltage(OC_Mode mode, int band) => am1am0.Get_OC_Mode_AM0_Voltage(mode, band); 
        public void Set_OC_Mode_AM0(RGB AM0, OC_Mode mode, int band) => am1am0.Set_OC_Mode_AM0(AM0, mode, band);
        public void Set_OC_Mode_AM0(RGB_Double AM0_Voltage, OC_Mode mode, int band) => am1am0.Set_OC_Mode_AM0(AM0_Voltage, mode, band);

        //RGB
        public RGB Get_OC_Mode_RGB(OC_Mode mode, int band, int gray) => rgb.Get_OC_Mode_RGB(mode, band, gray);
        public RGB_Double Get_OC_Mode_RGB_Voltage(OC_Mode mode, int band, int gray) => rgb.Get_OC_Mode_RGB_Voltage(mode, band, gray);
        public RGB[] Get_OC_Mode_RGB(OC_Mode mode, int band) => rgb.Get_OC_Mode_RGB(mode, band);
        public void Set_OC_Mode_RGB(RGB Gamma, OC_Mode mode, int band, int gray) => rgb.Set_OC_Mode_RGB(Gamma, mode, band, gray);

        public void Set_OC_Mode_RGB(RGB[] Gamma, OC_Mode mode, int band) => rgb.Set_OC_Mode_RGB(Gamma, mode, band);

        public void ShowOCParamData(OC_Mode mode)
        {
            dprotocal.richTextBox.AppendText("Gray,R,G,B,Measure_X,Measured_Y,Measured_Lv,Target_X,Target_Y,Target_Lv,Limit_X,Limit_Y,Limit_Lv,Extension_X,Extension_Y,ExtensionApplied,LoopCount + \n");
            for (int band = 0; band < DP213_Static.Max_Band_Amount; band++)
            {
                for (int gray = 0; gray < DP213_Static.Max_Gray_Amount; gray++)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append(grayValue.Get_OC_Mode_Gray(mode,band,gray) + ",").Append(rgb.Get_OC_Mode_RGB(mode,band,gray).int_R + ",").Append(rgb.Get_OC_Mode_RGB(mode, band, gray).int_G + ",").Append(rgb.Get_OC_Mode_RGB(mode, band, gray).int_B + ",")
                        .Append(measure.Get_OC_Mode_Measure(mode,band,gray).double_X + ",").Append(measure.Get_OC_Mode_Measure(mode, band, gray).double_Y + ",").Append(measure.Get_OC_Mode_Measure(mode, band, gray).double_Lv + ",")
                        .Append(target.Get_OC_Mode_Target(mode,band,gray).double_X + ",").Append(target.Get_OC_Mode_Target(mode, band, gray).double_Y + ",").Append(target.Get_OC_Mode_Target(mode, band, gray).double_Lv + ",")
                        .Append(limit.Get_OC_Mode_Limit(mode,band, gray).double_X + ",").Append(limit.Get_OC_Mode_Limit(mode, band, gray).double_Y + ",").Append(limit.Get_OC_Mode_Limit(mode, band, gray).double_Lv + ",")
                        .Append(extensionXY.Get_OC_Mode_ExtensionXY(mode,band,gray).double_X + ",").Append(extensionXY.Get_OC_Mode_ExtensionXY(mode, band, gray).double_Y + ",")
                        .Append(extensionXY.Get_OC_Mode_IsExtensionApplied(mode,band,gray) + ",")
                        .Append(loopCount.Get_OC_Mode_LoopCount(mode,band,gray) + "\n");
                    dprotocal.richTextBox.AppendText(sb.ToString());
                }
            }

            dprotocal.richTextBox.AppendText("\n\n");
            for (int band = 0; band < DP213_Static.Max_HBM_and_Normal_Band_Amount; band++)
                dprotocal.richTextBox.AppendText("Band" + band + " Vreg1 = " + vreg1.Get_OC_Mode_Vreg1(mode,band) + "\n");

            for (int band = 0; band < DP213_Static.Max_Band_Amount; band++)
                dprotocal.richTextBox.AppendText("Band" + band + " DBV = " + dbv.GetDBV(band) + "\n");

            dprotocal.richTextBox.AppendText("Normal_REF0 = " + ref0ref4095.Get_Normal_REF0() + "\n");
            dprotocal.richTextBox.AppendText("Normal_REF4095 = " + ref0ref4095.Get_Normal_REF4095() + "\n");
        }

        public void ShowVoltageData(OC_Mode mode)
        {
            dprotocal.richTextBox.AppendText("Gray,R,G,B,AM1_R,AM1_G,AM1_B,AM0_R,AM0_G,AM0_B \n");
            for (int band = 0; band < DP213_Static.Max_HBM_and_Normal_Band_Amount; band++)
            {
                for (int gray = 0; gray < DP213_Static.Max_Gray_Amount; gray++)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append(grayValue.Get_OC_Mode_Gray(mode, band, gray) + ",").Append(rgb.Get_OC_Mode_RGB_Voltage(mode, band, gray).double_R + ",").Append(rgb.Get_OC_Mode_RGB_Voltage(mode, band, gray).double_G + ",").Append(rgb.Get_OC_Mode_RGB_Voltage(mode, band, gray).double_B + ",")
                        .Append(am1am0.Get_OC_Mode_AM1_Voltage(mode, band).double_R + ",").Append(am1am0.Get_OC_Mode_AM1_Voltage(mode, band).double_G + ",").Append(am1am0.Get_OC_Mode_AM1_Voltage(mode, band).double_B + ",")
                        .Append(am1am0.Get_OC_Mode_AM0_Voltage(mode, band).double_R + ",").Append(am1am0.Get_OC_Mode_AM0_Voltage(mode, band).double_G + ",").Append(am1am0.Get_OC_Mode_AM0_Voltage(mode, band).double_B + "\n");
                    dprotocal.richTextBox.AppendText(sb.ToString());
                }
            }

            dprotocal.richTextBox.AppendText("\n\n");
            for (int band = 0; band < DP213_Static.Max_HBM_and_Normal_Band_Amount; band++)
                dprotocal.richTextBox.AppendText("Band" + band + " Vreg1 = " + vreg1.Get_OC_Mode_Vreg1_Voltage(mode, band) + "\n");

            dprotocal.richTextBox.AppendText("Normal_REF0 = " + ref0ref4095.Get_Normal_REF0_Voltage() + "\n");
            dprotocal.richTextBox.AppendText("Normal_REF4095 = " + ref0ref4095.Get_Normal_REF4095_Voltage() + "\n");
        }


        void Update_OC_Param_from_CSV()
        {
            try
            {
                string OC_Param_Mode1_Address = Directory.GetCurrentDirectory() + "\\Parameter\\OC_Parameter_G2G_Mode1.csv";
                string OC_Param_Mode2_Address = Directory.GetCurrentDirectory() + "\\Parameter\\OC_Parameter_G2G_Mode2.csv";
                string OC_Param_Mode3_Address = Directory.GetCurrentDirectory() + "\\Parameter\\OC_Parameter_G2G_Mode3.csv";
                string OC_Param_Mode4_Address = Directory.GetCurrentDirectory() + "\\Parameter\\OC_Parameter_G2G_Mode4.csv";
                string OC_Param_Mode5_Address = Directory.GetCurrentDirectory() + "\\Parameter\\OC_Parameter_G2G_Mode5.csv";
                string OC_Param_Mode6_Address = Directory.GetCurrentDirectory() + "\\Parameter\\OC_Parameter_G2G_Mode6.csv";

                string[][] OC_data_1 = File.ReadLines(OC_Param_Mode1_Address).Select(x => x.Split(',')).ToArray(); //Write OC_file to OC_data[][]
                string[][] OC_data_2 = File.ReadLines(OC_Param_Mode2_Address).Select(x => x.Split(',')).ToArray(); //Write OC_file to OC_data[][]
                string[][] OC_data_3 = File.ReadLines(OC_Param_Mode3_Address).Select(x => x.Split(',')).ToArray(); //Write OC_file to OC_data[][]
                string[][] OC_data_4 = File.ReadLines(OC_Param_Mode4_Address).Select(x => x.Split(',')).ToArray(); //Write OC_file to OC_data[][]
                string[][] OC_data_5 = File.ReadLines(OC_Param_Mode5_Address).Select(x => x.Split(',')).ToArray(); //Write OC_file to OC_data[][]
                string[][] OC_data_6 = File.ReadLines(OC_Param_Mode6_Address).Select(x => x.Split(',')).ToArray(); //Write OC_file to OC_data[][]

                int OC_row_length = OC_data_1.GetLength(0);
                for (int row = 2; row < OC_row_length; row++)
                {
                    int index = (row - 2);
                    int band = (index / DP213_Static.Max_Gray_Amount);
                    int gray = (index % DP213_Static.Max_Gray_Amount);

                    string Mode1_Gray = OC_data_1[row][0].Split('_')[1].Substring(1);
                    string Mode2_Gray = OC_data_2[row][0].Split('_')[1].Substring(1);
                    string Mode3_Gray = OC_data_3[row][0].Split('_')[1].Substring(1);
                    string Mode4_Gray = OC_data_4[row][0].Split('_')[1].Substring(1);
                    string Mode5_Gray = OC_data_5[row][0].Split('_')[1].Substring(1);
                    string Mode6_Gray = OC_data_6[row][0].Split('_')[1].Substring(1);

                    grayValue.Set_OC_Mode_Gray(OC_Mode.Mode1, band, gray, Convert.ToInt32(Mode1_Gray));
                    grayValue.Set_OC_Mode_Gray(OC_Mode.Mode2, band, gray, Convert.ToInt32(Mode2_Gray));
                    grayValue.Set_OC_Mode_Gray(OC_Mode.Mode3, band, gray, Convert.ToInt32(Mode3_Gray));
                    grayValue.Set_OC_Mode_Gray(OC_Mode.Mode4, band, gray, Convert.ToInt32(Mode4_Gray));
                    grayValue.Set_OC_Mode_Gray(OC_Mode.Mode5, band, gray, Convert.ToInt32(Mode5_Gray));
                    grayValue.Set_OC_Mode_Gray(OC_Mode.Mode6, band, gray, Convert.ToInt32(Mode6_Gray));

                    //(Gamma)Red,Green,Blue
                    rgb.Set_OC_Mode_RGB(new RGB(Convert.ToInt32(OC_data_1[row][1]), Convert.ToInt32(OC_data_1[row][2]), Convert.ToInt32(OC_data_1[row][3])), OC_Mode.Mode1, band, gray);
                    rgb.Set_OC_Mode_RGB(new RGB(Convert.ToInt32(OC_data_2[row][1]), Convert.ToInt32(OC_data_2[row][2]), Convert.ToInt32(OC_data_2[row][3])), OC_Mode.Mode2, band, gray);
                    rgb.Set_OC_Mode_RGB(new RGB(Convert.ToInt32(OC_data_3[row][1]), Convert.ToInt32(OC_data_3[row][2]), Convert.ToInt32(OC_data_3[row][3])), OC_Mode.Mode3, band, gray);
                    rgb.Set_OC_Mode_RGB(new RGB(Convert.ToInt32(OC_data_4[row][1]), Convert.ToInt32(OC_data_4[row][2]), Convert.ToInt32(OC_data_4[row][3])), OC_Mode.Mode4, band, gray);
                    rgb.Set_OC_Mode_RGB(new RGB(Convert.ToInt32(OC_data_5[row][1]), Convert.ToInt32(OC_data_5[row][2]), Convert.ToInt32(OC_data_5[row][3])), OC_Mode.Mode5, band, gray);
                    rgb.Set_OC_Mode_RGB(new RGB(Convert.ToInt32(OC_data_6[row][1]), Convert.ToInt32(OC_data_6[row][2]), Convert.ToInt32(OC_data_6[row][3])), OC_Mode.Mode6, band, gray);

                    //(Measure)X,Y,Lv,
                    measure.Set_OC_Mode_Measure(new XYLv(0, 0, 0), OC_Mode.Mode1, band, gray);
                    measure.Set_OC_Mode_Measure(new XYLv(0, 0, 0), OC_Mode.Mode2, band, gray);
                    measure.Set_OC_Mode_Measure(new XYLv(0, 0, 0), OC_Mode.Mode3, band, gray);
                    measure.Set_OC_Mode_Measure(new XYLv(0, 0, 0), OC_Mode.Mode4, band, gray);
                    measure.Set_OC_Mode_Measure(new XYLv(0, 0, 0), OC_Mode.Mode5, band, gray);
                    measure.Set_OC_Mode_Measure(new XYLv(0, 0, 0), OC_Mode.Mode6, band, gray);

                    //(Target)x,y,Lv,
                    target.Set_OC_Mode_Target(OC_Mode.Mode1, band, gray, new XYLv(Convert.ToDouble(OC_data_1[row][7]), Convert.ToDouble(OC_data_1[row][8]), Convert.ToDouble(OC_data_1[row][9])));
                    target.Set_OC_Mode_Target(OC_Mode.Mode2, band, gray, new XYLv(Convert.ToDouble(OC_data_2[row][7]), Convert.ToDouble(OC_data_2[row][8]), Convert.ToDouble(OC_data_2[row][9])));
                    target.Set_OC_Mode_Target(OC_Mode.Mode3, band, gray, new XYLv(Convert.ToDouble(OC_data_3[row][7]), Convert.ToDouble(OC_data_3[row][8]), Convert.ToDouble(OC_data_3[row][9])));
                    target.Set_OC_Mode_Target(OC_Mode.Mode4, band, gray, new XYLv(Convert.ToDouble(OC_data_4[row][7]), Convert.ToDouble(OC_data_4[row][8]), Convert.ToDouble(OC_data_4[row][9])));
                    target.Set_OC_Mode_Target(OC_Mode.Mode5, band, gray, new XYLv(Convert.ToDouble(OC_data_5[row][7]), Convert.ToDouble(OC_data_5[row][8]), Convert.ToDouble(OC_data_5[row][9])));
                    target.Set_OC_Mode_Target(OC_Mode.Mode6, band, gray, new XYLv(Convert.ToDouble(OC_data_6[row][7]), Convert.ToDouble(OC_data_6[row][8]), Convert.ToDouble(OC_data_6[row][9])));

                    //(Limit)x,y,Lv,
                    limit.Set_OC_Mode_Limit(OC_Mode.Mode1, band, gray, new XYLv(Convert.ToDouble(OC_data_1[row][10]), Convert.ToDouble(OC_data_1[row][11]), Convert.ToDouble(OC_data_1[row][12])));
                    limit.Set_OC_Mode_Limit(OC_Mode.Mode2, band, gray, new XYLv(Convert.ToDouble(OC_data_2[row][10]), Convert.ToDouble(OC_data_2[row][11]), Convert.ToDouble(OC_data_2[row][12])));
                    limit.Set_OC_Mode_Limit(OC_Mode.Mode3, band, gray, new XYLv(Convert.ToDouble(OC_data_3[row][10]), Convert.ToDouble(OC_data_3[row][11]), Convert.ToDouble(OC_data_3[row][12])));
                    limit.Set_OC_Mode_Limit(OC_Mode.Mode4, band, gray, new XYLv(Convert.ToDouble(OC_data_4[row][10]), Convert.ToDouble(OC_data_4[row][11]), Convert.ToDouble(OC_data_4[row][12])));
                    limit.Set_OC_Mode_Limit(OC_Mode.Mode5, band, gray, new XYLv(Convert.ToDouble(OC_data_5[row][10]), Convert.ToDouble(OC_data_5[row][11]), Convert.ToDouble(OC_data_5[row][12])));
                    limit.Set_OC_Mode_Limit(OC_Mode.Mode6, band, gray, new XYLv(Convert.ToDouble(OC_data_6[row][10]), Convert.ToDouble(OC_data_6[row][11]), Convert.ToDouble(OC_data_6[row][12])));

                    //(Extension)X,Y
                    extensionXY.Set_OC_Mode_ExtensionXY(OC_Mode.Mode1, band, gray, new XYLv(Convert.ToDouble(OC_data_1[row][13]), Convert.ToDouble(OC_data_1[row][14]), 0));
                    extensionXY.Set_OC_Mode_ExtensionXY(OC_Mode.Mode2, band, gray, new XYLv(Convert.ToDouble(OC_data_2[row][13]), Convert.ToDouble(OC_data_2[row][14]), 0));
                    extensionXY.Set_OC_Mode_ExtensionXY(OC_Mode.Mode3, band, gray, new XYLv(Convert.ToDouble(OC_data_3[row][13]), Convert.ToDouble(OC_data_3[row][14]), 0));
                    extensionXY.Set_OC_Mode_ExtensionXY(OC_Mode.Mode4, band, gray, new XYLv(Convert.ToDouble(OC_data_4[row][13]), Convert.ToDouble(OC_data_4[row][14]), 0));
                    extensionXY.Set_OC_Mode_ExtensionXY(OC_Mode.Mode5, band, gray, new XYLv(Convert.ToDouble(OC_data_5[row][13]), Convert.ToDouble(OC_data_5[row][14]), 0));
                    extensionXY.Set_OC_Mode_ExtensionXY(OC_Mode.Mode6, band, gray, new XYLv(Convert.ToDouble(OC_data_6[row][13]), Convert.ToDouble(OC_data_6[row][14]), 0));

                    //Applied
                    extensionXY.Set_OC_Mode_IsExtensionApplied(OC_Mode.Mode1, band, gray, false);
                    extensionXY.Set_OC_Mode_IsExtensionApplied(OC_Mode.Mode2, band, gray, false);
                    extensionXY.Set_OC_Mode_IsExtensionApplied(OC_Mode.Mode3, band, gray, false);
                    extensionXY.Set_OC_Mode_IsExtensionApplied(OC_Mode.Mode4, band, gray, false);
                    extensionXY.Set_OC_Mode_IsExtensionApplied(OC_Mode.Mode5, band, gray, false);
                    extensionXY.Set_OC_Mode_IsExtensionApplied(OC_Mode.Mode6, band, gray, false);

                    //Loop
                    loopCount.Set_OC_Mode_LoopCount(0, OC_Mode.Mode1, band, gray);
                    loopCount.Set_OC_Mode_LoopCount(0, OC_Mode.Mode2, band, gray);
                    loopCount.Set_OC_Mode_LoopCount(0, OC_Mode.Mode3, band, gray);
                    loopCount.Set_OC_Mode_LoopCount(0, OC_Mode.Mode4, band, gray);
                    loopCount.Set_OC_Mode_LoopCount(0, OC_Mode.Mode5, band, gray);
                    loopCount.Set_OC_Mode_LoopCount(0, OC_Mode.Mode6, band, gray);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Update_OC_Param_from_CSV() fail : " + ex.Message);
            }
        }
    }
}
