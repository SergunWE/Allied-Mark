using System;
using Unity.Services.Lobbies.Models;

namespace NetworkFramework.Data
{
    public static class LobbyData
    {
        public static Lobby Current { get; set; }
        public static bool Exist => Current != null;

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