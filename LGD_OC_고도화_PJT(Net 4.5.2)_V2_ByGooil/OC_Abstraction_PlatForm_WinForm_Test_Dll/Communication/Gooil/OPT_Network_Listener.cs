using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OC_Abstraction_PlatForm_WinForm_Test_Dll.Communication.Gooil
{

    public interface ITargetModelCheckPass
    {
        void cmd_Run(byte[] inputStream, int Cnt, int nCmd, int chNumb, Barrier waitBarrier);
        object getNetworkReadVal();
        object getNetworkOTPVal();
        void initNetworkOTPVal();

    }

    class OPT_Network_Listener : ITargetModelCheckPass
    {
        private object networkReadVal;
        private object networkOTPVal;

        public object getNetworkReadVal()
        {
            return networkReadVal;
        }

        public object getNetworkOTPVal()
        {
            return networkOTPVal;
        }

        public void initNetworkOTPVal()
        {
            networkOTPVal = null;
        }

        public void cmd_Run(byte[] inputStream, int Cnt, int nCmd, int chNumb, Barrier waitBarrier)
        {
            byte[] recvData = new byte[Cnt];
            Array.Copy(inputStream, 0, recvData, 0, Cnt);
            networkReadVal = recvData;

            switch (nCmd)
            {
                case 0x33:
                    {//B1 OTP Read
                        string temp = Encoding.Default.GetString(inputStream, 11, 2);
                        if (temp == "00")
                        {
                            byte[] recvOtp = inputStream;
                            Array.Resize(ref recvOtp, Cnt);
                            networkOTPVal = recvOtp;
                        }
                        else if (temp == "30")  // register Read
                        {
                            networkReadVal = inputStream;
                            networkOTPVal = inputStream;
                            waitBarrier.SignalAndWait(3000);
                        }
                    }
                    break;
            }
        }
    
    }
}
