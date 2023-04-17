using System;
using Unity.Services.Lobbies.Models;

namespace NetworkFramework.Data
{
    /// <summary>
    /// The class that stores the current lobby
    /// <remarks>
    /// For the host, this is where the lobby he created is stored. For the client, this stores the lobby it joined
    /// </remarks>
    /// </summary>
    public static class LobbyData
    {
        /// <summary>
        /// Current Lobby
        /// </summary>
        public static Lobby Current { get; set; }
        
        /// <summary>
        /// Does the current lobby exist now. If false, the user is not yet connected to the lobby
        /// <value>
        /// If true, the user has been connected to the lobby. If false, the user has not yet connected to the lobby
        /// </value>
        /// </summary>
        public static bool Exist => Current != null;

        /// <summary>
        /// Getting current lobby data by key
        /// </summary>
        /// <param name="key">Lobby data dictionary key</param>
        /// <returns>Returns string or null on error</returns>
        public static string GetLobbyData(string key)
        {
            try
            {
                var lobbyData = Current.Data;
                return lobbyData[key].Value;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        /// <summary>
        /// Getting player data of the current lobby
        /// </summary>
        /// <param name="key">Player data dictionary key</param>
        /// <param name="playerId">Authentication player ID</param>
        /// <returns>Returns string or null on error</returns>
        public static string GetPlayerData(string key, string playerId)
        {
            try
            {
                var players = Current.Players;
                var playerData = players.Find(p => p.Id == playerId);
                return playerData.Data[key].Value;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }
    }
}