using Unity.Netcode;
using UnityEngine;

public class NetworkComponentManager<T> : MonoBehaviour
{
    [SerializeField] protected T networkComponent;
    
    protected virtual void Start()
    {
        try
        {
            var ownerObjects = NetworkManager.Singleton.LocalClient.OwnedObjects;
            for (int i = 0; i < ownerObjects.Count; i++)
            {
                networkComponent = ownerObjects[i].GetComponentInChildren<T>();
                if (networkComponent != null) break;
            }
        }
        catch
        {
            // ignored
        }
    }
}