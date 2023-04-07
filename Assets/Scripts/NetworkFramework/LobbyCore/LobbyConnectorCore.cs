using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using TaskStatus = NetworkFramework.Data.TaskStatus;

namespace NetworkFramework.LobbyCore
{
    public class LobbyConnectorCore : LobbyCore
    {
        public async Task<TaskStatus> CreateLobbyAsync(string lobbyName, int maxPlayers, bool isPrivate,
            Dictionary<string, DataObject> lobbyData = null, Dictionary<string, PlayerDataObject> playerData = null)
        {
            if (!AuthenticationService.Instance.IsSignedIn) return new TaskStatus(false,
                new RequestFailedException(0, "Sign In Failed"));
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
            catch (Exception e)
            {
                return new TaskStatus(false, e);
            }
            
            Debug.Log($"Lobby Id: {CurrentLobby.Id}");
            Debug.Log($"Lobby Code: {CurrentLobby.LobbyCode}");
            return TaskStatus.Ok;
        }

        public async Task<TaskStatus> JoinLobbyByCodeAsync(string code, Dictionary<string, PlayerDataObject> playerData = null)
        {
            try
            {
                JoinLobbyByCodeOptions options = new JoinLobbyByCodeOptions();
                Player player = new Player(AuthenticationService.Instance.PlayerId, null, playerData);
                options.Player = player;
                CurrentLobby = await LobbyService.Instance.JoinLobbyByCodeAsync(code, options);
                return new TaskStatus(CurrentLobby != null, new Exception("Lobby not created"));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new TaskStatus(false, e);
            }
        }

        public async Task<TaskStatus> JoinLobbyQuickAsync(Dictionary<string, PlayerDataObject> playerData = null)
        {
            try
            {
                Player player = new Player(AuthenticationService.Instance.PlayerId, null, playerData);
                QuickJoinLobbyOptions options = new QuickJoinLobbyOptions
                {
                    Player = player
                };
                CurrentLobby = await LobbyService.Instance.QuickJoinLobbyAsync(options);
                return new TaskStatus(CurrentLobby != null, new Exception("Lobby not created"));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new TaskStatus(false, e);
            }
        }

        public override void Dispose()
        {
            
        }
    }
}