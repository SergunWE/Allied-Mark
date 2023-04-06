using NetworkFramework.Data;
using UnityEngine;

namespace NetworkFramework.MonoBehaviour_Components
{
    public class LobbyPlayerDisplayer : MonoBehaviour
    {
        [SerializeField] private PlayerDataDisplayer[] playerViews;

        private void Start()
        {
            SetViews();
        }

        public void OnLobbyRefreshed()
        {
            SetViews();
        }

        private void SetViews()
        {
            var currentPlayers = LobbyData.Current.Players;
            Debug.Log($"Player count: {currentPlayers.Count}");
            for (int i = 0; i < currentPlayers.Count; i++)
            {
                string playerId = currentPlayers[i].Id;
                string playerName = LobbyData.GetPlayerData(DataKeys.PlayerName.Key, playerId);
                string playerClass = LobbyData.GetPlayerData(CustomDataKeys.PlayerClass.Key, playerId);
                bool playerReady = bool.Parse(LobbyData.GetPlayerData(DataKeys.PlayerReady.Key, playerId));
                playerViews[i].SetData(playerName, playerClass, playerReady);
            }

            for (int i = currentPlayers.Count; i < 4; i++)
            {
                playerViews[i].HidePlayer();
            }
        }
    }
}