using UnityEngine;

public class PlayerClassViewer : MonoBehaviour
{
    [SerializeField] private PlayerClassHandler playerClassHandler;
    
    [SerializeField] private Transform rootPlayerModel;
    private GameObject _playerModel;

    public void SetPlayerModel(string playerClassName)
    {
        var playerClass = playerClassHandler.Get.Find(
            playerClass => playerClass.ClassName == playerClassName);
        if(_playerModel != null) Destroy(_playerModel);
        _playerModel = Instantiate(playerClass.playerModel, rootPlayerModel);
    }
}