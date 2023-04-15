using System;
using System.Threading.Tasks;
using NetworkFramework.Data;
using Unity.Networking.Transport.Relay;
using Unity.Services.Relay;
using UnityEngine;

namespace NetworkFramework.RelayCore
{
    public class RelayConnectorCore
    {
        public string JoinCode { get; private set; }
        public RelayServerData RelayServerData { get; private set; }

        public async Task<TaskResult> CreateRelay(int maxPlayers)
        {
            try
            {
                var allocation =
                    await RelayService.Instance.CreateAllocationAsync(maxPlayers - 1 == 0 ? 1 : maxPlayers - 1);
                JoinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
                RelayServerData = new RelayServerData(allocation, "dtls");
                Debug.Log("RELAY CREATE - " + JoinCode);
                return TaskResult.Ok;
            }
            catch (RelayServiceException e)
            {
                Debug.Log(e);
                return new TaskResult(false, e);
            }
        }

        public async Task<TaskResult> JoinRelay(string joinCode)
        {
            try
            {
                var allocationJoin = await RelayService.Instance.JoinAllocationAsync(joinCode);
                RelayServerData = new RelayServerData(allocationJoin, "dtls");
                Debug.Log("RELAY JOIN - " + JoinCode);
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