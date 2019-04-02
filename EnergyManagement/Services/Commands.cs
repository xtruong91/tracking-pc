using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnergyManagementSystem.Services
{
  /// <summary>
  /// CC2530-ZNP software command interface
  /// </summary>
  public class Commands
  {
    #region SYS Interface
    public const int ZB_WRITE_CONFIGURATION = 0x2605;
    public const int ZB_READ_CONFIGURATION = 0x2604;
    public const int SYS_GPIO = 0x210E;
    public const int SYS_TEST_RF = 0x4140;
    public const int SYS_VERSION = 0x2102;
    public const int SYS_RANDOM = 0x210C;
    public const int SYS_OSAL_NV_READ = 0x2108;
    public const int SYS_OSAL_NV_WRITE = 0x2109;
    public const int SYS_RESET_IND = 0x4180;
    public const int SYS_STACK_TUNE = 0x210F;
    public const int SYS_RESET_REQ = 0x4100;

    #endregion

    #region Simple API commands

    public const int ZB_APP_REGISTER_REQUEST = 0x260A;
    public const int ZB_APP_START_REQUEST = 0x2600;
    public const int ZB_SEND_DATA_REQUEST = 0x2603;
    public const int ZB_SEND_DATA_CONFIRM = 0x4683;
    public const int ZB_GET_DEVICE_INFO = 0x2606;
    public const int ZB_FIND_DEVICE_REQUEST = 0x2607;
    public const int ZB_FIND_DEVICE_CONFIRM = 0x4685;
    public const int ZB_PERMIT_JOINING_REQUEST = 0x2608;
    public const int ZB_START_CONFIRM = 0x4680; //will receive this asynchronously
    public const int ZB_RECEIVE_DATA_INDICATION = 0x4687; //will receive this asynchronously

    #endregion

    #region AFZDO commands
    public const int AF_REGISTER = 0x2400;
    public const int AF_DATA_REQUEST = 0x2401;
    public const int AF_DATA_CONFIRM = 0x4480;
    public const int AF_INCOMING_MSG = 0x4481; //will receive this asynchronously
    public const int ZDO_STARTUP_FROM_APP = 0x2540;
    public const int ZDO_IEEE_ADDR_REQ = 0x2501;
    public const int ZDO_IEEE_ADDR_RSP = 0x4581;
    public const int ZDO_NWK_ADDR_REQ = 0x2500;
    public const int ZDO_NWK_ADDR_RSP = 0x4580;

    public const int ZDO_STATE_CHANGE_IND = 0x45C0; //will receive this asynchronously  Old version of ZNP = 0x45C1
    public const int ZDO_END_DEVICE_ANNCE_IND = 0x45C1; //will receive this asynchronously  Old version of ZNP = 0x4593

    #endregion

    #region UTIL commands
    public const int UTIL_ADDRMGR_NWK_ADDR_LOOKUP = 0x2741;

    #endregion

    /// <summary>
    /// get ZNP command name
    /// </summary>
    /// <param name="command">command</param>
    /// <returns>command name</returns>
    public string getCommandName(int command)
    {
      switch (command)
      {
        //SYS Interface        
        case ZB_WRITE_CONFIGURATION: return ("ZB_WRITE_CONFIGURATION");
        case ZB_READ_CONFIGURATION: return ("ZB_READ_CONFIGURATION");
        case SYS_GPIO: return ("SYS_GPIO");
        case SYS_TEST_RF: return ("SYS_TEST_RF");
        case SYS_VERSION: return ("SYS_VERSION");
        case SYS_OSAL_NV_READ: return ("SYS_OSAL_NV_READ");
        case SYS_OSAL_NV_WRITE: return ("SYS_OSAL_NV_WRITE");
        case SYS_RESET_IND: return ("SYS_RESET_IND");

        //Simple API commands
        case ZB_APP_REGISTER_REQUEST: return ("ZB_APP_REGISTER_REQUEST");
        case ZB_APP_START_REQUEST: return ("ZB_APP_START_REQUEST");
        case ZB_SEND_DATA_REQUEST: return ("ZB_SEND_DATA_REQUEST");
        case ZB_SEND_DATA_CONFIRM: return ("ZB_SEND_DATA_CONFIRM");
        case ZB_GET_DEVICE_INFO: return ("ZB_GET_DEVICE_INFO");
        case ZB_FIND_DEVICE_REQUEST: return ("ZB_FIND_DEVICE_REQUEST");
        case ZB_FIND_DEVICE_CONFIRM: return ("ZB_FIND_DEVICE_CONFIRM");
        case ZB_PERMIT_JOINING_REQUEST: return ("ZB_PERMIT_JOINING_REQUEST");
        case ZB_START_CONFIRM: return ("ZB_START_CONFIRM");
        case ZB_RECEIVE_DATA_INDICATION: return ("ZB_RECEIVE_DATA_INDICATION");

        //AF/ZDO commands:    
        case AF_REGISTER: return ("AF_REGISTER");
        case AF_DATA_REQUEST: return ("AF_DATA_REQUEST");
        case AF_DATA_CONFIRM: return ("AF_DATA_CONFIRM");
        case AF_INCOMING_MSG: return ("AF_INCOMING_MSG");
        case ZDO_STARTUP_FROM_APP: return ("ZDO_STARTUP_FROM_APP");
        case ZDO_IEEE_ADDR_REQ: return ("ZDO_IEEE_ADDR_REQ");
        case ZDO_IEEE_ADDR_RSP: return ("ZDO_IEEE_ADDR_RSP");
        case ZDO_NWK_ADDR_REQ: return ("ZDO_NWK_ADDR_REQ");
        case ZDO_NWK_ADDR_RSP: return ("ZDO_NWK_ADDR_RSP");
        case ZDO_END_DEVICE_ANNCE_IND: return ("ZDO_END_DEVICE_ANNCE_IND");

        default: return ("UNKNOWN");
      }
    }
  }
}
