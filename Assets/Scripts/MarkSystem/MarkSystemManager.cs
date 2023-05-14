using System.Collections.Generic;
using NetworkFramework.NetcodeComponents;
using UnityEngine;
using UnityEngine.InputSystem;

public class MarkSystemManager : NetworkComponentManager<PlayerMarkNetwork>
{
    [SerializeField] private Camera playerCamera;

    private MarkInfo _playerMark;

    private Queue<(MarkNetwork, MarkInfoNetwork)> _markedObject;

    public void SetPlayerMark(MarkInfo playerMark)
    {
        _playerMark = playerMark;
        _markedObject = new Queue<(MarkNetwork, MarkInfoNetwork)>(_playerMark.maxMarkCount);
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

        var info = new MarkInfoNetwork(networkComponent.NetworkObject, _playerMark);
        component.SetMarkServerRpc(info);
        _markedObject.Enqueue((component,info));
    }
}