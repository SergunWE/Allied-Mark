using NetworkFramework.Data;
using Unity.Services.Lobbies.Models;

namespace NetworkFramework
{
    public static class DataKeysConstants
    {
        public static LobbyDataInfo<PlayerDataObject.VisibilityOptions> PlayerName = new("PlayerName",
            PlayerDataObject.VisibilityOptions.Member);
        public static LobbyDataInfo<PlayerDataObject.VisibilityOptions> PlayerReady = new("PlayerReady", 
            PlayerDataObject.VisibilityOptions.Member);
        public static LobbyDataInfo<DataObject.VisibilityOptions> LobbyLevel = new("LobbyLevel", 
            DataObject.VisibilityOptions.Public);
    }

    
}