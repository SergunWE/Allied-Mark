using UnityEngine;

namespace NetworkFramework.MonoBehaviourComponents.Interfaces
{
    /// <summary>
    /// Basic class for components displaying lobby data
    /// </summary>
    public abstract class DisplayerBase : MonoBehaviour
    {
        private void Start()
        {
            SetViews();
        }

        /// <summary>
        /// Function intended for the lobby update event
        /// </summary>
        public void OnLobbyRefreshed()
        {
            SetViews();
        }

        /// <summary>
        /// Setting the lobby data display
        /// </summary>
        protected abstract void SetViews();
    }
}