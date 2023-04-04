using System;
using UnityEngine;

public class LevelDifficultyHandler : MonoBehaviour
{
    [SerializeField] private LevelDifficulty[] levelDifficulties;

    public int DifficultyCount => levelDifficulties.Length;

    public LevelDifficulty GetDifficult(int index)
    {
        try
        {
            return levelDifficulties[index];
        }
        catch (IndexOutOfRangeException e)
        {
            Console.WriteLine(e);
            return null;
        }
    }
}