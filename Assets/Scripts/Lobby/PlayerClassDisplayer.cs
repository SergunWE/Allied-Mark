using TMPro;
using UnityEditor.Animations;
using UnityEngine;

public class PlayerClassDisplayer : MonoBehaviour
{
    [SerializeField] private PlayerClassHandler playerClassHandler;
    [SerializeField] private AnimatorController lobbyAnimatorController;
    [SerializeField] private TMP_Text classText;
    [SerializeField] private Transform modelRoot;

    private string _prevClassName;

    public void SetData(string className)
    {
        if(_prevClassName == className) return;
        var playerClass = playerClassHandler.DataDictionary[className];
        classText.text = playerClass.DisplayName;

        if (modelRoot.childCount != 0)
        {
            foreach (Transform child in modelRoot)
            {
                Destroy(child.gameObject);
            }
        }

        var obj = Instantiate(playerClass.playerModel, modelRoot);
        obj.GetComponentInChildren<Animator>().runtimeAnimatorController = lobbyAnimatorController;
        _prevClassName = className;
    }
}