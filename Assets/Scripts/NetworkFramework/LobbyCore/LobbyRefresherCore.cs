using System;
using System.Threading;
using System.Threading.Tasks;
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
        
        public async Task<bool> RefreshLobbyDataAsync()
        {
            if (CurrentLobby == null) return false;
            try
            {
                var newLobby = await LobbyService.Instance.GetLobbyAsync(CurrentLobby.Id);
                if (CurrentLobby.LastUpdated < newLobby.LastUpdated)
                {
                    CurrentLobby = newLobby;
                }
                return true;
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
                return false;
            }
        }
        
        private void HeartbeatLobbyThread()
        {
            while (true)
            {
                try
                {
                    LobbyService.Instance.SendHeartbeatPingAsync(LobbyData.Current.Id);
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
                    bool result = task.Result;
                    if (result)
                    {
                        OnLobbyDataUpdated?.Invoke();
                    }
                    Debug.Log("UpdateLobby");
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
            OnLobbyDataUpdated = null;
            _heartbeatLobbyThread?.Abort();
            _refreshLobbyThread?.Abort();
        }
    }
}