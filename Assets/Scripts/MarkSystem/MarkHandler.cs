using System.Collections.Generic;

public class MarkHandler : DataHandler<MarkInfo>
{
    private Dictionary<string, MarkInfo> _markDict;

    public Dictionary<string, MarkInfo> MarkDict
    {
        get
        {
            if (_markDict == null)
            {
                Awake();
            }

            return _markDict;
        }
        
    }

    private void Awake()
    {
        _markDict = new Dictionary<string, MarkInfo>();
        foreach (var info in data)
        {
            MarkDict[info.markName] = info;
        }
    }
}