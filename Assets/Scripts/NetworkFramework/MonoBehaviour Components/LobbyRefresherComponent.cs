using NetworkFramework.EventSystem.Event;
using NetworkFramework.LobbyCore;
using UnityEngine;

namespace NetworkFramework.MonoBehaviour_Components
{
    public class LobbyRefresherComponent : MonoBehaviour
    {
        [SerializeField] private GameEvent lobbyRefreshed;
        
        private LobbyRefresherCore _core;
        private bool _isUpdate;

        private void Awake()
        {
            _core ??= new LobbyRefresherCore();
        }

        private void Start()
        {
            if (!LobbyData.LobbyExist) return;
            StartSync();
        }

        private void FixedUpdate()
        {
            if (!_isUpdate) return;
            lobbyRefreshed.Raise();
            _isUpdate = false;
        }

        private void OnEnable()
        {
            _core.OnLobbyDataUpdated += OnLobbyUpdated;
        }

        private void OnDisable()
        {
            _core.OnLobbyDataUpdated -= OnLobbyUpdated;
        }

        public void StartSync()
        {
            _core.StartHeartbeat();
            _core.StartRefresh();
        }

        public void OnLobbyUpdated()
        {
            _isUpdate = true;
        }
    }
}