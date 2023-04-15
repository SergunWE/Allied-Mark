public class LevelDifficultyHandler : DataHandler<LevelDifficulty>
{
    protected override string GetKey(LevelDifficulty value)
    {
        return value.DifficultName;
    }
}