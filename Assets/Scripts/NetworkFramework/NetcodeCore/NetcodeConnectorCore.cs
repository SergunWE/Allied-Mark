using System;
using NetworkFramework.Data;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using UnityEngine;

namespace NetworkFramework.NetcodeCore
{
    public class NetcodeConnectorCore
    {
        public NetcodeConnectorCore()
        {
            NetworkManager.Singleton.SetSingleton();
        }
        
        public TaskStatus StartHost(RelayServerData relayServerData)
        {
            try
            {
                NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
                NetworkManager.Singleton.StartHost();
                return TaskStatus.Ok;
            }
            catch (Exception e)
            {
                Debug.Log(e);
                return new TaskStatus(false, e);
            }
        }

        public TaskStatus StartClient(RelayServerData relayServerData)
        {
            try
            {
                NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
                NetworkManager.Singleton.StartClient();
                return TaskStatus.Ok;
            }
            catch (Exception e)
            {
                Debug.Log(e);
                return new TaskStatus(false, e);
            }
        }
    }
}