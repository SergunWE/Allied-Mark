using NetworkFramework;
using NetworkFramework.Data;
using UnityEngine;

public class LevelDifficultyHandlerDropdown : DropdownDataDisplayer<LevelDifficulty>
{
    [SerializeField] private CustomLobbyUpdaterComponent lobbyUpdater;
    
    protected override void OnDropdownValueChanged(int index)
    {
        lobbyUpdater.ChangeLevelDifficulty(data.Get[index]);
    }

    public override void OnLobbyRefreshed()
    {
        if (!LobbyData.Exist) return;
        string diffName = LobbyData.Current.Data[CustomDataKeys.LevelDifficulty.Key].Value;
        dropdown.value = data.Get.FindIndex(x => x.DifficultName == diffName);
    }
}