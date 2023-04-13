using System.Collections.Generic;
using UnityEngine;

public class MarkViewer : MonoBehaviour
{
    [SerializeField] private GameObject markModel;
    [SerializeField] private Transform startPositionMarks;
    [SerializeField] private float markPositionOffset;

    private readonly List<(MarkInfo, Renderer)> _markRenderers = new();

    public void SetMark(MarkInfo markInfo)
    {
        var mark = Instantiate(markModel);
        _markRenderers.Add((markInfo,mark.GetComponent<Renderer>()));
        SetMarksPosition();
    }

    public void UnsetMark(MarkInfo markInfo)
    {
        var mark = _markRenderers.Find(
            (x) => x.Item1.markName == markInfo.markName);
        _markRenderers.Remove(mark);
        Destroy(mark.Item2.gameObject);
    }

    private void SetMarksPosition()
    {
        for (int i = 0; i < _markRenderers.Count; i++)
        {
            var startPos = startPositionMarks.transform.position;
            _markRenderers[i].Item2.transform.position = new Vector3(startPos.x, startPos.y + markPositionOffset * i,
                startPos.z);
        }
    }
}