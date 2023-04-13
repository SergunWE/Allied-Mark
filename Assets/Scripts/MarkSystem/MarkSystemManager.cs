using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class MarkSystemManager : NetworkComponentManager<PlayerMarkNetwork>
{
    [SerializeField] private Camera playerCamera;

    private MarkInfo _playerMark;

    public void SetPlayerMark(MarkInfo playerMark)
    {
        _playerMark = playerMark;
    }

    public void OnClick(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        var obj = RaycastHelper.GetObject(playerCamera.transform);
        if(obj == null) return;
        var component = obj.GetComponentInParent<MarkNetwork>();
        if (component != null)
        {
            component.SetMarkServerRpc(new MarkInfoStruct(networkComponent.NetworkObject, 
                _playerMark));
        }
    }
}