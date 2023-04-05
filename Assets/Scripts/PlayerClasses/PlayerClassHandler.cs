using NetworkFramework.MonoBehaviour_Components;
using UnityEngine;

public class PlayerClassHandler : DropdownDataDisplayer<PlayerClass>
{
    [SerializeField] private LobbyUpdaterComponent lobbyUpdater;

    protected override void OnDropdownValueChanged(int index)
    {
        lobbyUpdater.ChangePlayerClass(data[index]);
    }

    public override void OnLobbyUpdated()
    {
        //It is enough to store the class locally and not synchronize it with the server.
        // if (!LobbyData.Exist) return;
        // string className = LobbyData.Current.Data[lobbyUpdater.LevelDifficulty.Key].Value;
        // dropdown.value = data.FindIndex((x) => x.ClassName == className);
    }
}