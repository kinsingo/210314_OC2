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
    public class DataProtocal
    {
        public IBusinessAPI api;
        public int channel_num;
        public RichTextBox richTextBox;

        public DataProtocal(IBusinessAPI _api, int _channel_num, RichTextBox _richTextBox)
        {
            api = _api;
            channel_num = _channel_num;
            richTextBox = _richTextBox;
        }
        public byte[] GetReadData(byte[] output)
        {
            int offset = Convert.ToInt32(output[0]);
            byte address = output[1];
            int amount = Convert.ToInt32(output[2]);
            return api.ReadData(address, amount, offset, channel_num);
        }
    }
}
