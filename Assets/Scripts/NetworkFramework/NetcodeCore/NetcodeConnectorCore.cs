using System;
using NetworkFramework.Data.Scene;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using UnityEngine;
using TaskStatus = NetworkFramework.Data.TaskStatus;

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

        public TaskStatus LoadNetworkScene(SceneInfo sceneInfo)
        {
            try
            {
                NetworkManager.Singleton.SceneManager.LoadScene(sceneInfo.Name,
                    sceneInfo.LoadMode);
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