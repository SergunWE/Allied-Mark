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

        private void Awake()
        {
            _refreshCore ??= new LobbyRefresherCore();
            _relayCore ??= new RelayConnectorCore();
        }

        public async void CreateRelay()
        {
            if (!(await _refreshCore.RefreshLobbyDataAsync()).Success || !readyChecker.PlayerReady()) return;
            if ((await _relayCore.CreateRelay(lobbyOptions.MaxPlayer - 1)).Success)
            {
                relayStarted.Raise((await _dataUpdaterCore.UpdateLobbyData(DataKeys.RelayCode,
                    _relayCore.JoinCode)).Success);
            }
        }

        public async void JoinRelay()
        {
            relayStarted.Raise((await _relayCore.JoinRelay(
                LobbyData.GetLobbyData(DataKeys.RelayCode.Key))).Success);
        }
    }
}