using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnergyManagementSystem.Services
{
  public partial class ZNPInterface
  {
    //Radio Device Information Property 
    public const int DIP_STATE = 0x00;
    public const int DIP_MAC_ADDRESS = 0x01;
    public const int DIP_SHORT_ADDRESS = 0x02;
    public const int DIP_PARENT_SHORT_ADDRESS = 0x03;
    public const int DIP_PARENT_MAC_ADDRESS = 0x04;
    public const int DIP_CHANNEL = 0x05;
    public const int DIP_PANID = 0x06;
    public const int DIP_EXTENDED_PANID = 0x07;
    public const int MAX_DEVICE_INFORMATION_PROPERTY = 0x07;

    public const int DIP_MAC_ADDRESS_LENGTH = 8;
    public const int DIP_SHORT_ADDRESS_LENGTH = 2;
    public const int DIP_PARENT_SHORT_ADDRESS_LENGTH = 2;
    public const int DIP_PARENT_MAC_ADDRESS_LENGTH = 8;
    public const int DIP_CHANNEL_LENGTH = 1;
    public const int DIP_PANID_LENGTH = 2;
    public const int DIP_EXTENDED_PANID_LENGTH = 8;

    public const int ZB_WRITE_CONFIGURATION_LEN = 2;  //excluding payload length
    public const int ZB_READ_CONFIGURATION_PAYLOAD_LEN = 1;

    public const int SRSP_DIP_VALUE_FIELD = (SRSP_HEADER_SIZE + 1); //index in znpBuf[] of the start of the Device Information Property field. LSB is first.

    //WRITE CONFIGURATION OPTIONS 
    public const int ZCD_NV_STARTUP_OPTION = 0x03;
    public const int ZCD_NV_STARTUP_OPTION_LEN = 1;
    public const int ZCD_NV_LOGICAL_TYPE = 0x87;
    public const int ZCD_NV_LOGICAL_TYPE_LEN = 1;
    public const int ZCD_NV_ZDO_DIRECT_CB = 0x8F;
    public const int ZCD_NV_ZDO_DIRECT_CB_LEN = 1;
    public const int ZCD_NV_POLL_RATE = 0x25;
    public const int ZCD_NV_POLL_RATE_LEN = 2;

    //For Security:
    public const int ZCD_NV_PRECFGKEY = 0x62;
    public const int ZCD_NV_PRECFGKEY_LEN = 16;
    public const int ZCD_NV_PRECFGKEYS_ENABLE = 0x63;
    public const int ZCD_NV_PRECFGKEYS_ENABLE_LEN = 1;
    public const int ZCD_NV_SECURITY_MODE = 0x64;
    public const int ZCD_NV_SECURITY_MODE_LEN = 1;

    //NETWORK SPECIFIC CONFIGURATION PARAMETERS 
    public const int ZCD_NV_PANID = 0x83;
    public const int ZCD_NV_PANID_LEN = 2;
    public const int ZCD_NV_CHANLIST = 0x84;
    public const int ZCD_NV_CHANLIST_LEN = 4;

    //STARTUP OPTIONS 
    public const int STARTOPT_CLEAR_CONFIG = 0x01;
    public const int STARTOPT_CLEAR_STATE = 0x02;
    public const int STARTOPT_AUTO = 0x04;

    //used in ZB_SEND_DATA_REQUEST & AF_DATA_REQUEST
    public const int MAC_ACK = 0x00;    //Require Acknowledgement from next device on route
    public const int AF_APS_ACK = 0x10;    //Require Acknowledgement from final destination (if using AFZDO)
    public const int SAPI_APS_ACK = 0x01;    //Require Acknowledgement from final destination (if using Simple API)
    public const int DEFAULT_RADIUS = 0x0F;    //Maximum number of hops to get to destination
    public const int MAXIMUM_PAYLOAD_LENGTH = 66;      //84 bytes without security, 66 bytes with security
    public const int ALL_DEVICES = 0xFFFF;
    public const int ALL_ROUTERS_AND_COORDINATORS = 0xFFFC;


    //Security Options
    public const int SECURITY_MODE_OFF = 0;
    public const int SECURITY_MODE_PRECONFIGURED_KEYS = 1;
    public const int SECURITY_MODE_COORD_DIST_KEYS = 2;

    //Callbacks
    public const int CALLBACKS_DISABLED = 0;
    public const int CALLBACKS_ENABLED = 1;

    //used in PERMIT_JOINING_REQUEST methods
    public const int NO_TIMEOUT = 0xFF;

    //used in setting PAN ID
    public const int ANY_PAN = 0xFFFF;
    public const int MAX_PANID = 0xFFF7;
    public static  bool IS_VALID_PANID(int id) { return (id <= MAX_PANID); }


    //ZDO States - returned from get Device Information Property(DIP_STATE)
    public const int DEV_HOLD = 0;
    public const int DEV_INIT = 1;
    public const int DEV_NWK_DISC = 2;
    public const int DEV_NWK_JOINING = 3;
    public const int DEV_NWK_REJOIN = 4;
    public const int DEV_END_DEVICE_UNAUTH = 5;
    public const int DEV_END_DEVICE = 6;
    public const int DEV_ROUTER = 7;
    public const int DEV_COORD_STARTING = 8;
    public const int DEV_ZB_COORD = 9;
    public const int DEV_NWK_ORPHAN = 10;

    //used in setting CHAN_LIST
    //see ZDO_MGMT_NWK_UPDATE_REQ
    public const uint ANY_CHANNEL = 0x07FFF800;  //Channel 11-26 bitmask
    public const uint MIN_CHANNEL = 0x00000800;
    public const uint MAX_CHANNEL = ANY_CHANNEL;
    public static bool IS_VALID_CHANNEL(int channel) { return ((channel >= 11) && (channel <= 26)); }

    //GPIO Pin read/write
    //Operations:
    public const int GPIO_SET_DIRECTION = 0x00;
    public const int GPIO_SET_INPUT_MODE = 0x01;
    public const int GPIO_SET = 0x02;
    public const int GPIO_CLEAR = 0x03;
    public const int GPIO_TOGGLE = 0x04;
    public const int GPIO_READ = 0x05;

    public const int ALL_GPIO_PINS = 0x0F;  //GPIO 0-3

    //options for GPIO_SET_INPUT_MODE
    public const int GPIO_INPUT_MODE_ALL_PULL_DOWNS = 0xF0;
    public const int GPIO_INPUT_MODE_ALL_PULL_UPS = 0x00;
    public const int GPIO_INPUT_MODE_ALL_TRI_STATE = 0x0F;
    public const int GPIO_DIRECTION_ALL_INPUTS = 0x00;

    public const int MIN_NV_ITEM = 1;
    public const int MAX_NV_ITEM = 7;
    //2 Bytes each:
    public const int NV_ITEM_1 = 0x0F01;
    public const int NV_ITEM_2 = 0x0F02;
    public const int NV_ITEM_3 = 0x0F03;
    public const int NV_ITEM_4 = 0x0F04;
    //16 Bytes each:
    public const int NV_ITEM_5 = 0x0F05;
    public const int NV_ITEM_6 = 0x0F06;

    // Define 2 bytes for RF test parameters
    public const int ZNP_NV_RF_TEST_PARMS = 0x0F07;


    public const int SYS_RESET_PAYLOAD_LEN = 0;
    public const int SYS_VERSION_PAYLOAD_LEN = 0;
    public const int SYS_RANDOM_PAYLOAD_LEN = 0;
    public const int STACK_TX_POWER = 0;
    public const int SYS_STACK_TUNE_PAYLOAD_LEN = 2;
    public const int SYS_GPIO_PAYLOAD_LEN = 2;
    public const int SYS_OSAL_NV_WRITE_PAYLOAD_LEN = 4;
    public const int SYS_OSAL_NV_READ_PAYLOAD_LEN = 3;
    public const int ZB_GET_DEVICE_INFO_PAYLOAD_LEN = 0x01;

    public const int ZNP_SUCCESS = 0x00;
    public const int ZNP_FAIL = -1;

    //Received in SRSP message
    public const int SRSP_STATUS_SUCCESS = 0x00;
    public const int SRSP_TIMEOUT = -11;
    public const int SRSP_FRAMEERROR = -12;
    public const int SRSP_CMDERROR = -13;
    public const int SRSP_UNKNOWERROR = -14;

    //SRSP timeout in ms
    public const int SRSP_TIMEOUT_MS = 100;

    public const int SRSP_HEADER_SIZE = 3;
    public const int SRSP_BUFFER_SIZE = 20;

    public const int SRSP_PAYLOAD_START = 3;
    public const int SRSP_LENGTH_FIELD = 0;
    public const int SRSP_CMD_LSB_FIELD = 2;
    public const int SRSP_CMD_MSB_FIELD = 1;

    //Logical Types
    public const int COORDINATOR = 0x00;
    public const int ROUTER = 0x01;
    public const int END_DEVICE = 0x02;


    //configuration parameter from the ZNP.
    public const int LENGTH_OF_LARGEST_CONFIG_PARAMETER = 17; //ZCD_NV_USERDESC is largest
    public const int STATUS_FIELD = SRSP_PAYLOAD_START;
    public const int CONFIG_ID_FIELD = SRSP_PAYLOAD_START + 1;
    public const int LENGTH_FIELD = SRSP_PAYLOAD_START + 2;
    public const int START_OF_VALUE_FIELD = SRSP_PAYLOAD_START + 3;

    //expected Response message types
    public const int NO_RESPONSE_EXPECTED = 0x00;

    public const int ZNP_NV_RF_TEST_HEADER_LEN = 4;  //length of RF test parameters header
    public const int ZNP_NV_RF_TEST_PARMS_LEN = 4;  //length of RF test parameters 


    //simple api
    public const int ZB_APP_REGISTER_REQUEST_PAYLOAD_LEN = 9;
    public const int ZB_START_CONFIRM_TIMEOUT = 5;
    public const int ZB_APP_START_REQUEST_PAYLOAD_LEN = 0;
    public const int ZB_PERMIT_JOINING_REQUEST_PAYLOAD_LEN = 3;
    public const int ZB_SEND_DATA_CONFIRM_TIMEOUT = 5;
    public const int ZB_SEND_DATA_REQUEST_PAYLOAD_LEN = 8;

    // af api
    public const int AF_REGISTER_PAYLOAD_LEN = 9;
    public const int ZDO_STARTUP_FROM_APP_PAYLOAD_LEN = 1;
    public const int NO_START_DELAY = 0;
    public const int AF_DATA_CONFIRM_TIMEOUT = 2;
    public const int AF_DATA_REQUEST_PAYLOAD_LEN = 10;
    public const int ZDO_IEEE_ADDR_REQ_PAYLOAD_LEN = 4;
    public const int SINGLE_DEVICE_RESPONSE = 0;
    public const int INCLUDE_ASSOCIATED_DEVICES = 1;
    public const int ZDO_NWK_ADDR_REQ_PAYLOAD_LEN = 10;

    // UTIL
    public const int UTIL_ADDRMGR_NWK_ADDR_LOOKUP_PAYLOAD_LEN = 2;

    //RF TEST MODES
    public const int RF_TEST_RECEIVE = 1;
    public const int RF_TEST_UNMODULATED = 2;
    public const int RF_TEST_MODULATED = 3;
    public const int RF_TEST_NONE = 0xFF;  //NOTE: this is for the application only, don't send this to the ZNP

    //From Table 1 in cc2530 datasheet, p 21
    public const int RF_OUTPUT_POWER_PLUS_4_5_DBM = 0xF5;  //+4.5dBm
    public const int RF_OUTPUT_POWER_PLUS_2_5_DBM = 0xE5;  //+2.5dBm
    public const int RF_OUTPUT_POWER_PLUS_1_0_DBM = 0xD5;  //+1.0dBm
    public const int RF_OUTPUT_POWER_MINUS_0_5_DBM = 0xC5;  //-0.5dBm
    public const int RF_OUTPUT_POWER_MINUS_1_5_DBM = 0xB5;  //-1.5dBm
    public const int RF_OUTPUT_POWER_MINUS_3_0_DBM = 0xA5;  //-3.0dBm
    public const int RF_OUTPUT_POWER_MINUS_10_0_DBM = 0x65;  //-10.0dBm

    //used for RF Test Modes:
    public const int RF_TEST_CHANNEL_MIN = 11;
    public const int RF_TEST_CHANNEL_MAX = 26;

    /// <summary>
    /// SerialPort Receive Interrupt.If received 4 bytes,then this event happens.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// 
    private const byte SOP_STATE = 0x00;
    private const byte CMD_STATE1 = 0x01;
    private const byte CMD_STATE2 = 0x02;
    private const byte LEN_STATE = 0x03;
    private const byte DATA_STATE = 0x04;
    private const byte FCS_STATE = 0x05;

    private byte ReceivedState = SOP_STATE;
    private byte[] CMD_Token = new byte[2];
    private byte LEN_Token;
    private byte FCS_Token;
    private byte tempDataLen;
    private byte[] RxMsg = new byte[100];

  }
}
