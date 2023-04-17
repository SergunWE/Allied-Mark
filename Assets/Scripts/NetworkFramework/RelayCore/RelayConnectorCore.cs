using System;
using System.Threading.Tasks;
using NetworkFramework.Data;
using Unity.Networking.Transport.Relay;
using Unity.Services.Relay;
using UnityEngine;

namespace NetworkFramework.RelayCore
{
    /// <summary>
    /// Relay server management class
    /// </summary>
    public class RelayConnectorCore
    {
        /// <summary>
        /// Relay server connection code
        /// </summary>
        public string JoinCode { get; private set; }
        
        /// <summary>
        /// Relay server information
        /// </summary>
        public RelayServerData RelayServerData { get; private set; }

        /// <summary>
        /// Creating a Relay Server. Only the lobby owner can create a server
        /// </summary>
        /// <param name="maxPlayers">Maximum number of players</param>
        /// <returns>The task with her status</returns>
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

        /// <summary>
        /// Connecting to a Relay server
        /// </summary>
        /// <param name="joinCode">Connection code</param>
        /// <returns>The task with her status</returns>
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