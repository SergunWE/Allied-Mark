using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NetworkFramework.Data;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;

namespace NetworkFramework.LobbyCore
{
    /// <summary>
    /// Class to connect to the lobby
    /// </summary>
    public class LobbyConnectorCore : LobbyCore
    {
        /// <summary>
        /// Lobby creation function
        /// </summary>
        /// <param name="lobbyName">Lobby Name</param>
        /// <param name="maxPlayers">Maximum number of players in the lobby</param>
        /// <param name="isPrivate">Is the lobby private</param>
        /// <param name="lobbyData">Dictionary of initial lobby data</param>
        /// <param name="playerData">Dictionary of initial data of the host player</param>
        /// <returns>Task with its progress status</returns>
        public async Task<TaskResult> CreateLobbyAsync(string lobbyName, int maxPlayers, bool isPrivate,
            Dictionary<string, DataObject> lobbyData = null, Dictionary<string, PlayerDataObject> playerData = null)
        {
            if (!AuthenticationService.Instance.IsSignedIn)
            {
                return new TaskResult(false, new RequestFailedException(0, "Sign In Failed"));
            }
            //Forming data for the lobby
            var player = new Player(AuthenticationService.Instance.PlayerId, null, playerData);
            var options = new CreateLobbyOptions
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
                return new TaskResult(false, e);
            }

            Debug.Log($"Lobby Id: {CurrentLobby.Id}");
            Debug.Log($"Lobby Code: {CurrentLobby.LobbyCode}");
            return TaskResult.Ok;
        }

        /// <summary>
        /// Function to join the lobby by code
        /// </summary>
        /// <param name="code">Code for joining the lobby</param>
        /// <param name="playerData">Dictionary with initial user data</param>
        /// <returns>Task with its progress status</returns>
        public async Task<TaskResult> JoinLobbyByCodeAsync(string code,
            Dictionary<string, PlayerDataObject> playerData = null)
        {
            try
            {
                JoinLobbyByCodeOptions options = new JoinLobbyByCodeOptions();
                Player player = new Player(AuthenticationService.Instance.PlayerId, null, playerData);
                options.Player = player;
                CurrentLobby = await LobbyService.Instance.JoinLobbyByCodeAsync(code, options);
                return new TaskResult(CurrentLobby != null, new Exception("Lobby not created"));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new TaskResult(false, e);
            }
        }

        /// <summary>
        /// Quick connection function to the lobby
        /// </summary>
        /// <param name="playerData">Dictionary with initial user data</param>
        /// <returns>Task with its progress status</returns>
        public async Task<TaskResult> JoinLobbyQuickAsync(Dictionary<string, PlayerDataObject> playerData = null)
        {
            try
            {
                Player player = new Player(AuthenticationService.Instance.PlayerId, null, playerData);
                QuickJoinLobbyOptions options = new QuickJoinLobbyOptions
                {
                    Player = player
                };
                CurrentLobby = await LobbyService.Instance.QuickJoinLobbyAsync(options);
                return new TaskResult(CurrentLobby != null, new Exception("Lobby not created"));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new TaskResult(false, e);
            }
        }
        
        public override void Dispose()
        {
        }
    }
}