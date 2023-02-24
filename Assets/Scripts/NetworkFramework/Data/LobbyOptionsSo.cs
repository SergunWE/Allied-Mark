using NetworkFramework.EventSystem.Events;
using UnityEngine;

namespace NetworkFramework.Data
{
    [CreateAssetMenu(menuName = "Lobby/Lobby Options")]
    public class LobbyOptionsSo : ScriptableObject
    {
        [SerializeField] private GameEvent changed;

        [SerializeField] private string lobbyName;
        [SerializeField] private bool privacy;
        [SerializeField] private int maxPlayer;

        private void OnValidate()
        {
            CheckOptions();
        }

        private void CheckOptions()
        {
            lobbyName ??= "";
            if (maxPlayer <= 0)
            {
                maxPlayer = 1;
            }

            changed.Raise();
        }

        public void SetMaxPlayer(string value)
        {
            if (int.TryParse(value, out int maxPlayerValue))
            {
                MaxPlayer = maxPlayerValue;
            }
        }

        public string LobbyName
        {
            get => lobbyName;
            set
            {
                lobbyName = value;
                CheckOptions();
            } 
        }

        public bool Privacy
        {
            get => privacy;
            set => privacy = value;
        }

        public int MaxPlayer
        {
            get => maxPlayer;
            set
            {
                maxPlayer = value;
                CheckOptions();
            }
        }
    }
}