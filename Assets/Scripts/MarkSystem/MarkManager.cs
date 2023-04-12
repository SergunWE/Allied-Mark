using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class MarkManager : NetworkComponentManager<MarkNetwork>
{
    [SerializeField] private Camera playerCamera;
    
    public void OnClick(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        var obj = GetObject();
        if(obj == null) return;
        var component = obj.GetComponentInParent<NetworkObject>();
        if (component != null)
        {
            networkComponent.SetMarkServerRpc(component);
        }
    }

    public GameObject GetObject()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        return Physics.Raycast(ray, out var hit, 100) ? hit.collider.gameObject : null;
    }
}