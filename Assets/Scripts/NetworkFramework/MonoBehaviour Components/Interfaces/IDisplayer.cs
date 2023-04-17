using UnityEngine;

namespace NetworkFramework.MonoBehaviour_Components.Interfaces
{
    public abstract class DisplayerBase : MonoBehaviour
    {
        private void Start()
        {
            SetViews();
        }

        public void OnLobbyRefreshed()
        {
            SetViews();
        }

        protected abstract void SetViews();
    }
}