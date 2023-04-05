using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(menuName = "Player Class")]
public class PlayerClass : ScriptableObject, IUIDisplayed
{
    //This class is for storing data of the player class
    //This can be his health, model, weapon, etc.
    [SerializeField] private string className;
    public string ClassName => className;

    public string DisplayName => className;
}