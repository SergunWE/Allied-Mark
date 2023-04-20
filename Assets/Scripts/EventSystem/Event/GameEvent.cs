using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Event/Game Event")]
public class GameEvent : ScriptableObject
{
    /// <summary>
    /// List of listener components
    /// </summary>
    private readonly List<GameEventListener> _eventListeners = new();
    
    /// <summary>
    /// Event call
    /// </summary>
    public void Raise()
    {
        for (int i = _eventListeners.Count - 1; i >= 0; i--)
            _eventListeners[i].OnEventRaised();
    }

    /// <summary>
    /// Event subscription
    /// </summary>
    /// <param name="listener"></param>
    public void RegisterListener(GameEventListener listener)
    {
        if (!_eventListeners.Contains(listener))
            _eventListeners.Add(listener);
    }
    
    /// <summary>
    /// Unsubscribe from the event
    /// </summary>
    /// <param name="listener"></param>
    public void UnregisterListener(GameEventListener listener)
    {
        if (_eventListeners.Contains(listener))
            _eventListeners.Remove(listener);
    }
}