using System;
using System.Threading;
using NetworkFramework.Data;
using NetworkFramework.EventSystem.EventParameter;
using NetworkFramework.EventSystem.Events;
using NetworkFramework.Managers;
using TMPro;
using UnityEngine;

namespace NetworkFramework.MonoBehaviour_Components
{
    public class LobbyComponent : MonoBehaviour
    {
        [SerializeField] private GameEventBool lobbyCreated;
        [SerializeField] private GameEventBool lobbyJoined;
        [SerializeField] private GameEvent lobbyRefreshed;

        [SerializeField] private LobbyOptions lobbyOptions;
        [SerializeField] private LobbyData lobbyData;
        [SerializeField] private LobbyPlayerData lobbyPlayerData;
        
        private static LobbyManager _lobbyManager;
        private static Thread _lobbyRefresherThread;
        
        private void Awake()
        {
            _lobbyManager ??= new LobbyManager();
            if (!_lobbyManager.LobbyExist()) return;
            _lobbyRefresherThread?.Abort();
            _lobbyRefresherThread = new Thread(RefreshLobbyThread);
            _lobbyRefresherThread.Start();
        }

        private void OnDestroy()
        {
            _lobbyRefresherThread?.Abort();
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

        public async void RefreshLobbyData()
        {
            bool refreshStatus = await _lobbyManager.RefreshLobbyDataAsync();
            if (refreshStatus)
            {
                Debug.Log("Refresh lobby success");
                lobbyRefreshed.Raise();
            }
        }

        private void RefreshLobbyThread()
        {
            try
            {
                while (true)
                {
                    RefreshLobbyData();
                    Thread.Sleep(GlobalConstants.RefreshLobbyDelayMilliseconds);
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }
    }
}