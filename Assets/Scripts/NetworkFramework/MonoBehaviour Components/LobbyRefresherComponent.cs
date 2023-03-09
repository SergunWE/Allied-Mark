using System;
using NetworkFramework.EventSystem.Event;
using NetworkFramework.LobbyCore;
using UnityEngine;

namespace NetworkFramework.MonoBehaviour_Components
{
    public class LobbyRefresherComponent : MonoBehaviour
    {
        [SerializeField] private GameEvent lobbyRefreshed;
        
        private LobbyRefresherCore _core;

        private void Awake()
        {
            _core ??= new LobbyRefresherCore();
        }

        private void Start()
        {
            if (!LobbyData.LobbyExist) return;
            StartSync();
        }

        private void OnEnable()
        {
            _core.OnLobbyDataUpdated += lobbyRefreshed.Raise;
        }

        private void OnDisable()
        {
            _core.OnLobbyDataUpdated -= lobbyRefreshed.Raise;
        }

        public void StartSync()
        {
            _core.StartHeartbeat();
            _core.StartRefresh();
        }
    }
}