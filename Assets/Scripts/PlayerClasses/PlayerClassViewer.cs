using Unity.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerClassViewer : MonoBehaviour
{
    [SerializeField] private PlayerClassNetwork playerClassNetwork;
    [SerializeField] private PlayerClassHandler playerClassHandler;
    [SerializeField] private Transform rootPlayerModel;
    private GameObject _playerModel;

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
        var playerModel = playerClassHandler.DataDictionary[playerClassName.Value].playerModel;
        if (_playerModel != null) Destroy(_playerModel);
        _playerModel = Instantiate(playerModel, rootPlayerModel);

        if (playerClassNetwork.IsOwner)
        {
            SetOwnerModelSettings();
        }
    }

    private void SetOwnerModelSettings()
    {
        if (!_playerModel.TryGetComponent<SkinnedMeshRenderer>(out var comp))
        {
            comp = _playerModel.GetComponentInChildren<SkinnedMeshRenderer>();
        }

        if (comp != null)
        {
            comp.shadowCastingMode = ShadowCastingMode.ShadowsOnly;
        }
    }
}