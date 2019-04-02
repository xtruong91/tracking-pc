using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnergyManagementSystem.Services
{
  /// <summary>
  /// This is a set of application configuration parameters. These parameters affect the way our particular Zigbee Application will behave.
  /// </summary>
  public class ZNPConfig
  {
    /** Maximum number of clusters available for binding */
    public const int MAX_BINDING_CLUSTERS = 32;
    //default values used when creating applicationConfigurations in Simple API or AFZDO
    public const int DEFAULT_ENDPOINT = 0xD7;
    public const int DEFAULT_PROFILE_ID = 0xD7D7;
    public const int ZNP_DEVICE_ID = 0x4567;
    public const int DEVICE_VERSION = 0x89;
    //Values for latencyRequested field of struct applicationConfiguration. Not used in Simple API.
    public const int LATENCY_NORMAL = 0;
    public const int LATENCY_FAST_BEACONS = 1;
    public const int LATENCY_SLOW_BEACONS = 2;


    private byte endpoint;
    private uint profileid;
    private uint deviceid;
    private byte deviceversion;
    private byte latencyrequested;
    private byte numberofbindinginputclusters;
    private uint[] bindinginputclusters = new uint[MAX_BINDING_CLUSTERS];
    private byte numberofbindingoutputclusters;
    private uint[] bindingoutputclusters = new uint[MAX_BINDING_CLUSTERS];

    /// <summary>
    /// The Zigbee Endpoint. Simple API only supports one endpoint. Must use the same value of endpoint for all devices in the network.
    /// </summary>
    public byte endPoint
    {
      get { return endpoint; }
      set { endpoint = value; }
    }

    /// <summary>
    /// The Application Profile ID as assigned by the Zigbee Association. Must use the same application profile for all devices in the network.
    /// </summary>
    public uint profileId
    {
      get { return profileid; }
      set { profileid = value; }
    }

    /// <summary>
    /// A user-defined device ID field. Not used in Simple API, but when using AFZDO API a remote device can query for this using the ZDO_SIMPLE_DESC_REQ command.
    /// </summary>
    public uint deviceId
    {
      get { return deviceid; }
      set { deviceid = value; }
    }

    /// <summary>
    /// A user-defined device ID field. Not used in Simple API, but when using AFZDO API a remote device can query for this using the ZDO_SIMPLE_DESC_REQ command.
    /// </summary>
    public byte deviceVersion
    {
      get { return deviceversion; }
      set { deviceversion = value; }
    }

    /// <summary>
    /// A very simple Quality of Service (QoS) setting. Must be LATENCY_NORMAL, LATENCY_FAST_BEACONS, or LATENCY_SLOW_BEACONS. Not used in Simple API.
    /// </summary>
    public byte latencyRequested
    {
      get { return latencyrequested; }
      set { latencyrequested = value; }
    }

    /// <summary>
    /// Number of Input Clusters for Binding. If not using binding then set to zero.
    /// </summary>
    public byte numberOfBindingInputClusters
    {
      get { return numberofbindinginputclusters; }
      set { numberofbindinginputclusters = value; }
    }

    /// <summary>
    /// List of Input Clusters for Binding. If not using binding then this does not apply. To allow another device to bind to this device, must use ZB_ALLOW_BIND on this device and must use ZB_BIND_DEVICE on the other device.
    /// </summary>
    public uint[] bindingInputClusters
    {
      get { return bindinginputclusters; }
      set { bindinginputclusters = value; }
    }

    /// <summary>
    /// Number of Output Clusters for Binding. If not using binding then set to zero.
    /// </summary>
    public byte numberOfBindingOutputClusters
    {
      get { return numberofbindingoutputclusters; }
      set { numberofbindingoutputclusters = value; }
    }

    /// <summary>
    /// List of Output Clusters for Binding. If not using binding then this does not apply. To bind to another device, that device must use ZB_ALLOW_BIND and this device must use ZB_BIND_DEVICE to create a binding. 
    /// </summary>
    public uint[] bindingOutputClusters
    {
      get { return bindingoutputclusters; }
      set { bindingoutputclusters = value; }
    }
  }
}
