using System;
using System.Threading;
using System.Threading.Tasks;
using NetworkFramework.Data;
using Unity.Services.Authentication;
using Unity.Services.Lobbies;
using UnityEngine;

namespace NetworkFramework.LobbyCore
{
    /// <summary>
    /// A class for updating lobby data
    /// </summary>
    public class LobbyRefresherCore : LobbyCore
    {
        /// <summary>
        /// Data update event
        /// </summary>
        public event Action OnLobbyUpdated;

        /// <summary>
        /// Heartbeat Lobby Send Thread
        /// </summary>
        private Thread _heartbeatLobbyThread;
        /// <summary>
        /// Lobby update thread
        /// </summary>
        private Thread _refreshLobbyThread;

        
        /// <summary>
        /// Starting a heartbeat sending thread
        /// </summary>
        public void StartHeartbeat()
        {
            if(!PlayerIsHost) return;
            _heartbeatLobbyThread?.Abort();
            _heartbeatLobbyThread = new Thread(HeartbeatLobbyThread);
            _heartbeatLobbyThread.Start();
        }

        /// <summary>
        /// Starting the lobby update thread
        /// </summary>
        public void StartRefresh()
        {
            _refreshLobbyThread?.Abort();
            _refreshLobbyThread = new Thread(RefreshLobbyThread);
            _refreshLobbyThread.Start();
        }

        /// <summary>
        /// Function to update lobby data
        /// <remarks>The result is in the class <see cref="LobbyData"/></remarks>
        /// </summary>
        /// <returns>Task with its progress status</returns>
        public async Task<TaskResult> RefreshLobbyDataAsync()
        {
            if (CurrentLobby == null) return new TaskResult(false, new Exception("Lobby not exist"));
            try
            {
                var newLobby = await LobbyService.Instance.GetLobbyAsync(CurrentLobby.Id);
                CurrentLobby = newLobby;
                return TaskResult.Ok;
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
                return new TaskResult(false, e);
            }
        }
        
        /// <summary>
        /// Player Logout Function in the Lobby
        /// </summary>
        /// <returns>Task with its progress status</returns>
        public async Task<TaskResult> LeaveLobbyAsync()
        {
            try
            {
                await LobbyService.Instance.RemovePlayerAsync(CurrentLobby.Id, AuthenticationService.Instance.PlayerId);
                StopUpdatingLobby();
                return TaskResult.Ok;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new TaskResult(false, e);
            }
        }
        
        /// <summary>
        /// Stops lobby data updates and sending heartbeat
        /// </summary>
        public void StopUpdatingLobby()
        {
            _heartbeatLobbyThread?.Abort();
            _refreshLobbyThread?.Abort();
        }
        
        /// <summary>
        /// Sends heartbeat to the lobby. Designed to run in a separate thread.
        /// </summary>
        private void HeartbeatLobbyThread()
        {
            while (true)
            {
                try
                {
                    LobbyService.Instance.SendHeartbeatPingAsync(CurrentLobby.Id);
                    Debug.Log("HeartBeat");
                    Thread.Sleep(GlobalConstants.HeartbeatLobbyDelayMilliseconds);
                }
                catch (ThreadAbortException)
                {
                    return;
                }
                catch (Exception e)
                {
                    Debug.Log($"The HeartbeatLobby thread ended with an error - {e.Message}");
                    return;
                }
            }
        }

        /// <summary>
        /// Updates lobby data periodically. The function is designed to run in a separate thread
        /// </summary>
        private void RefreshLobbyThread()
        {
            while (true)
            {
                try
                {
                    var task = RefreshLobbyDataAsync();
                    task.Wait();
                    var result = task.Result;
                    if (result.Success)
                    {
                        Debug.Log("UpdateLobby");
                        OnLobbyUpdated?.Invoke();
                    }
                    Thread.Sleep(GlobalConstants.RefreshLobbyDelayMilliseconds);
                }
                catch (ThreadAbortException)
                {
                    return;
                }
                catch (Exception e)
                {
                    Debug.Log($"The RefreshLobby thread ended with an error - {e.Message}");
                    return;
                }
            }
        }
        
        /// <summary>
        /// Abort running threads
        /// </summary>
        public override void Dispose()
        {
            _heartbeatLobbyThread?.Abort();
            _refreshLobbyThread?.Abort();
        }
    }
}