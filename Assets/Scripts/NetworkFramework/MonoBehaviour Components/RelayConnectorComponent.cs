using System;
using NetworkFramework.LobbyCore;
using NetworkFramework.RelayCore;
using UnityEngine;

namespace NetworkFramework.MonoBehaviour_Components
{
    public class RelayConnectorComponent : MonoBehaviour
    {
        [SerializeField] private PlayersReadyChecker readyChecker;
        
        private LobbyRefresherCore _refreshCore;
        private RelayConnectorCore _core;

        private void Awake()
        {
            _refreshCore ??= new LobbyRefresherCore();
        }

        public async void StartGame()
        {
            if ((await _refreshCore.RefreshLobbyDataAsync()).Success && readyChecker.PlayerReady())
            {
                _core.CreateRelay(1);
            }
        }
    }
}