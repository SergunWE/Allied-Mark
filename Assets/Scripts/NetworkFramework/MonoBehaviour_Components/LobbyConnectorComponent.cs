using NetworkFramework.Data;
using NetworkFramework.EventSystem.EventParameter;
using NetworkFramework.LobbyCore;
using NetworkFramework.SO;
using TMPro;
using UnityEngine;

namespace NetworkFramework.MonoBehaviour_Components
{
    /// <summary>
    /// Lobby connection component
    /// </summary>
    public class LobbyConnectorComponent : MonoBehaviour
    {
        /// <summary>
        /// Lobby creation event
        /// </summary>
        [SerializeField] private GameEventBool lobbyCreated;
        /// <summary>
        /// The event of joining the lobby
        /// </summary>
        [SerializeField] private GameEventBool lobbyJoined;
        
        /// <summary>
        /// Lobby settings
        /// </summary>
        [SerializeField] private LobbyOptions lobbyOptions;
        /// <summary>
        /// Lobby data
        /// </summary>
        [SerializeField] private LobbyInternalData lobbyInternalData;
        /// <summary>
        /// Lobby player data
        /// </summary>
        [SerializeField] private LobbyInternalPlayerData lobbyInternalPlayerData;

        private LobbyConnectorCore _core;

        private void Awake()
        {
            _core ??= new LobbyConnectorCore();
        }

        /// <summary>
        /// Creating a lobby
        /// </summary>
        public async void CreateLobby()
        {
            lobbyCreated.Raise((await _core.CreateLobbyAsync(lobbyOptions.LobbyName, lobbyOptions.MaxPlayer,
                lobbyOptions.Privacy, lobbyInternalData.GetDictionary, lobbyInternalPlayerData.GetDictionary)).Success);
        }

        /// <summary>
        /// Quick connection to the lobby
        /// </summary>
        public async void JoinLobbyQuick()
        {
            lobbyJoined.Raise((await _core.JoinLobbyQuickAsync(lobbyInternalPlayerData.GetDictionary)).Success);
        }

        /// <summary>
        /// Connecting to the lobby by code
        /// </summary>
        /// <param name="code">Lobby connection code</param>
        public async void JoinLobbyByCode(string code)
        {
            lobbyJoined.Raise((await _core.JoinLobbyByCodeAsync(code, lobbyInternalPlayerData.GetDictionary)).Success);
        }
        
        /// <summary>
        /// Connecting to the lobby by code
        /// </summary>
        /// <param name="tmpInputField">Lobby code input field</param>
        public void JoinLobbyByCode(TMP_InputField tmpInputField)
        {
            JoinLobbyByCode(tmpInputField.text.ToUpper());
        }
    }
}