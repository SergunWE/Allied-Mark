using System;
using System.Threading;
using System.Threading.Tasks;
using NetworkFramework.Data;
using Unity.Services.Authentication;
using Unity.Services.Lobbies;
using UnityEngine;

namespace NetworkFramework.LobbyCore
{
    public class LobbyRefresherCore : LobbyCore
    {
        public event Action OnLobbyUpdated;

        private Thread _heartbeatLobbyThread;
        private Thread _refreshLobbyThread;

        public void StartHeartbeat()
        {
            if(!PlayerIsHost) return;
            _heartbeatLobbyThread?.Abort();
            _heartbeatLobbyThread = new Thread(HeartbeatLobbyThread);
            _heartbeatLobbyThread.Start();
        }

        public void StartRefresh()
        {
            _refreshLobbyThread?.Abort();
            _refreshLobbyThread = new Thread(RefreshLobbyThread);
            _refreshLobbyThread.Start();
        }

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
        
        public void StopUpdatingLobby()
        {
            _heartbeatLobbyThread?.Abort();
            _refreshLobbyThread?.Abort();
        }
        
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
        
        public override void Dispose()
        {
            _heartbeatLobbyThread?.Abort();
            _refreshLobbyThread?.Abort();
        }
    }
}