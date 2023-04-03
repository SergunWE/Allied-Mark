using Unity.Services.Lobbies.Models;

namespace NetworkFramework.LobbyCore
{
    public static class LobbyData
    {
        static LobbyData()
        {
            
        }
        
        public static Lobby Current { get; set; }
        public static bool Exist => Current != null;
    }
}