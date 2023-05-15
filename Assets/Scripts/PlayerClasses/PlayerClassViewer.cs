using Unity.Collections;
using UnityEngine;

public class PlayerClassViewer : MonoBehaviour
{
    [SerializeField] private PlayerClassNetwork playerClassNetwork;
    [SerializeField] private SkinnedMeshRenderer rendererModel;

    private void Start()
    {
        SetPlayerModel(playerClassNetwork.LocalValue);
    }

    private void OnEnable()
    {
        playerClassNetwork.ValueChanged += SetPlayerModel;
    }
    
    private void OnDisable()
    {
        playerClassNetwork.ValueChanged -= SetPlayerModel;
    }

    private void SetPlayerModel(FixedString128Bytes playerClassName)
    {
        if(playerClassName.IsEmpty) return;
        var playerModel =  playerClassNetwork.PlayerClassInfo.playerModel.
            GetComponentInChildren<SkinnedMeshRenderer>();
        rendererModel.sharedMesh = playerModel.sharedMesh;
        ModelHelper.SetOwnerModelSettings(playerClassNetwork, rendererModel.gameObject);
    }
}