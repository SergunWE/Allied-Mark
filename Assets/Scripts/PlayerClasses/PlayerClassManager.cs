using NetworkFramework.NetcodeComponents;
using UnityEngine;

public class PlayerClassManager : NetworkComponentManager<PlayerClassNetwork>
{
    [SerializeField] private MarkSystemManager markManager;

    protected override void Start()
    {
        base.Start();
        markManager.SetPlayerMark(GetMarkInfo());
    }

    private MarkInfo GetMarkInfo()
    {
        return networkComponent.PlayerClassInfo.markInfo;
    }
}