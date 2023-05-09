using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(menuName = "Level Difficulty/Difficulty")]
public class LevelDifficulty : ScriptableObject, IUIDisplayed
{
    //Here are the difficulty level settings fields
    //Here you can add, for example, the health of certain enemies or their damage

    [SerializeField] private string difficultName;

    [SerializeField] private string displayedName;
    public string DifficultName => difficultName;

    public string DisplayName => displayedName;
}