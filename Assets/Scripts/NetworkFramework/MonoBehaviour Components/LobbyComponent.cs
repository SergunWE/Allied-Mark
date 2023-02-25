using NetworkFramework.Data;
using NetworkFramework.EventSystem.EventParameter;
using NetworkFramework.Managers;
using TMPro;
using UnityEngine;

namespace NetworkFramework.MonoBehaviour_Components
{
    public class LobbyComponent : MonoBehaviour
    {
        public GameEventBool lobbyCreated;
        public GameEventBool lobbyJoined;

        [SerializeField] private LobbyOptions lobbyOptions;
        [SerializeField] private LobbyData lobbyData;
        [SerializeField] private LobbyPlayerData lobbyPlayerData;
        private static LobbyManager _lobbyManager;

        private void Awake()
        {
            _lobbyManager ??= new LobbyManager();
        }

        public async void CreateLobby()
        {
            lobbyCreated.Raise(await _lobbyManager.CreateLobbyAsync(lobbyOptions.LobbyName, lobbyOptions.MaxPlayer, 
                lobbyOptions.Privacy, lobbyData.GetDictionary, lobbyPlayerData.GetDictionary));
        }

        public async void JoinLobbyByCode(string code)
        {
            lobbyJoined.Raise(await _lobbyManager.JoinLobbyByCodeAsync(code));
        }
        
        public void JoinLobbyByCode(TMP_InputField tmpInputField)
        {
            JoinLobbyByCode(tmpInputField.text.ToUpper());
        }
    }
}