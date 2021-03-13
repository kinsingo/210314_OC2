using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LGD_OC_AstractPlatForm.CommonAPI;
using System.Windows.Forms;
using System.Threading;
using OC_Abstraction_PlatForm_WinForm_Test_Dll.Measurement.LGD_CA;


namespace OC_Abstraction_PlatForm_WinForm_Test_Dll.Measurement
{
    class LGDMeasurement : Imeasurement
    {
        RichTextBox richtextbox;
        Single_CA410_Control singleCA410obj;
        public LGDMeasurement (RichTextBox _richtextbox)
        {
            richtextbox = _richtextbox;
            singleCA410obj = new Single_CA410_Control(_richtextbox);
        }

        public double Get_Frequency(int channel_num)
        {
            try { return singleCA410obj.Get_Frequency(channel_num); }
            catch (Exception ex) { throw new Exception(ex.Message + "]Get_Frequency Exception Occured"); }
        }

        public double[] measure_UVL(int channel_num)
        {
            try {return singleCA410obj.measure_UVL(channel_num); }
            catch (Exception ex) { throw new Exception(ex.Message + "]measure_UVL Exception Occured"); }
        }

        public double[] measure_XYL(int channel_num)
        {
            try {return singleCA410obj.measure_XYL(channel_num); }
            catch(Exception ex) { throw new Exception(ex.Message + "]measure_XYL Exception Occured"); }
        }

        public static void CA_Connect()
        {
            bool Connected = Single_CA410_Control.connect_CA();

            if (Connected == false)
                throw new Exception("CA_Connect failed");
        }
    }
}
