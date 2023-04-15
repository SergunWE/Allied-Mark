using System.Collections.Generic;

public class MarkHandler : DataHandler<MarkInfo>
{
    protected override string GetKey(MarkInfo value)
    {
        return value.markName;
    }
}