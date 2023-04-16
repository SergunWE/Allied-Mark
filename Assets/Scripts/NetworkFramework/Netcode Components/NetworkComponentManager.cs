using Unity.Netcode;
using UnityEngine;

namespace NetworkFramework.Netcode_Components
{
    public class NetworkComponentManager<T> : MonoBehaviour
    {
        [SerializeField] protected T networkComponent;

        protected virtual void Start()
        {
            try
            {
                var ownerObjects = NetworkManager.Singleton.LocalClient.OwnedObjects;
                //Search among your objects for the right component
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
}