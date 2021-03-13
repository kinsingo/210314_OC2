using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LGD_OC_AstractPlatForm.CommonAPI;
using System.Windows.Forms;
using System.Threading;
using OC_Abstraction_PlatForm_WinForm_Test_Dll.Communication.Gooil;

namespace OC_Abstraction_PlatForm_WinForm_Test_Dll.Measurement
{
    class GooilMeasurement : Imeasurement
    {
        RichTextBox richtextbox;
        public GooilMeasurement(RichTextBox _richtextbox)
        {
            richtextbox = _richtextbox;
        }

        public double Get_Frequency(int channel_num)
        {
            throw new NotImplementedException();
        }

        public double[] measure_UVL(int channel_num)
        {
            throw new NotImplementedException();
        }

        public double[] measure_XYL(int channel_num)
        {
            try {return SubMeasure(channel_num); }
            catch (Exception ex) { throw new Exception(ex.Message + "]measure_XYL Exception Occured"); }
        }

        double[] SubMeasure(int channel_num)
        {
            double[] measureData = new double[3];
            try
            {
                int errorCode = ConfigInfo.objCa[channel_num].Measure();

                ConfigInfo.objProbe[channel_num].get_sx(ref measureData[0]);
                ConfigInfo.objProbe[channel_num].get_sy(ref measureData[1]);
                ConfigInfo.objProbe[channel_num].get_Lv(ref measureData[2]);

                if (measureData[0] >= 1) measureData[0] = 0;
                if (measureData[1] >= 1) measureData[1] = 0;
                if (measureData[2] >= 9999) measureData[2] = 0;

                richtextbox.AppendText(String.Format("Test Measure X/Y/Lv : {0:0.0000}/{1:0.0000}/{2:0.00} \n", measureData[0], measureData[1], measureData[2]));
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.Message);

                measureData[0] = -1;
                measureData[1] = -1;
                measureData[2] = -1;
                richtextbox.AppendText(String.Format("Test Measure X/Y/Lv : {0:0.0000}/{1:0.0000}/{2:0.00} \n", measureData[0], measureData[1], measureData[2]));
            }

            return measureData;
        }


        public static void CA_Connect()
        {
            try
            {
                bool[] bResult = { false, false, false, false, false, false, false, false };
                
                ConfigInfo.CA_Connect(ref bResult);
                for (int ca = 0; ca < ConfigInfo.nPgNum; ca++)
                {
                    if (bResult[ca])
                    {
                        MessageBox.Show(string.Format("CA410 Connect OK CH{0}", ca + 1));
                        ConfigInfo.IsCaConnected[ca] = true;
                    }
                    else
                    {
                        ConfigInfo.IsCaConnected[ca] = false;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("CA_Connect : " + ex.Message);
                throw ex;
            }
        }
    }
}
