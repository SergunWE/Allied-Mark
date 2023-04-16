using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Event/Game Event")]
public class GameEvent : ScriptableObject
{
	//list of listener components
    private readonly List<GameEventListener> _eventListeners = new();

	//event call
    public void Raise()
    {
        for (int i = _eventListeners.Count - 1; i >= 0; i--)
            _eventListeners[i].OnEventRaised();
    }

	//event subscription
    public void RegisterListener(GameEventListener listener)
    {
        if (!_eventListeners.Contains(listener))
            _eventListeners.Add(listener);
    }
	
	//unsubscribe from the event
    public void UnregisterListener(GameEventListener listener)
    {
        if (_eventListeners.Contains(listener))
            _eventListeners.Remove(listener);
    }
}