using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO.Ports;//Port 190530
using CASDK2;
using LGD_OC_AstractPlatForm.CommonAPI;

namespace OC_Abstraction_PlatForm_WinForm_Test_Dll.Measurement.LGD_CA
{
    class Single_CA410_Control : Imeasurement
    {
        //Will be created by factory.
        static CASDK2Ca objCa;
        static CASDK2Probes objProbes;
        static CASDK2OutputProbes objOutputProbes;
        static CASDK2Probe objProbe;
        static CASDK2Memory objMemory;
        //static CASDK2DeviceData[] portDeviceData; //there can be many port

        //Need to be initailized
        static CASDK2Ca200 objCa200;
        static CASDK2Cas objCas;
        static bool Is_CA_Connected;
        RichTextBox richtextbox;

        public Single_CA410_Control(RichTextBox _richtextbox)
        {
            richtextbox = _richtextbox;
        }

        static bool ShowCAErrorMsg(int errornum)
        {
            string errormessage = "";
            if (errornum != 0)
            {
                GlobalFunctions.CASDK2_GetLocalizedErrorMsgFromErrorCode(0, errornum, ref errormessage);
                MessageBox.Show(errormessage);
                return false;
            }
            return true;
        }

    

        public static bool connect_CA()
        {
            initialize();
            const int ca_port = 5;//CA Port Must be "COM5";

            bool IsConnectionValid = ShowCAErrorMsg(objCa200.SetConfiguration(1, "1", ca_port, 38400, 1)); if (IsConnectionValid == false) return false;
            IsConnectionValid = ShowCAErrorMsg(objCa200.get_Cas(ref objCas)); if (IsConnectionValid == false) return false;
            IsConnectionValid = ShowCAErrorMsg(objCas.get_Item(0 + 1, ref objCa)); if (IsConnectionValid == false) return false;
            IsConnectionValid = ShowCAErrorMsg(objCa.get_Probes(ref objProbes)); if (IsConnectionValid == false) return false;
            IsConnectionValid = ShowCAErrorMsg(objCa.get_OutputProbes(ref objOutputProbes)); if (IsConnectionValid == false) return false;
            IsConnectionValid = ShowCAErrorMsg(objCa.get_Memory(ref objMemory)); if (IsConnectionValid == false) return false;
            IsConnectionValid = ShowCAErrorMsg(objProbes.get_Item(1, ref objProbe)); if (IsConnectionValid == false) return false;
            IsConnectionValid = ShowCAErrorMsg(objOutputProbes.AddAll()); if (IsConnectionValid == false) return false;
            IsConnectionValid = ShowCAErrorMsg(objOutputProbes.get_Item(1, ref objProbe)); if (IsConnectionValid == false) return false;

            Is_CA_Connected = true;

            if (Is_CA_Connected)
                CA_Setting_After_CA_Connect();
            
            return Is_CA_Connected;
        }
        static void initialize()
        {
            objCa200 = new CASDK2Ca200();
            objCa200.get_Cas(ref objCas);
            Is_CA_Connected = false;
        }








        







        static void CA_Setting_After_CA_Connect()
        {
            int freqmode = 0;   // SyncMode : NTSC 0 / PAL 1 / EXT 2 / INT 4
            double freq = 60.0; //frequency = 60.0Hz
            int speed = 1;      //Measurement speed : FAST
            int Lvmode = 1;     //Lv : cd/m2
            int DisplayMode = 0; //Lxy = 0;

            ShowCAErrorMsg(objCa.put_DisplayMode(DisplayMode));             //Set Lvxy mode
            ShowCAErrorMsg(objCa.CalZero());                      //Zero-Calibration
            ShowCAErrorMsg(objCa.put_DisplayProbe("P1"));         //Set display probe to P1
            ShowCAErrorMsg(objCa.put_SyncMode(freqmode, freq));   //Set sync mode and frequency
            ShowCAErrorMsg(objCa.put_AveragingMode(speed));       //Set measurement speed
            ShowCAErrorMsg(objCa.put_BrightnessUnit(Lvmode));     //Set Brightness unit
            //GetErrorMessage(objMemory.put_ChannelNO(Convert.ToInt32(textBox_ch_W.Text)));//White Channel.
        }

        public double[] measure_XYL(int channel_num)
        {
            double[] measureData = new double[3];

            ShowCAErrorMsg(objCas.SendMsr());         //Measure
            ShowCAErrorMsg(objCas.ReceiveMsr());      //Get results

            // Get measurement data
            ShowCAErrorMsg(objProbe.get_sx(ref measureData[0]));
            ShowCAErrorMsg(objProbe.get_sy(ref measureData[1]));
            ShowCAErrorMsg(objProbe.get_Lv(ref measureData[2]));

            richtextbox.AppendText(String.Format("Measure X/Y/Lv : {0:0.0000}/{1:0.0000}/{2:0.00} \n", measureData[0], measureData[1], measureData[2]));

            return measureData;
        }

        public double[] measure_UVL(int channel_num)
        {
            throw new NotImplementedException();
        }

         public double Get_Frequency(int channel_num)
        {
            throw new NotImplementedException();
        }

        static public bool IsConnected()
        {
            return Is_CA_Connected;
        }

        /*
        public void Zero_Cal()
        {
            GetErrorMessage(objCa.CalZero()); 
        }
        public void Set_SyncMode(int freqmode, double freq = 60.0)
        {
            GetErrorMessage(objCa.put_SyncMode(freqmode, freq));
        }

        public void Set_MeasruemnetMode(int MeasuremntMode)
        {
            GetErrorMessage(objCa.put_AveragingMode(MeasuremntMode));
        }

        public void Set_White_Channel(int ca_ch)
        {
            GetErrorMessage(objMemory.put_ChannelNO(ca_ch));
        }
        */


    }
}
