using System;
using System.Collections.Generic;
using UnityEngine;

namespace NetworkFramework.EventSystem.EventParameter
{
    public class GameEventParameter<T> : ScriptableObject
    {
        private readonly List<GameEventListenerParameter<T>> _eventListeners = new();

        public void Raise(T value)
        {
            for(int i = _eventListeners.Count -1; i >= 0; i--)
                _eventListeners[i].OnEventRaised(value);
        }

        public void RegisterListener(GameEventListenerParameter<T> listener)
        {
            if (!_eventListeners.Contains(listener))
                _eventListeners.Add(listener);
        }

        public void UnregisterListener(GameEventListenerParameter<T> listener)
        {
            if (_eventListeners.Contains(listener))
                _eventListeners.Remove(listener);
        }
    }
}