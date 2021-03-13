using CASDK2;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OC_Abstraction_PlatForm_WinForm_Test_Dll.Communication.Gooil
{
    class ConfigInfo
    {
        public static int nPgNum = 8;
        public static SocketControl[] sockControl = new SocketControl[nPgNum];

        public static Barrier[] barrier_SendPacket = { null, null };

        public static Barrier barrier_ModelChange = null;

        //  CA-410
        public static CASDK2Ca200 objCa200 = new CASDK2Ca200();
        public static CASDK2Cas objCas;
        public static CASDK2Ca[] objCa = new CASDK2Ca[8];
        public static CASDK2Probes[] objProbes = new CASDK2Probes[8];
        public static CASDK2OutputProbes[] objOutputProbes = new CASDK2OutputProbes[8];
        public static CASDK2Probe[] objProbe = new CASDK2Probe[8];
        public static CASDK2Memory[] objMemory = new CASDK2Memory[8];
        public static CASDK2MemoryStatus pMemCHData = new CASDK2MemoryStatus();
        public static CASDK2DeviceData[] pDeviceData = new CASDK2DeviceData[255];
        public static string[] strProbeChInfo = new string[8];
        public static bool[] IsCaConnected = new bool[8];

        public static bool SendPacket(SocketControl sc, bool bBarrier, string strPacket, int dwWaitTimeMs = 0)
        {
            string strChkSum;
            StringBuilder sbStrInfo = new StringBuilder();
            int nChkSum = GetChkSum(strPacket);
            strChkSum = nChkSum.ToString("X2");

            sbStrInfo.Clear();
            sbStrInfo.Append(Convert.ToChar(Definition.PACKET_START_SIGNAL));
            sbStrInfo.Append(strPacket);
            sbStrInfo.Append(strChkSum.ToUpper());
            sbStrInfo.Append(Convert.ToChar(Definition.PACKET_END_SIGNAL));

            // cmd index ?? ??
            int nIndex = int.Parse(sbStrInfo.ToString().Substring(5, 2), NumberStyles.HexNumber);		// get a cmd
            sc.bCmdList[nIndex] = false;

            sc.sendPacketString(sbStrInfo.ToString());

            Stopwatch sw = new Stopwatch();
            sw.Start();
            while (!sc.bCmdList[nIndex])
            {
                if (sw.ElapsedMilliseconds >= dwWaitTimeMs)
                {
                    sw.Stop();
                    return false;
                }
                Thread.Sleep(0);
            }
            sc.bSubCmdList[nIndex] = false;
            sw.Stop();
            return true;
        }
        private static int GetChkSum(string packet)
        {
            int nSum = 0;
            int nLen = packet.Length;

            for (int i = 0; i < nLen; i++)
            {
                nSum += packet.ElementAt(i);
            }

            nSum = nSum & 0xFF;

            return nSum;
        }

        public static void CA_Connect(ref bool[] bResult)
        {
            try
            {
                if (!loadProbe(ref bResult)) return;
                ZeroCal();
            }
            catch (Exception e)
            {
                MessageBox.Show("CA_Connect : " + e.Message);
            }
        }

        public static void CA_Close(int chNum = 99)
        {
            try
            {
                if (chNum == 99)
                    objCa200.DisconnectAll();
                else
                    objCa200.Disconnect(chNum);
            }
            catch (Exception e)
            {
                MessageBox.Show("CA_Close : " + e.Message);
            }
        }

        private static bool loadProbe(ref bool[] bResult)
        {
            bool bRet = false;
            CASDK2.CASDK2Discovery.SearchAllUSBDevices(ref pDeviceData);

            for (int ca = 0; ca < nPgNum; ca++)
            {
                if (pDeviceData[ca] != null)
                {
                    objCa200.SetConfiguration(ca + 1, "1", pDeviceData[ca].lPortNo, 38400, 0);
                    bRet = true;
                }
            }

            if (!bRet)
                return false;

            objCa200.get_Cas(ref objCas);
            
            for (int ca = 0; ca < nPgNum; ca++)
            {
                if (pDeviceData[ca] == null)
                    continue;
                objCas.get_Item(ca + 1, ref objCa[ca]);  //objCas에 저장된 프로브들의 정보를 각각의 objCa에 입력 함.

                objCa[ca].get_Probes(ref objProbes[ca]);
                objCa[ca].get_OutputProbes(ref objOutputProbes[ca]);
                objCa[ca].get_Memory(ref objMemory[ca]);
                objCa[ca].put_NegativeValue(0);
                objCa[ca].put_DisplayMode(0);

                objCa[ca].put_SyncMode(2);    //  0 : NTSC, 2 : EXT
                objCa[ca].put_AveragingMode(1); // 1 : FAST, 2 : LTD.AUTO 

                objProbes[ca].get_Item(1, ref objProbe[ca]);
                objOutputProbes[ca].AddAll();
                objOutputProbes[ca].get_Item(1, ref objProbe[ca]);

                bResult[ca] = true;
            }

            for (int ca = 0; ca < nPgNum; ca++)
            {
                if (pDeviceData[ca] != null && !bResult[ca])
                    return false;
            }

            return true;
        }

        private static void ZeroCal()
        {
            for (int ca = 0; ca < nPgNum; ca++)
            {
                if (pDeviceData[ca] == null)
                    continue;

                objCa[ca].CalZero();
                MessageBox.Show(string.Format("Zero-Cal ok CH{0}", ca + 1));
            }
        }
    }
}
