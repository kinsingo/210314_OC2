using System;
using System.IO;
using System.Data;
using System.Linq;
using BSQH_Csharp_Library;
using System.Windows.Forms;
using System.Text;
using LGD_OC_AstractPlatForm.CommonAPI;
using System.Drawing;

namespace LGD_OC_AstractPlatForm.OpticCompensation.DP213.Data
{
    public class DP213_OCVreg1
    {
        DP213_OCREF0REF4095 ref0ref4095;
        DataProtocal dprotocal;
        public DP213_OCVreg1(DP213_OCREF0REF4095 _ref0ref4095, DataProtocal _dprotocal)
        {
            ref0ref4095 = _ref0ref4095;
            dprotocal = _dprotocal;
            Update_Vreg1_From_Sample();
        }

        void Update_Vreg1_From_Sample()
        {
            try
            {
                byte[] OCMode1_cmds = DP213Model.getInstance().Get_Normal_Read_Vreg1_CMD(DP213OCSet.GetGammaSet(OC_Mode.Mode1));
                byte[] OCMode2_cmds = DP213Model.getInstance().Get_Normal_Read_Vreg1_CMD(DP213OCSet.GetGammaSet(OC_Mode.Mode2));
                byte[] OCMode3_cmds = DP213Model.getInstance().Get_Normal_Read_Vreg1_CMD(DP213OCSet.GetGammaSet(OC_Mode.Mode3));
                byte[] OCMode4_cmds = DP213Model.getInstance().Get_Normal_Read_Vreg1_CMD(DP213OCSet.GetGammaSet(OC_Mode.Mode4));
                byte[] OCMode5_cmds = DP213Model.getInstance().Get_Normal_Read_Vreg1_CMD(DP213OCSet.GetGammaSet(OC_Mode.Mode5));
                byte[] OCMode6_cmds = DP213Model.getInstance().Get_Normal_Read_Vreg1_CMD(DP213OCSet.GetGammaSet(OC_Mode.Mode6));

                byte[] OCMode1_ReadVreg1 = dprotocal.GetReadData(OCMode1_cmds);
                byte[] OCMode2_ReadVreg1 = dprotocal.GetReadData(OCMode2_cmds);
                byte[] OCMode3_ReadVreg1 = dprotocal.GetReadData(OCMode3_cmds);
                byte[] OCMode4_ReadVreg1 = dprotocal.GetReadData(OCMode4_cmds);
                byte[] OCMode5_ReadVreg1 = dprotocal.GetReadData(OCMode5_cmds);
                byte[] OCMode6_ReadVreg1 = dprotocal.GetReadData(OCMode6_cmds);

                for (int band = 0; band < DP213_Static.Max_HBM_and_Normal_Band_Amount; band++)
                {
                    Set_OC_Mode_Vreg1(ModelFactory.Get_DP213_Instance().Get_Normal_Vreg1s(OCMode1_ReadVreg1, band), OC_Mode.Mode1, band);
                    Set_OC_Mode_Vreg1(ModelFactory.Get_DP213_Instance().Get_Normal_Vreg1s(OCMode2_ReadVreg1, band), OC_Mode.Mode2, band);
                    Set_OC_Mode_Vreg1(ModelFactory.Get_DP213_Instance().Get_Normal_Vreg1s(OCMode3_ReadVreg1, band), OC_Mode.Mode3, band);
                    Set_OC_Mode_Vreg1(ModelFactory.Get_DP213_Instance().Get_Normal_Vreg1s(OCMode4_ReadVreg1, band), OC_Mode.Mode4, band);
                    Set_OC_Mode_Vreg1(ModelFactory.Get_DP213_Instance().Get_Normal_Vreg1s(OCMode5_ReadVreg1, band), OC_Mode.Mode5, band);
                    Set_OC_Mode_Vreg1(ModelFactory.Get_DP213_Instance().Get_Normal_Vreg1s(OCMode6_ReadVreg1, band), OC_Mode.Mode6, band);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Update_Vreg1_From_Sample() fail");
            }
        }


        //paramters need to be read from samples
        int[] OC_Mode1_Vreg1 = new int[DP213_Static.Max_HBM_and_Normal_Band_Amount];
        int[] OC_Mode2_Vreg1 = new int[DP213_Static.Max_HBM_and_Normal_Band_Amount];
        int[] OC_Mode3_Vreg1 = new int[DP213_Static.Max_HBM_and_Normal_Band_Amount];
        int[] OC_Mode4_Vreg1 = new int[DP213_Static.Max_HBM_and_Normal_Band_Amount];
        int[] OC_Mode5_Vreg1 = new int[DP213_Static.Max_HBM_and_Normal_Band_Amount];
        int[] OC_Mode6_Vreg1 = new int[DP213_Static.Max_HBM_and_Normal_Band_Amount];

        double[] OC_Mode1_Vreg1_Voltage = new double[DP213_Static.Max_HBM_and_Normal_Band_Amount];
        double[] OC_Mode2_Vreg1_Voltage = new double[DP213_Static.Max_HBM_and_Normal_Band_Amount];
        double[] OC_Mode3_Vreg1_Voltage = new double[DP213_Static.Max_HBM_and_Normal_Band_Amount];
        double[] OC_Mode4_Vreg1_Voltage = new double[DP213_Static.Max_HBM_and_Normal_Band_Amount];
        double[] OC_Mode5_Vreg1_Voltage = new double[DP213_Static.Max_HBM_and_Normal_Band_Amount];
        double[] OC_Mode6_Vreg1_Voltage = new double[DP213_Static.Max_HBM_and_Normal_Band_Amount];

        public int[] Get_OC_Mode_Vreg1(OC_Mode mode)
        {
            if (mode == OC_Mode.Mode1) return OC_Mode1_Vreg1;
            if (mode == OC_Mode.Mode2) return OC_Mode2_Vreg1;
            if (mode == OC_Mode.Mode3) return OC_Mode3_Vreg1;
            if (mode == OC_Mode.Mode4) return OC_Mode4_Vreg1;
            if (mode == OC_Mode.Mode5) return OC_Mode5_Vreg1;
            if (mode == OC_Mode.Mode6) return OC_Mode6_Vreg1;
            throw new Exception("Mode Should be 1~6");
        }

        public int Get_OC_Mode_Vreg1(OC_Mode mode,int band)
        {
            if (mode == OC_Mode.Mode1) return OC_Mode1_Vreg1[band];
            if (mode == OC_Mode.Mode2) return OC_Mode2_Vreg1[band];
            if (mode == OC_Mode.Mode3) return OC_Mode3_Vreg1[band];
            if (mode == OC_Mode.Mode4) return OC_Mode4_Vreg1[band];
            if (mode == OC_Mode.Mode5) return OC_Mode5_Vreg1[band];
            if (mode == OC_Mode.Mode6) return OC_Mode6_Vreg1[band];
            throw new Exception("Mode Should be 1~6");
        }

        public double Get_OC_Mode_Vreg1_Voltage(OC_Mode mode, int band)
        {
            if (band > DP213_Static.Max_HBM_and_Normal_Band_Amount)
                throw new Exception("band should be less than " + DP213_Static.Max_HBM_and_Normal_Band_Amount);
    
            int[] OCMode_Vreg1 = Get_OC_Mode_Vreg1(mode);
            double[] OCMode_Vreg1_Voltage = Get_OC_Mode_Vreg1_Voltage(mode);
            OCMode_Vreg1_Voltage[band] = Imported_my_cpp_dll.DP213_Get_Vreg1_Voltage(ref0ref4095.Get_Normal_REF4095_Voltage(), ref0ref4095.Get_Normal_REF0_Voltage(), OCMode_Vreg1[band]);
            return OCMode_Vreg1_Voltage[band];
        }

        public void Set_OC_Mode_Vreg1(int Vreg1, OC_Mode mode, int band)
        {
            dprotocal.api.WriteLine($"[Mode{mode}/Band{band}] Vreg1 : {Vreg1}", Color.Purple);

            int[] OCMode_Vreg1 = Get_OC_Mode_Vreg1(mode);
            double[] OCMode_Vreg1_Voltage = Get_OC_Mode_Vreg1_Voltage(mode);
            OCMode_Vreg1[band] = Vreg1;
            OCMode_Vreg1_Voltage[band] = Imported_my_cpp_dll.DP213_Get_Vreg1_Voltage(ref0ref4095.Get_Normal_REF4095_Voltage(), ref0ref4095.Get_Normal_REF0_Voltage(), Vreg1);
        }

        private double[] Get_OC_Mode_Vreg1_Voltage(OC_Mode mode)
        {
            if (mode == OC_Mode.Mode1) return OC_Mode1_Vreg1_Voltage;
            if (mode == OC_Mode.Mode2) return OC_Mode2_Vreg1_Voltage;
            if (mode == OC_Mode.Mode3) return OC_Mode3_Vreg1_Voltage;
            if (mode == OC_Mode.Mode4) return OC_Mode4_Vreg1_Voltage;
            if (mode == OC_Mode.Mode5) return OC_Mode5_Vreg1_Voltage;
            if (mode == OC_Mode.Mode6) return OC_Mode6_Vreg1_Voltage;
            throw new Exception("Mode Should be 1~6");
        }
    }
}
