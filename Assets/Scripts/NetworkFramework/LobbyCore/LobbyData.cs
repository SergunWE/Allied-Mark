using Unity.Services.Lobbies.Models;

namespace NetworkFramework.LobbyCore
{
    public static class LobbyData
    {
        public static Lobby Current { get; set; }
        public static bool LobbyExist => Current != null;
    }
}