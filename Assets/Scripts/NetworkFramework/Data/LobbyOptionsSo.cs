using UnityEngine;

namespace NetworkFramework.Data
{
    [CreateAssetMenu(menuName = "Lobby/Lobby Options")]
    public class LobbyOptionsSo : ScriptableObject
    {
        public string Name;
        public bool Privacy;
        public int MaxPlayer;

        private void OnValidate()
        {
            if (MaxPlayer <= 0)
            {
                MaxPlayer = 1;
            }
        }
    }
}