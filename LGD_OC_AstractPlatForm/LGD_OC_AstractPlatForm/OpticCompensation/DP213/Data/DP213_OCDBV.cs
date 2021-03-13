using System;
using BSQH_Csharp_Library;
using System.Windows.Forms;


namespace LGD_OC_AstractPlatForm.OpticCompensation.DP213.Data
{
    public class DP213_OCDBV
    {
        DataProtocal dprotocal;
        public DP213_OCDBV(DataProtocal _dprotocal)
        {
            dprotocal = _dprotocal;
            Update_DBV_From_Sample();
        }

        int[] DBV = new int[DP213_Static.Max_Band_Amount];
        public int GetDBV(int band) { return DBV[band]; }

        private void Update_DBV_From_Sample()
        {
            try
            {
                UpdateNormalDBV();
                UpdateAODDBV();
            }
            catch (Exception)
            {
                MessageBox.Show("Update_DBV_From_Sample() fail");
            }
        }


        private void UpdateNormalDBV()
        {
            for (int band = 0; band < DP213_Static.Max_HBM_and_Normal_Band_Amount; band++)
                DBV[band] = ModelFactory.Get_DP213_Instance().Get_Normal_DBV(Get_DBV_Normal_ReadData(), band);
        }
        private void UpdateAODDBV()
        {
            for (int band = DP213_Static.Max_HBM_and_Normal_Band_Amount; band < DP213_Static.Max_Band_Amount; band++)
                DBV[band] = ModelFactory.Get_DP213_Instance().Get_AOD_DBV(Get_DBV_AOD_ReadData(), (band - DP213_Static.Max_HBM_and_Normal_Band_Amount));
        }

        private byte[] Get_DBV_Normal_ReadData()
        {
            byte[] cmds_normal = DP213Model.getInstance().Get_Normal_Read_DBV_CMD();
            return dprotocal.GetReadData(cmds_normal);
        }

        private byte[] Get_DBV_AOD_ReadData()
        {
            byte[] cmds_AOD = DP213Model.getInstance().Get_AOD_Read_DBV_CMD();
            return dprotocal.GetReadData(cmds_AOD);
        }

    }
}
