using UnityEngine;
using UnityEngine.Rendering;

namespace NetworkFramework.Data
{
    [CreateAssetMenu(menuName = "Lobby/Lobby Options")]
    public class LobbyOptionsSo : ScriptableObject
    {
        [SerializeField] private string lobbyName;
        [SerializeField] private bool privacy;
        [SerializeField] private int maxPlayer;

        private void OnValidate()
        {
            if (string.IsNullOrEmpty(lobbyName))
            {
                lobbyName = "Lobby name";
            }

            if (maxPlayer <= 0)
            {
                maxPlayer = 1;
            }
        }

        public void SetMaxPlayer(string value)
        {
            if (int.TryParse(value, out int maxPlayerValue))
            {
                maxPlayer = maxPlayerValue;
            }
        }

        public string LobbyName
        {
            get => lobbyName;
            set => lobbyName = value;
        }

        public bool Privacy
        {
            get => privacy;
            set => privacy = value;
        }

        public int MaxPlayer
        {
            get => maxPlayer;
            set => maxPlayer = value;
        }
    }
}