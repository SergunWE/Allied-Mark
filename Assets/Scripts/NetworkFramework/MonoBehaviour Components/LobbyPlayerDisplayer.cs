using System.Collections.Generic;
using NetworkFramework.Data;
using Unity.Services.Lobbies.Models;
using UnityEngine;

namespace NetworkFramework.MonoBehaviour_Components
{
    public class LobbyPlayerDisplayer : MonoBehaviour
    {
        [SerializeField] private PlayerDataDisplayer[] playerViews;
        
        private List<Player> _prevLobbyPlayers;

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
                var playerData = currentPlayers[i].Data;
                playerViews[i].SetData(playerData[DataKeysConstants.PlayerName.Key].Value, 
                    bool.Parse(playerData[DataKeysConstants.PlayerReady.Key].Value));
            }

            for (int i = currentPlayers.Count; i < 4; i++)
            {
                playerViews[i].HidePlayer();
            }
        }
    }
}