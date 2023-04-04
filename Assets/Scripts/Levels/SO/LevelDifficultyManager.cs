using UnityEngine;

[CreateAssetMenu(menuName = "Level Difficulty/Manager")]
public class LevelDifficultyManager : ScriptableObject
{
    [SerializeField] private LevelDifficulty[] levelDifficulties;
}