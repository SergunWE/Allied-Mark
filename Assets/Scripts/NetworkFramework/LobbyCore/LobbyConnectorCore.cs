using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;

namespace NetworkFramework.LobbyCore
{
    public class LobbyConnectorCore
    {
        private static Lobby CurrentLobby
        {
            get => LobbyData.Current;
            set => LobbyData.Current = value;
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
                CurrentLobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName,
                    maxPlayers, options);
            }
            catch (Exception)
            {
                return false;
            }
            
            Debug.Log($"Lobby Id: {CurrentLobby.Id}");
            Debug.Log($"Lobby Code: {CurrentLobby.LobbyCode}");
            return true;
        }

        public async Task<bool> JoinLobbyByCodeAsync(string code)
        {
            try
            {
                JoinLobbyByCodeOptions options = new JoinLobbyByCodeOptions();
                Player player = new Player(AuthenticationService.Instance.PlayerId, null, null);
                options.Player = player;
                CurrentLobby = await LobbyService.Instance.JoinLobbyByCodeAsync(code, options);
                return CurrentLobby != null;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }
    }
}