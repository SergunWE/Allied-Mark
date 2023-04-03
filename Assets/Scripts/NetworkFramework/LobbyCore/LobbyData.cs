using Unity.Services.Lobbies.Models;

namespace NetworkFramework.LobbyCore
{
    public class LobbyData
    {
        public static Lobby Current { get; set; }
        public static bool LobbyExist => Current != null;
    }
}