using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LGD_OC_AstractPlatForm.CommonAPI;
using System.Windows.Forms;
using System.Threading;

namespace OC_Abstraction_PlatForm_WinForm_Test_Dll
{
    class Test
    {
        RichTextBox richTextBox;

        private static object syncObj = new object();
        public Test(RichTextBox _richTextBox)
        {
            richTextBox = _richTextBox;
        }
        public void TestThread()
        {
            for (int i = 0; i < 100; i++)
            {
                richTextBox.Invoke(new DisplayCountDelegate(DisplayCount),i);
            }
        }

        delegate void DisplayCountDelegate(int i);

        void DisplayCount(int i)
        {
            
            richTextBox.AppendText(i.ToString() + "\n");
            Thread.Sleep(100);
            
        }


    }
}

