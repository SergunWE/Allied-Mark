using Unity.Netcode;
using UnityEngine;

namespace NetworkFramework.NetcodeComponents
{
    /// <summary>
    /// Universal class for components, for managing client-owner network variables
    /// </summary>
    /// <typeparam name="T"><see cref="ObjectNetwork{T}"/></typeparam>
    public class NetworkComponentManager<T> : MonoBehaviour
    {
        /// <summary>
        /// Network variable
        /// </summary>
        [SerializeField] protected T networkComponent;

        /// <summary>
        /// Searching for the right component of the client-owner
        /// </summary>
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