using System.Threading.Tasks;
using Unity.Services.Relay;
using UnityEngine;
using TaskStatus = NetworkFramework.Data.TaskStatus;

namespace NetworkFramework.RelayCore
{
    public class RelayConnectorCore
    {
        
        
        public async Task<TaskStatus> CreateRelay(int maxPlayers)
        {
            try
            {
                var allocation = await RelayService.Instance.CreateAllocationAsync(maxPlayers);
                string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
                return TaskStatus.Ok;
            }
            catch (RelayServiceException e)
            {
                Debug.Log(e);
                return new TaskStatus(false, e);
            }
        }
    }
}