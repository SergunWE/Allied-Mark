using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(menuName = "Level Difficulty/Difficulty")]
public class LevelDifficulty : ScriptableObject
{
    //Here are the difficulty level settings fields
    //Here you can add, for example, the health of certain enemies or their damage

    [SerializeField] private string difficultName;
    public string DifficultName => difficultName;
}