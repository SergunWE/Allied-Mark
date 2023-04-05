using NetworkFramework.MonoBehaviour_Components;
using UnityEngine;

public class LevelDifficultyHandler : DropdownDataDisplayer<LevelDifficulty>
{
    [SerializeField] private LobbyUpdaterComponent lobbyUpdater;
    
    protected override void OnDropdownValueChanged(int index)
    {
        lobbyUpdater.ChangeLevelDifficulty(data[index]);
    }
}