using NetworkFramework;
using NetworkFramework.Data;
using NetworkFramework.MonoBehaviour_Components;
using UnityEngine;

public class LevelDifficultyHandlerDropdown : DropdownDataDisplayer<LevelDifficulty>
{
    [SerializeField] private LobbyUpdaterComponent lobbyUpdater;
    
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