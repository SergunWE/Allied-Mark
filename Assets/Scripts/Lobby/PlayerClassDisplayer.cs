using TMPro;
using UnityEngine;

public class PlayerClassDisplayer : MonoBehaviour
{
    
    [SerializeField] private TMP_Text classText;

    public void SetData(string className)
    {
        classText.text = className;
    }
}