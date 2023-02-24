using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace NetworkFramework.EventSystem.EventParameter
{
    public class GameEventListenerParameter<T> : MonoBehaviour
    {
        [SerializeField]
        private GameEventParameter<T> gameEvent;

        public UnityEvent<T> response;

        private void OnEnable()
        {
            gameEvent.RegisterListener(this);
        }

        private void OnDisable()
        {
            gameEvent.UnregisterListener(this);
        }

        public void OnEventRaised(T value)
        {
            response.Invoke(value);
        }
    }
}