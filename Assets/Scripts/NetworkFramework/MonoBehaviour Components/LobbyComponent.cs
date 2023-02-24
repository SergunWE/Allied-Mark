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

        [SerializeField] private LobbyOptionsSo lobbyOptions;
        private static LobbyManager _lobbyManager;

        private void Awake()
        {
            _lobbyManager ??= new LobbyManager();
        }

        public async void CreateLobby()
        {
            lobbyCreated.Raise(await _lobbyManager.CreateLobbyAsync(lobbyOptions, null));
        }

        public async void JoinLobbyByCode(string code)
        {
            lobbyJoined.Raise(await _lobbyManager.JoinLobbyAsync(code));
        }
        
        public void JoinLobbyByCode(TMP_InputField tmpInputField)
        {
            JoinLobbyByCode(tmpInputField.text.ToUpper());
        }
    }
}