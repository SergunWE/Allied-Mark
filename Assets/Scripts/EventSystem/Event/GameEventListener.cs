using UnityEngine;
using UnityEngine.Events;

public class GameEventListener : MonoBehaviour
{
    /// <summary>
    /// SO event
    /// </summary>
    [SerializeField] private GameEvent gameEvent;

    /// <summary>
    /// List of objects and actions when an event is triggered
    /// </summary>
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