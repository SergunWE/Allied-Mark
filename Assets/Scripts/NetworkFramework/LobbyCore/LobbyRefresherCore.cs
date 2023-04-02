using System;
using System.Threading;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;

namespace NetworkFramework.LobbyCore
{
    public class LobbyRefresherCore
    {
        public event Action OnLobbyDataUpdated;
        
        private static Lobby CurrentLobby
        {
            get => LobbyData.Current;
            set => LobbyData.Current = value;
        }
        
        private Thread _heartbeatLobbyThread;
        private Thread _refreshLobbyThread;

        public void StartHeartbeat()
        {
            if(CurrentLobby.HostId != AuthenticationService.Instance.PlayerId) return;
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

        public async Task<TaskStatus> RefreshLobbyDataAsync()
        {
            if (CurrentLobby == null) return new TaskStatus(false, new Exception("Lobby not exist"));
            try
            {
                var newLobby = await LobbyService.Instance.GetLobbyAsync(CurrentLobby.Id);
                if (CurrentLobby.LastUpdated < newLobby.LastUpdated)
                {
                    CurrentLobby = newLobby;
                }
                return new TaskStatus(true);
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
                return new TaskStatus(false, e);
            }
        }
        
        public async Task<TaskStatus> LeaveLobbyAsync()
        {
            try
            {
                await LobbyService.Instance.RemovePlayerAsync(CurrentLobby.Id, AuthenticationService.Instance.PlayerId);
                StopUpdatingLobby();
                return new TaskStatus(true);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new TaskStatus(false, e);
            }
        }
        
        private void StopUpdatingLobby()
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
                        OnLobbyDataUpdated?.Invoke();
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

        ~LobbyRefresherCore()
        {
            _heartbeatLobbyThread?.Abort();
            _refreshLobbyThread?.Abort();
        }
    }
}