using NetworkFramework.Data;
using NetworkFramework.EventSystem.EventParameter;
using NetworkFramework.LobbyCore;
using NetworkFramework.RelayCore;
using NetworkFramework.SO;
using UnityEngine;

namespace NetworkFramework.MonoBehaviour_Components
{
    public class RelayConnectorComponent : MonoBehaviour
    {
        [SerializeField] private PlayersReadyChecker readyChecker;
        [SerializeField] private GameEventBool relayStarted;

        [SerializeField] private LobbyOptions lobbyOptions;

        private LobbyRefresherCore _refreshCore;
        private RelayConnectorCore _relayCore;
        private LobbyDataUpdaterCore _dataUpdaterCore;

        private bool _playerStartedGame;

        private void Awake()
        {
            _refreshCore ??= new LobbyRefresherCore();
            _relayCore ??= new RelayConnectorCore();
            _dataUpdaterCore ??= new LobbyDataUpdaterCore(false);
        }

        public async void CreateRelay()
        {
            if (!readyChecker.PlayerReady() || LobbyData.GetLobbyData(DataKeys.RelayCode.Key) != null) return;
            if ((await _relayCore.CreateRelay(lobbyOptions.MaxPlayer)).Success)
            {
                relayStarted.Raise((await _dataUpdaterCore.UpdateLobbyData(DataKeys.RelayCode,
                    _relayCore.JoinCode)).Success);
            }
        }

        public void OnLobbyRefreshed()
        {
            if (!_playerStartedGame && LobbyData.GetLobbyData(DataKeys.RelayCode.Key) != null && !_refreshCore.PlayerIsHost)
            {
                JoinRelay();
            }
        }

        private async void JoinRelay()
        {
            _playerStartedGame = (await _relayCore.JoinRelay(
                LobbyData.GetLobbyData(DataKeys.RelayCode.Key))).Success;
            relayStarted.Raise(_playerStartedGame);
        }
    }
}