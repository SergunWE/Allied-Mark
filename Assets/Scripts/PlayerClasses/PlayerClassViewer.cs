using System.Linq;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerClassViewer : MonoBehaviour
{
    [SerializeField] private PlayerClassNetwork playerClassNetwork;
    [SerializeField] private PlayerClassHandler playerClassHandler;
    [SerializeField] private Transform rootPlayerModel;

    [SerializeField] private PlayerModelAnimationController animationController;
    
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
        
        ModelHelper.SetOwnerModelSettings(playerClassNetwork, _playerModel);

        animationController.SetAnimator(ModelHelper.FindComponents<Animator>(_playerModel).First());
    }
}