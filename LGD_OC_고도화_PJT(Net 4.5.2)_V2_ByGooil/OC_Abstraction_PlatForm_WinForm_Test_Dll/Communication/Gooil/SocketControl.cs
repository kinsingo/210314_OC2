using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OC_Abstraction_PlatForm_WinForm_Test_Dll.Communication.Gooil
{

    public class SocketControl
    {
        TcpClient clientSocket = null;
        NetworkStream sStream;
        public bool Pass_WakUp = false, Pass_Write = false, Pass_Complete = false;
        public bool[] bCmdList = new bool[Definition.CMD_MAX];
        public bool[] bSubCmdList = new bool[Definition.CMD_MAX];
        public int m_channelNumb = 0;
        public byte[] receiveData = new byte[4096];
        public bool bConnect = false;

        private static bool IsConnectionSuccessful = false;
        private static Exception socketexception;
        private static ManualResetEvent TimeoutObject = new ManualResetEvent(false);

        Thread receiveThread = null;

        public Barrier barrier_wait_for_recv { set; get; }

        private ITargetModelCheckPass chk_pass;

        public SocketControl()
        {
            chk_pass = new OPT_Network_Listener();//ObjectFactoryManager.getTargetCheckPass(SynchronizedSingleTon<GlobalSetting>.GetInstance().current_InspectionTarget);
        }

        public ITargetModelCheckPass getCheckPass()
        {
            return chk_pass;
        }

        public static TcpClient Connect(string ip, int port, int timeoutMSec)
        {
            TimeoutObject.Reset();
            socketexception = null;

            TcpClient tcpclient = new TcpClient();
            tcpclient.BeginConnect(ip, port, new AsyncCallback(CallBackMethod), tcpclient);


            if (TimeoutObject.WaitOne(timeoutMSec, false))
            {
                if (IsConnectionSuccessful)
                {
                    return tcpclient;
                }
                else
                {
                    throw socketexception;
                }
            }
            else
            {
                tcpclient.Close();
                throw new TimeoutException("TimeOut Exception");
            }
        }

        private static void CallBackMethod(IAsyncResult asyncresult)
        {
            try
            {
                IsConnectionSuccessful = false;

                TcpClient tcpclient = asyncresult.AsyncState as TcpClient;

                if (tcpclient.Client != null)
                {
                    tcpclient.EndConnect(asyncresult);

                    IsConnectionSuccessful = true;
                }
            }
            catch (Exception ex)
            {
                IsConnectionSuccessful = false;
                socketexception = ex;
            }
            finally
            {
                TimeoutObject.Set();
            }
        }

        public bool startClient(int pgNum, string strIp, int nPort)
        {
            m_channelNumb = pgNum + 1;
            if (clientSocket == null)
                clientSocket = new TcpClient();

            if (!clientSocket.Connected)
            {
                try
                {
                    clientSocket = Connect(strIp, nPort, 1000);
                    sStream = clientSocket.GetStream();
                }
                catch (Exception)
                {
                    return false;
                }
            }

            byte[] buffer = new byte[clientSocket.ReceiveBufferSize];

            clientSocket.Client.BeginReceive(receiveData, 0, receiveData.Length, SocketFlags.None, new AsyncCallback(OnReceiveData), clientSocket.Client);

            receiveThread = new Thread(new ThreadStart(isConnect));
            receiveThread.Start();

            //if (ConfigInfo.hFwDown_Form != IntPtr.Zero)
            //    Utils.PostMessage(ConfigInfo.hFwDown_Form, Definition.MSG_SOCKET_CONNECT, (uint)m_channelNumb, (uint)0);
            //if (ConfigInfo.hMain_Form != IntPtr.Zero)
            //    Utils.PostMessage(ConfigInfo.hMain_Form, Definition.MSG_SOCKET_CONNECT, IntPtr.Zero, IntPtr.Zero);

            bConnect = true;
            return true;
        }

        public void endClient()
        {
            if (sStream != null)
                sStream.Close();
            if (clientSocket != null)
            {
                clientSocket.Close();
                clientSocket = null;
            }

            bConnect = false;

            //Log.SaveLog(Definition.DEBUG_LOG, string.Format("[PG{0}] Socket Close!", m_channelNumb));
        }

        public bool IsConnected()
        {
            return bConnect;
        }

        public void sendPacketString(string sPacket)
        {
            try
            {
                byte[] outStream = Encoding.Default.GetBytes(sPacket);
                sStream.Write(outStream, 0, outStream.Length);
                sStream.Flush();
            }
            catch (Exception)
            {
            }
        }

        public void sendPacketByte(byte[] Packet)
        {
            try
            {
                sStream.Write(Packet, 0, Packet.Length);
                sStream.Flush();
            }
            catch (Exception)
            {
            }
        }

        public void sendPacketByte(byte[] Packet, int length)
        {
            try
            {
                sStream.Write(Packet, 0, length);
                sStream.Flush();
            }
            catch (Exception)
            {
            }
        }

        private void socketDisconnect()
        {
            //socket disconnect...
            //if (ConfigInfo.hFwDown_Form != IntPtr.Zero)
            //    Utils.PostMessage(ConfigInfo.hFwDown_Form, Definition.MSG_SOCKET_DISCONNECT, (uint)m_channelNumb, (uint)0);

            //try
            //{
            //    Utils.PostMessage(ConfigInfo.hMain_Form, Definition.MSG_SOCKET_DISCONNECT, (uint)m_channelNumb, 0);
            //}
            //catch (System.Exception ex)
            //{

            //}
        }

        private void isConnect()
        {
            try
            {
                while (clientSocket.Connected)
                {
                    Thread.Sleep(500);
                }
                socketDisconnect();
            }
            catch (Exception)
            { }
        }

        private void OnReceiveData(IAsyncResult result)
        {
            try
            {
                int readCount = clientSocket.Client.EndReceive(result);

                if (receiveData[readCount - 1] == 0x03)
                {
                    checkPass(receiveData, readCount);
                }

                clientSocket.Client.BeginReceive(receiveData, 0, receiveData.Length, SocketFlags.None, new AsyncCallback(OnReceiveData), clientSocket.Client);
            }
            catch (Exception)
            { }
        }

        private void checkPass(byte[] inputStream, int Cnt)
        {
            if (inputStream[0] != 0x02)
                return;

            if (inputStream[Cnt - 1] != 0x03)
                return;

            int nCmd = int.Parse(Encoding.Default.GetString(inputStream, 5, 2), NumberStyles.HexNumber);
            bCmdList[nCmd] = true;

            chk_pass.cmd_Run(inputStream, Cnt, nCmd, m_channelNumb, barrier_wait_for_recv);
        }
    }
}
