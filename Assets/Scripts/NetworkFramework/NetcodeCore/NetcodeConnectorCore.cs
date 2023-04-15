using System;
using NetworkFramework.Data;
using NetworkFramework.Data.Scene;
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

        public TaskResult StartHost(RelayServerData relayServerData)
        {
            try
            {
                NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
                NetworkManager.Singleton.StartHost();
                return TaskResult.Ok;
            }
            catch (Exception e)
            {
                Debug.Log(e);
                return new TaskResult(false, e);
            }
        }

        public TaskResult StartClient(RelayServerData relayServerData)
        {
            try
            {
                NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
                NetworkManager.Singleton.StartClient();
                return TaskResult.Ok;
            }
            catch (Exception e)
            {
                Debug.Log(e);
                return new TaskResult(false, e);
            }
        }

        public TaskResult LoadNetworkScene(SceneInfo sceneInfo)
        {
            try
            {
                NetworkManager.Singleton.SceneManager.LoadScene(sceneInfo.Name,
                    sceneInfo.LoadMode);
                return TaskResult.Ok;
            }
            catch (Exception e)
            {
                Debug.Log(e);
                return new TaskResult(false, e);
            }
        }
    }
}