using UnityEngine;

public class PlayerClassViewer : MonoBehaviour
{
    [SerializeField] private PlayerClassHandler playerClassHandler;
    [SerializeField] private Transform rootPlayerModel;
    [SerializeField] private WeaponViewer weaponViewer;
    private GameObject _playerModel;

    public void SetPlayerModel(string playerClassName)
    {
        var playerClass = playerClassHandler.Get.Find(
            playerClass => playerClass.ClassName == playerClassName);
        if(_playerModel != null) Destroy(_playerModel);
        _playerModel = Instantiate(playerClass.playerModel, rootPlayerModel);
        for (int i = 0; i < playerClass.weapons.Count; i++)
        {
            weaponViewer.SetWeaponModel(i, playerClass.weapons[i].model);
        }
    }
}