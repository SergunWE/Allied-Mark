using NetworkFramework.Data;
using NetworkFramework.EventSystem.EventParameter;
using NetworkFramework.LobbyCore;
using TMPro;
using UnityEngine;

namespace NetworkFramework.MonoBehaviour_Components
{
    public class LobbyConnectorComponent : MonoBehaviour
    {
        [SerializeField] private GameEventBool lobbyCreated;
        [SerializeField] private GameEventBool lobbyJoined;
        
        [SerializeField] private LobbyOptions lobbyOptions;
        [SerializeField] private LobbyInternalData lobbyInternalData;
        [SerializeField] private LobbyInternalPlayerData lobbyInternalPlayerData;

        private LobbyConnectorCore _core;

        private void Awake()
        {
            _core ??= new LobbyConnectorCore();
        }

        public async void CreateLobby()
        {
            lobbyCreated.Raise((await _core.CreateLobbyAsync(lobbyOptions.LobbyName, lobbyOptions.MaxPlayer,
                lobbyOptions.Privacy, lobbyInternalData.GetDictionary, lobbyInternalPlayerData.GetDictionary)).Success);
        }

        public async void JoinLobbyQuick()
        {
            lobbyJoined.Raise((await _core.JoinLobbyQuickAsync(lobbyInternalPlayerData.GetDictionary)).Success);
        }

        public async void JoinLobbyByCode(string code)
        {
            lobbyJoined.Raise((await _core.JoinLobbyByCodeAsync(code, lobbyInternalPlayerData.GetDictionary)).Success);
        }
        
        public void JoinLobbyByCode(TMP_InputField tmpInputField)
        {
            JoinLobbyByCode(tmpInputField.text.ToUpper());
        }
    }
}