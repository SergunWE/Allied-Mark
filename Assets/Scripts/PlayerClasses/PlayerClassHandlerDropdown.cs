using UnityEngine;

public class PlayerClassHandlerDropdown : DropdownDataDisplayer<PlayerClass>
{
    [SerializeField] private CustomLobbyUpdaterComponent lobbyUpdater;

    protected override void OnDropdownValueChanged(int index)
    {
        lobbyUpdater.ChangePlayerClass(data.Get[index]);
    }

    public override void OnLobbyRefreshed()
    {
        //It is enough to store the class locally and not synchronize it with the server.
        // if (!LobbyData.Exist) return;
        // string className = LobbyData.Current.Data[lobbyUpdater.LevelDifficulty.Key].Value;
        // dropdown.value = data.FindIndex((x) => x.ClassName == className);
    }
}