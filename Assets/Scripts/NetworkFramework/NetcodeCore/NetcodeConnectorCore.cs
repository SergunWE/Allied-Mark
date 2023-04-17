using System;
using NetworkFramework.Data;
using NetworkFramework.Data.Scene;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using UnityEngine;

namespace NetworkFramework.NetcodeCore
{
    /// <summary>
    /// Network connection class
    /// </summary>
    public class NetcodeConnectorCore
    {
        public NetcodeConnectorCore()
        {
            NetworkManager.Singleton.SetSingleton();
        }

        /// <summary>
        /// Running the host
        /// </summary>
        /// <param name="relayServerData">Relay server information</param>
        /// <returns>Status of the task performed</returns>
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

        /// <summary>
        /// Running the client
        /// </summary>
        /// <param name="relayServerData">Relay server information</param>
        /// <returns>Status of the task performed</returns>
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

        /// <summary>
        /// Loading a new scene for all clients
        /// </summary>
        /// <param name="sceneInfo">Scene information <see cref="SceneInfo"/>></param>
        /// <returns>Status of the task performed</returns>
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