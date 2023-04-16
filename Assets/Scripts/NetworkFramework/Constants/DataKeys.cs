using NetworkFramework.Data;
using Unity.Services.Lobbies.Models;

namespace NetworkFramework
{
    public static class DataKeys
    {
		//player data - player name
        public static LobbyDataInfo<PlayerDataObject.VisibilityOptions> PlayerName = new("PlayerName",
            PlayerDataObject.VisibilityOptions.Member);
		//player data - player readiness for the game
        public static LobbyDataInfo<PlayerDataObject.VisibilityOptions> PlayerReady = new("PlayerReady",
            PlayerDataObject.VisibilityOptions.Member);
		//lobby data - lobby level
        public static LobbyDataInfo<DataObject.VisibilityOptions> LobbyLevel = new("LobbyLevel",
            DataObject.VisibilityOptions.Public);
		//lobby data - code to connect to a Relay server
        public static LobbyDataInfo<DataObject.VisibilityOptions> RelayCode = new("RelayCode",
            DataObject.VisibilityOptions.Member);
    }
}