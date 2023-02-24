using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace NetworkFramework.EventSystem.Events
{
    public class GameEventListener : MonoBehaviour
    {
        [FormerlySerializedAs("Event")]
        [Tooltip("Event to register with.")]
        [SerializeField]
        private GameEvent gameEvent;

        [Tooltip("Response to invoke when Event is raised.")]
        public UnityEvent response;

        private void OnEnable()
        {
            gameEvent.RegisterListener(this);
        }

        private void OnDisable()
        {
            gameEvent.UnregisterListener(this);
        }

        public void OnEventRaised()
        {
            response.Invoke();
        }
    }
}