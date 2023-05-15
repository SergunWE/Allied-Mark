using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player Class")]
public class PlayerClass : ScriptableObject, IUIDisplayed
{
    //This class is for storing data of the player class
    //This can be his health, model, weapon, etc.
    [SerializeField] private string className;
    [SerializeField] private string displayedName;
    
    [field: SerializeField] public int Health { get; private set; }
    public string ClassName => className;
    
    public List<WeaponInfo> weapons;
    public MarkInfo markInfo;
    public GameObject playerModel;

    public string DisplayName => displayedName;
}