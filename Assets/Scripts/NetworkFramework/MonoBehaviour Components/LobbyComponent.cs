using System;
using NetworkFramework.Data;
using NetworkFramework.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace NetworkFramework.MonoBehaviour_Components
{
    public class LobbyComponent : MonoBehaviour
    {
        public UnityEvent<bool> lobbyCreated;
        public UnityEvent<bool> lobbyJoined;

        [SerializeField] private LobbyOptionsSo lobbyOptions;
        private static LobbyManager _lobbyManager;

        private void Awake()
        {
            _lobbyManager ??= new LobbyManager();
        }

        public async void CreateLobby()
        {
            lobbyCreated?.Invoke(await _lobbyManager.CreateLobbyAsync(lobbyOptions, null));
        }

        public async void JoinLobbyByCode(string code)
        {
            lobbyJoined?.Invoke(await _lobbyManager.JoinLobbyAsync(code));
        }
        
        public void JoinLobbyByCode(TMP_InputField tmpInputField)
        {
            JoinLobbyByCode(tmpInputField.text.ToUpper());
        }
    }
}