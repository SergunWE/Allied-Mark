using NetworkFramework.Netcode_Components;
using UnityEngine;

public class PlayerClassManager : NetworkComponentManager<PlayerClassNetwork>
{
    [SerializeField] private PlayerClassHandler playerClassHandler;
    [SerializeField] private MarkSystemManager markManager;

    private PlayerClass _playerClass;

    protected override void Start()
    {
        base.Start();
        string networkClassName = networkComponent.GetPlayerClassName();
        _playerClass = playerClassHandler.DataDictionary[networkClassName];
        markManager.SetPlayerMark(GetMarkInfo());
    }

    private MarkInfo GetMarkInfo()
    {
        return _playerClass.markInfo;
    }
}