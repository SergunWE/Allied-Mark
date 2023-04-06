using NetworkFramework.Data;
using NetworkFramework.MonoBehaviour_Components;
using UnityEngine;

public class LevelDifficultyHandler : DropdownDataDisplayer<LevelDifficulty>
{
    [SerializeField] private LobbyUpdaterComponent lobbyUpdater;
    
    protected override void OnDropdownValueChanged(int index)
    {
        lobbyUpdater.ChangeLevelDifficulty(data[index]);
    }

    public override void OnLobbyUpdated()
    {
        if (!LobbyData.Exist) return;
        string diffName = LobbyData.Current.Data[CustomDataKeys.LevelDifficulty.Key].Value;
        dropdown.value = data.FindIndex(x => x.DifficultName == diffName);
    }
}