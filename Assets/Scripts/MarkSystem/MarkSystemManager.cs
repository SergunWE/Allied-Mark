using System.Collections.Generic;
using NetworkFramework.Netcode_Components;
using UnityEngine;
using UnityEngine.InputSystem;

public class MarkSystemManager : NetworkComponentManager<PlayerMarkNetwork>
{
    [SerializeField] private Camera playerCamera;

    private MarkInfo _playerMark;

    private Queue<(MarkNetwork, MarkInfoStruct)> _markedObject;

    public void SetPlayerMark(MarkInfo playerMark)
    {
        _playerMark = playerMark;
        _markedObject = new Queue<(MarkNetwork, MarkInfoStruct)>(_playerMark.maxMarkCount);
    }

    public void OnClick(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        var obj = RaycastHelper.GetObject(playerCamera.transform);
        if(obj == null) return;
        var component = obj.GetComponentInParent<MarkNetwork>();
        if (component == null) return;
        if (_markedObject.Count >= _playerMark.maxMarkCount &&
            _markedObject.TryDequeue(out var result))
        {
            result.Item1.UnSetMarkServerRpc(result.Item2);
        }

        var info = new MarkInfoStruct(networkComponent.NetworkObject, _playerMark);
        component.SetMarkServerRpc(info);
        _markedObject.Enqueue((component,info));
    }
}