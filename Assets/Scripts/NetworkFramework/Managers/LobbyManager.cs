using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NetworkFramework.Data;
using Unity.Services.Authentication;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;

namespace NetworkFramework.Managers
{
    public class LobbyManager
    {
        private Lobby _lobby;
        private Thread _heartbeatLobbyThread;

        public LobbyManager()
        {
        }

        public async Task<bool> CreateLobbyAsync(string lobbyName, int maxPlayers, bool isPrivate,
            Dictionary<string, DataObject> lobbyData = null, Dictionary<string, PlayerDataObject> playerData = null)
        {
            if (!AuthenticationService.Instance.IsSignedIn) return false;
            Player player = new Player(AuthenticationService.Instance.PlayerId, null, playerData);

            CreateLobbyOptions options = new CreateLobbyOptions
            {
                Player = player,
                IsPrivate = isPrivate,
                Data = lobbyData
            };

            try
            {
                _lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName,
                    maxPlayers, options);
            }
            catch (Exception)
            {
                return false;
            }

            _heartbeatLobbyThread?.Abort();
            _heartbeatLobbyThread = new Thread(HeartbeatLobbyThread);
            _heartbeatLobbyThread.Start();

            Debug.Log($"Lobby Id: {_lobby.Id}");
            Debug.Log($"Lobby Code: {_lobby.LobbyCode}");
            return true;
        }

        public async Task<bool> JoinLobbyByCodeAsync(string code)
        {
            try
            {
                JoinLobbyByCodeOptions options = new JoinLobbyByCodeOptions();
                Player player = new Player(AuthenticationService.Instance.PlayerId, null, null);
                options.Player = player;
                _lobby = await LobbyService.Instance.JoinLobbyByCodeAsync(code, options);
                return _lobby != null;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }
        
        public async Task<bool> RefreshLobbyDataAsync()
        {
            if (_lobby == null) return false;
            try
            {
                var newLobby = await LobbyService.Instance.GetLobbyAsync(_lobby.Id);
                if (_lobby.LastUpdated < newLobby.LastUpdated)
                {
                    _lobby = newLobby;
                }
                return true;
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
                return false;
            }
        }

        public bool LobbyExist()
        {
            return _lobby != null;
        }
        
        public string GetLobbyCode()
        {
            return _lobby?.LobbyCode;
        }

        private void HeartbeatLobbyThread()
        {
            while (true)
            {
                try
                {
                    LobbyService.Instance.SendHeartbeatPingAsync(_lobby.Id);
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

        ~LobbyManager()
        {
            _heartbeatLobbyThread?.Abort();
        }
    }
}