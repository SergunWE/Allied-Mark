using NetworkFramework.MonoBehaviour_Components;
using UnityEngine;

public class PlayerClassHandler : DropdownDataDisplayer<PlayerClass>
{
    [SerializeField] private LobbyUpdaterComponent lobbyUpdater;
    
    protected override void OnDropdownValueChanged(int index)
    {
        lobbyUpdater.ChangePlayerClass(data[index]);
    }
}