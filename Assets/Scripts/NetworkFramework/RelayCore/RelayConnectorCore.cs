using System;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Relay;
using UnityEngine;
using TaskStatus = NetworkFramework.Data.TaskStatus;

namespace NetworkFramework.RelayCore
{
    public class RelayConnectorCore
    {
        public string JoinCode { get; private set; }
        public RelayServerData RelayServerData { get; private set; }

        public async Task<TaskStatus> CreateRelay(int maxPlayers)
        {
            try
            {
                var allocation = await RelayService.Instance.CreateAllocationAsync(maxPlayers);
                JoinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
                RelayServerData = new RelayServerData(allocation, "dtls");
                
                //NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(RelayServerData);
                //NetworkManager.Singleton.StartHost();
                Debug.Log("RELAY CREATE - " + JoinCode);
                return TaskStatus.Ok;
            }
            catch (RelayServiceException e)
            {
                Debug.Log(e);
                return new TaskStatus(false, e);
            }
        }

        public async Task<TaskStatus> JoinRelay(string joinCode)
        {
            try
            {
                var allocationJoin = await RelayService.Instance.JoinAllocationAsync(joinCode);
                RelayServerData = new RelayServerData(allocationJoin, "dtls");
                //NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(RelayServerData);
                //NetworkManager.Singleton.StartClient();
                Debug.Log("RELAY JOIN - " + JoinCode);
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