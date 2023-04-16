using UnityEngine;
using UnityEngine.Events;

public class GameEventListener : MonoBehaviour
{
	//SO event
    [SerializeField] private GameEvent gameEvent;

	//list of objects and actions when an event is triggered
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