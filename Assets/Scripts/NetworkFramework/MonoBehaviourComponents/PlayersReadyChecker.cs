using System.Linq;
using NetworkFramework.Data;
using NetworkFramework.EventSystem.EventParameter;
using UnityEngine;

namespace NetworkFramework.MonoBehaviourComponents
{
    /// <summary>
    /// The player ready checkup component
    /// </summary>
    public class PlayersReadyChecker : MonoBehaviour
    {
        /// <summary>
        /// Ready event for all players
        /// </summary>
        [SerializeField] private GameEventBool allPlayersReady;

        private bool _prevState;
        
        /// <summary>
        /// Actions when a lobby update is triggered
        /// </summary>
        public void OnLobbyRefreshed()
        {
            bool state = PlayerReady();
            if (_prevState == state) return;
            allPlayersReady.Raise(state);
            _prevState = state;
        }

        /// <summary>
        /// Checking player ready
        /// </summary>
        /// <returns>True, if all the players are ready</returns>
        public bool PlayerReady()
        {
            var currentPlayers = LobbyData.Current.Players;
            return currentPlayers.All(player => bool.Parse(player.Data[DataKeys.PlayerReady.Key].Value));
        }
    }
}