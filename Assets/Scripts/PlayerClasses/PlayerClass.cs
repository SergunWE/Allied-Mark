using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(menuName = "Player Class")]
public class PlayerClass : ScriptableObject, IUIDisplayed
{
    //This class is for storing data of the player class
    //This can be his health, model, weapon, etc.
    [SerializeField] private string className;
    public string ClassName => className;

    public List<WeaponInfo> weapons;
    public MarkInfo markInfo;
    public GameObject playerModel;

    public string DisplayName => className;
}