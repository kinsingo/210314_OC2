using System;
using System.IO;
using System.Data;
using System.Linq;
using BSQH_Csharp_Library;
using System.Windows.Forms;
using System.Text;
using LGD_OC_AstractPlatForm.CommonAPI;

namespace LGD_OC_AstractPlatForm.OpticCompensation.DP213.Data
{
    public class DP213_OCREF0REF4095
    {
        byte Normal_REF0;
        double Normal_REF0_Voltage;
        byte Normal_REF4095;
        double Normal_REF4095_Voltage;

        DataProtocal dprotocal;
        public DP213_OCREF0REF4095(DataProtocal _dprotocal)
        {
            dprotocal = _dprotocal;
            Update_Normal_REF0_and_REF4095_From_CMD();
        }

        void Update_Normal_REF0_and_REF4095_From_CMD()
        {
            try
            {
                byte[] cmds = DP213Model.getInstance().Get_Normal_Read_REF0_REF4095_CMD();
                byte[] read_REF0_REF4095 = dprotocal.GetReadData(cmds);
                Set_Normal_REF0(DP213Model.getInstance().Get_Normal_REF0(read_REF0_REF4095));
                Set_Normal_REF4095(DP213Model.getInstance().Get_Normal_REF4095(read_REF0_REF4095));
            }
            catch (Exception)
            {
                MessageBox.Show("Update_Normal_REF0_and_REF4095_From_CMD() fail");
            }
        }


        public byte Get_Normal_REF0() { return Normal_REF0; }
        public double Get_Normal_REF0_Voltage() 
        {
            Normal_REF0_Voltage = Imported_my_cpp_dll.DP213_VREF0_Dec_to_Voltage(Normal_REF0);
            return Normal_REF0_Voltage; 
        }
        public byte Get_Normal_REF4095() { return Normal_REF4095; }
        public double Get_Normal_REF4095_Voltage() 
        {
            Normal_REF4095_Voltage = Imported_my_cpp_dll.DP213_VREF4095_Dec_to_Voltage(Normal_REF4095);
            return Normal_REF4095_Voltage; 
        }

        public void Set_Normal_REF0(byte _Normal_REF0)
        {
            Normal_REF0 = _Normal_REF0;
            Normal_REF0_Voltage = Imported_my_cpp_dll.DP213_VREF0_Dec_to_Voltage(_Normal_REF0);
        }

        public void Set_Normal_REF4095(byte _Normal_REF4095)
        {
            Normal_REF4095 = _Normal_REF4095;
            Normal_REF4095_Voltage = Imported_my_cpp_dll.DP213_VREF4095_Dec_to_Voltage(_Normal_REF4095);
        }
    }
}
