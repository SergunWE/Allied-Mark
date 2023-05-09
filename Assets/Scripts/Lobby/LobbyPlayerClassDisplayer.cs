using NetworkFramework;
using NetworkFramework.Data;
using NetworkFramework.MonoBehaviour_Components.Interfaces;
using UnityEngine;

public class LobbyPlayerClassDisplayer : DisplayerBase
{
    [SerializeField] private PlayerClassHandler playerClassHandler;
    [SerializeField] private PlayerClassDisplayer[] views;
    
    protected override void SetViews()
    {
        var currentPlayers = LobbyData.Current.Players;
        for (int i = 0; i < currentPlayers.Count; i++)
        {
            string playerId = currentPlayers[i].Id;
            string playerClass = LobbyData.GetPlayerData(CustomDataKeys.PlayerClass.Key, playerId);
            views[i].SetData(playerClassHandler.DataDictionary[playerClass].DisplayName);
        }
    }
}