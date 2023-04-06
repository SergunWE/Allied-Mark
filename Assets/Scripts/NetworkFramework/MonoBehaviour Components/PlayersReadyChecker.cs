using System.Linq;
using NetworkFramework.Data;
using NetworkFramework.EventSystem.EventParameter;
using UnityEngine;

namespace NetworkFramework.MonoBehaviour_Components
{
    public class PlayersReadyChecker : MonoBehaviour
    {
        [SerializeField] private GameEventBool allPlayersReady;

        private bool _prevState;
        
        public void OnLobbyRefreshed()
        {
            bool state = PlayerReady();
            if (_prevState == state) return;
            allPlayersReady.Raise(state);
            _prevState = state;
        }

        public bool PlayerReady()
        {
            var currentPlayers = LobbyData.Current.Players;
            return currentPlayers.All(player => bool.Parse(player.Data[DataKeys.PlayerReady.Key].Value));
        }
    }
}