using NetworkFramework.Data;
using Unity.Services.Lobbies.Models;

namespace NetworkFramework
{
    /// <summary>
    /// A class containing the obligatory data for the lobby
    /// <remarks>
    /// Data information represents the key and visibility of the data
    /// </remarks>
    /// </summary>
    public static class DataKeys
    {
		/// <value>
		/// Key and visibility for player data for the player name
		/// </value>
        public static LobbyDataInfo<PlayerDataObject.VisibilityOptions> PlayerName = new("PlayerName",
            PlayerDataObject.VisibilityOptions.Member);
		/// <value>
		/// Key and visibility for player data for player ready
		/// </value>
        public static LobbyDataInfo<PlayerDataObject.VisibilityOptions> PlayerReady = new("PlayerReady",
            PlayerDataObject.VisibilityOptions.Member);
		/// <value>
		/// Key and visibility for lobby data for the game level
		/// </value>
        public static LobbyDataInfo<DataObject.VisibilityOptions> LobbyLevel = new("LobbyLevel",
            DataObject.VisibilityOptions.Public);
		/// <value>
		/// Key and visibility for lobby data for Relay code to join the server for other players
		/// </value>
        public static LobbyDataInfo<DataObject.VisibilityOptions> RelayCode = new("RelayCode",
            DataObject.VisibilityOptions.Member);
    }
}