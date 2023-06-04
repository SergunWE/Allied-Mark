using System;
using System.Collections.Generic;
using NetworkFramework.NetcodeComponents;
using UnityEngine;
using UnityEngine.InputSystem;

public class MarkSystemManager : NetworkComponentManager<PlayerClassNetwork>
{
    private MarkInfo _playerMark;

    private List<(MarkNetwork, MarkInfoNetwork)> _markedObject;
    private Transform _playerCameraTransform;

    protected override void Start()
    {
        base.Start();
        _playerMark = networkComponent.PlayerClassInfo.markInfo;
        _markedObject = new List<(MarkNetwork, MarkInfoNetwork)>(_playerMark.maxMarkCount);
        _playerCameraTransform = CameraHelper.GetPlayerCamera.transform;
    }

    public void OnClick(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        var obj = RaycastHelper.GetObject(_playerCameraTransform);
        if(obj == null) return;
        var component = obj.GetComponentInParent<MarkNetwork>();
        if (component == null) return;
        CheckMarkNetworkDestroy();
        if (_markedObject.Count >= _playerMark.maxMarkCount)
        {
            _markedObject[0].Item1.UnSetMarkServerRpc(_markedObject[0].Item2);
            _markedObject.RemoveAt(0);
        }
        var info = new MarkInfoNetwork(networkComponent.NetworkObject, _playerMark);
        component.SetMarkServerRpc(info);
        _markedObject.Add((component,info));
    }

    private void CheckMarkNetworkDestroy()
    {
        for (int i = 0; i < _markedObject.Count; i++)
        {
            var markObject = _markedObject[i];
            try
            {
                if (!markObject.Item1.HasNetworkObject)
                {
                    _markedObject.Remove(markObject);
                }
            }
            catch (Exception)
            {
                _markedObject.Remove(markObject);
            }
        }
    }
}