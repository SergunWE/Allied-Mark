using NetworkFramework.Data;
using NetworkFramework.MonoBehaviour_Components.Interfaces;
using UnityEngine;

namespace NetworkFramework.MonoBehaviour_Components
{
    public class LobbyPlayerDisplayer : DisplayerBase
    {
        [SerializeField] private PlayerDataDisplayer[] playerViews;

        protected override void SetViews()
        {
            var currentPlayers = LobbyData.Current.Players;
            Debug.Log($"Player count: {currentPlayers.Count}");
            for (int i = 0; i < currentPlayers.Count; i++)
            {
                string playerId = currentPlayers[i].Id;
                string playerName = LobbyData.GetPlayerData(DataKeys.PlayerName.Key, playerId);
                bool playerReady = bool.Parse(LobbyData.GetPlayerData(DataKeys.PlayerReady.Key, playerId));
                playerViews[i].SetData(playerName, playerReady);
            }

            for (int i = currentPlayers.Count; i < 4; i++)
            {
                playerViews[i].HidePlayer();
            }
        }
    }
}