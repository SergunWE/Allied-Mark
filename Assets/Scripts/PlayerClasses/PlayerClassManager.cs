using NetworkFramework.Netcode_Components;
using UnityEngine;

public class PlayerClassManager : NetworkComponentManager<PlayerClassNetwork>
{
    [SerializeField] private PlayerClassHandler playerClassHandler;
    [SerializeField] private MarkSystemManager markManager;

    public PlayerClass PlayerClass { get; private set; }

    protected override void Start()
    {
        base.Start();
        string networkClassName = networkComponent.GetPlayerClassName();
        PlayerClass = playerClassHandler.DataDictionary[networkClassName];
        markManager.SetPlayerMark(GetMarkInfo());
    }

    private MarkInfo GetMarkInfo()
    {
        return PlayerClass.markInfo;
    }
}