using NetworkFramework;
using NetworkFramework.LobbyCore;
using UnityEngine;

/// <summary>
/// Custom component for sending custom lobby data
/// </summary>
public class CustomLobbyUpdaterComponent : MonoBehaviour
{
    private LobbyDataUpdaterCore _core;

    private void Awake()
    {
        _core ??= new LobbyDataUpdaterCore(true);
    }
    
    /// <summary>
    /// Change the level of difficulty. Only the host lobby can change the difficulty level
    /// </summary>
    /// <param name="difficulty">Level of difficulty</param>
    public async void ChangeLevelDifficulty(LevelDifficulty difficulty)
    {
        await _core.UpdateLobbyData(CustomDataKeys.LevelDifficulty, difficulty.DifficultName);
    }

    /// <summary>
    /// Changing the player's class
    /// </summary>
    /// <param name="class">Player's game class</param>
    public async void ChangePlayerClass(PlayerClass @class)
    {
        await _core.UpdatePlayerData(CustomDataKeys.PlayerClass, @class.ClassName);
    }
}