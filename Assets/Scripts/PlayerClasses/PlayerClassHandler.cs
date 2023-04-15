public class PlayerClassHandler : DataHandler<PlayerClass>
{
    protected override string GetKey(PlayerClass value)
    {
        return value.ClassName;
    }
}