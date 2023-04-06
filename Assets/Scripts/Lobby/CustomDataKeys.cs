using NetworkFramework.Data;
using Unity.Services.Lobbies.Models;

public static class CustomDataKeys
{
    //custom lobby data
    public static LobbyDataInfo<DataObject.VisibilityOptions> LevelDifficulty { get; }
    
    //custom player data
    public static LobbyDataInfo<PlayerDataObject.VisibilityOptions> PlayerClass { get; }

    static CustomDataKeys()
    {
        LevelDifficulty = new LobbyDataInfo<DataObject.VisibilityOptions>("LevelDifficulty",
            DataObject.VisibilityOptions.Public);
        PlayerClass = new LobbyDataInfo<PlayerDataObject.VisibilityOptions>("PlayerClass", 
            PlayerDataObject.VisibilityOptions.Member);
    }
}