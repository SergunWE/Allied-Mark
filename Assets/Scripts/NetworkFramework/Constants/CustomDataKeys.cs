using NetworkFramework.Data;
using Unity.Services.Lobbies.Models;

namespace NetworkFramework
{
    /// <summary>
    /// The class is designed to store custom information for the lobby
    /// <example>
    /// This is an example of adding information, you need to add a static property
    /// <code>
    /// public static LobbyDataInfo{DataObject.VisibilityOptions} LevelDifficulty = new ("LevelDifficulty",
    /// DataObject.VisibilityOptions.Public);
    /// </code>
    /// Addressing from other classes
    /// <code>CustomDataKeys.LevelDifficulty.Key</code>
    /// </example>
    /// </summary>
    public static class CustomDataKeys
    {
        //custom lobby data
        public static LobbyDataInfo<DataObject.VisibilityOptions> LevelDifficulty = new ("LevelDifficulty",
            DataObject.VisibilityOptions.Public);

        //custom player data
        public static LobbyDataInfo<PlayerDataObject.VisibilityOptions> PlayerClass = new ("PlayerClass",
            PlayerDataObject.VisibilityOptions.Member);
    }
}