using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OC_Abstraction_PlatForm_WinForm_Test_Dll.Communication
{
    class Definition
    {
        // WND MESSAGE
        //public const int WM_KEYDOWN = 0x100;
        //public const int WM_USER = 0x0400;
        //public const int MSG_SOCKET_CONNECT = WM_USER + 1;
        //public const int MSG_SOCKET_DISCONNECT = WM_USER + 2;

        //public const int MSG_RECEIVEDATA = WM_USER + 101;
        //public const int MSG_POWER_SENSING_CH1 = WM_USER + 102;
        //public const int MSG_POWER_SENSING_CH2 = WM_USER + 103;
        //public const int MSG_OUTPUT_DATA = WM_USER + 104;
        //public const int MSG_COMMAND = WM_USER + 105;
        //public const int MSG_SEQ_INFO = WM_USER + 106;
        //public const int MSG_MODEL_CHANGE_EVENT = WM_USER + 107;
        //public const int MSG_MODEL_CHANGE_EVENT_END = WM_USER + 108;
        //public const int MSG_OPTIC_MEASURE_OUTPUT = WM_USER + 109;
        //public const int MSG_OPTIC_WRITE_COUNT = WM_USER + 110;
        //public const int MSG_OPTIC_MATCHING_RESULT = WM_USER + 111;
        //public const int MSG_OPTIC_MEASURE_RESULT = WM_USER + 112;
        //public const int MSG_UI_INIT = WM_USER + 113;
        //public const int MSG_MEASURE_VERIFY = WM_USER + 114;
        //public const int MSG_DIO_UP = WM_USER + 115;
        //public const int MSG_DIO_DOWN = WM_USER + 116;
        //public const int MSG_GMES_NG = WM_USER + 117;
        //public const int MSG_OPTIC_INFO = WM_USER + 118;
        //public const int MSG_SET_BCRCODE = WM_USER + 119;
        //public const int MSG_SET_SERIAL = WM_USER + 120;
        //public const int MSG_SET_TOUCH_WR = WM_USER + 122;
        //public const int MSG_LISTVIEW_INFO_NOTIFY = WM_USER + 123;
        //public const int MSG_WM_END = WM_USER + 124;
        //public const int MSG_DSC_RESULT = WM_USER + 125;

        public const int DEBUG_LOG = 0;
        public const int RELEASE_LOG = 1;

        public const int STATION_NUMB = 2;
        public const int PG_MAX = 8;    //  A4 FT : 2ch, A4 광학 : 8ch
        public const int CMD_MAX = (0xff) + 1;
        public const int SUB_CMD_MAX = (0x25) + 1;

        public const int PACKET_START_SIGNAL = 0x02;
        public const int PACKET_END_SIGNAL = 0x03;
        public const string STR_PC_TO_PG_CODE = "A1A3";
        public const string STR_PC_TO_PWR_CODE = "A1A5";
        public const string STR_PC_TO_IF_CODE = "A1A6";

        public const string STR_START_LCD_INFO = "_LCDINFO_";
        public const string STR_END_LCD_INFO = "#endif";
        public const string STR_COMMENT = "//";
        public const string STR_END_OF_BIN_FILE = ":#end_Script_code";

        public const string STR_M_LCD_IF_MODE = "m_LCD_IF_MODE";
        public const string STR_M_IO_VOLT_LEVEL = "m_IO_VOLT_LEVEL";
        public const string STR_M_DOTCLK = "m_DOTCLK";
        public const string STR_M_RGB_H_SIZE = "m_RGB_H_SIZE";
        public const string STR_M_RGB_V_SIZE = "m_RGB_V_SIZE";
        public const string STR_M_HSYNC_WIDTH = "m_HSYNC_WIDTH";
        public const string STR_M_VSYNC_WIDTH = "m_VSYNC_WIDTH";
        public const string STR_M_H_BACKPORCH = "m_H_BACKPORCH";
        public const string STR_M_V_BACKPORCH = "m_V_BACKPORCH";
        public const string STR_M_H_FRONTPORCH = "m_H_FRONTPORCH";
        public const string STR_M_V_FRONTPORCH = "m_V_FRONTPORCH";
        public const string STR_M_DCLK_POLARITY = "m_DCLK_POLARITY";
        public const string STR_M_H_POLARITY = "m_H_POLARITY";
        public const string STR_M_V_POLARITY = "m_V_POLARITY";
        public const string STR_M_DE = "m_DE";
        public const string STR_M_RGB_MODE = "m_RGB_MODE";
        public const string STR_M_BITS_SWAP = "m_BITS_SWAP";

        public const string STR_VOLTAGE_BAT = "F_VBAT";
        public const string STR_VOLTAGE_LCD = "F_VLCD";
        public const string STR_VOLTAGE_EXT = "F_VEXT";
        public const string STR_VOLTAGE_CC = "F_VCC";
        public const string STR_VOLTAGE_BL = "F_VBL";

        public const string STR_CURRENT_BAT = "F_IBAT";
        public const string STR_CURRENT_LCD = "F_ILCD";
        public const string STR_CURRENT_EXT = "F_IEXT";
        public const string STR_CURRENT_CC = "F_ICC";
        public const string STR_CURRENT_BL = "F_IBL";

        public const string STR_MODEL_ADDR = "MODEL_ADDR";
        public const string STR_MODEL_NAME = "MODEL_NAME";

        public const string STR_PG_VERSION_CMD_FC = "FC";
        public const string STR_PG_VERSION_CMD_FE = "FE";
        public const string STR_LCM_RESET_CMD = "30";
        public const string STR_LCM_RESET_CMD_STR = "25200100";
        public const string STR_BUZZER_CMD_EB = "EB";

        // INI
        public const string STR_INI_START_UP = "SMStartUp.Ini";
        public const string STR_INI_BMP_INFO = "BmpInfo.Ini";
        public const string STR_INI_POWER_LABEL = "PowerLabel.Ini";
        public const string STR_INI_WRGB_SPEC = "WRGB_Spec.Ini";
        public const string STR_INI_CA310 = "CA310.Ini";

        public const string STR_INI_START_UP_LIST = "START_UP";
        public const string STR_INI_BMP_INFO_INDEX_LIST = "INDEX_LIST";
        public const string STR_INI_POWER_LABEL_LIST = "LABEL_LIST";
        public static string[] STR_INI_POWER_LABEL_DATA = new string[] { "DATA_1", "DATA_2", "DATA_3", "DATA_4", "DATA_5", "DATA_6", "DATA_7", "DATA_8", "DATA_9" };


        public const string STR_INI_MODEL_NAME = "MODEL_NAME";
        public const string STR_INI_FORM_INFO = "FORM_INFO";

        public const string STR_GMES_SITE = "SITE";
        public const string STR_COMBI_CODE = "COMBI_CODE";
        public const string STR_COMBI_INFO = "COMBI_INFO";

        // LOCATION
        public const string STR_BMP_FILE_PATH = "\\BMP\\";
        public const string STR_HEX_FILE_PATH = "\\FW\\";
        public const string STR_BIN_FILE_PATH = "\\BIN\\";
        public const string STR_PAT_FILE_PATH = "\\PATTERN\\";
        public const string STR_INI_FILE_PATH = "\\INI\\";
        public const string STR_GMES_FILE_PATH = "INI\\GMES.INI";
        public const string STR_EAS_FILE_PATH = "INI\\EAS.INI";

        // value of power measure position
        public const int N_POWER_MEASURE_ERR_NAME_POS = 100;
        public const int N_POWER_MEASURE_ERR_RESULT_POS = 101;
        public const int N_POWER_MEASURE_ERR_RANGE_POS = 102;
        public const int N_POWER_MEASURE_ERR_VALUE_POS = 103;
        public const int N_POWER_MEASURE_ERR_VALUE_LENGTH = 6;
        public const int N_POWER_MEASURE_ERR_INT_POS = N_POWER_MEASURE_ERR_VALUE_POS + N_POWER_MEASURE_ERR_VALUE_LENGTH;

        public const int N_PACKET_WAIT_COUNT = 3500;
        public const int N_PACKET_MIN_WAIT_COUNT = 300;
        public const int N_USE_BMP_FILE_MAX = 15;

        public const int N_FW_WAKE_WAIT_COUNT = 2000;
        public const int N_FW_DOWN_WAIT_COUNT = 5000;
        public const int N_FW_DONE_WAIT_COUNT = 10000;
        public const int N_PACKET_CURRENT_CALIBRATION_WAIT_COUNT = 1500;

        public const int N_PACKET_DATA_SIZE = 2048;

        public const int N_BUZZER_ON = 1;
        public const int N_BUZZER_OFF = 0;
        public const int CA310_AVERAGING_SLOW = 0;
        public const int CA310_AVERAGING_FAST = 1;
        public const int CA310_AVERAGING_AUTO = 2;
    }

    public struct ST_XYLv
    {
        public float Lv;
        public float X;
        public float Y;

        public void Init()
        {
            Lv = 0;
            X = 0;
            Y = 0;
        }
    }
}
