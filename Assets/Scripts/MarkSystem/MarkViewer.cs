using System.Collections.Generic;
using UnityEngine;

public class MarkViewer : MonoBehaviour
{
    [SerializeField] private MarkNetwork markNetwork;
    [SerializeField] private MarkHandler markHandler;
    [SerializeField] private GameObject markModel;
    [SerializeField] private Transform startPositionMarks;
    [SerializeField] private float markPositionOffset;

    private readonly List<(MarkInfoNetwork, Renderer)> _marks = new();
    private void OnEnable()
    {
        markNetwork.MarkSet += SetMark;
        markNetwork.MarkUnset += UnsetMark;
    }

    private void OnDisable()
    {
        markNetwork.MarkSet -= SetMark;
        markNetwork.MarkUnset -= UnsetMark;
    }

    private void SetMark(MarkInfoNetwork markInfoNetwork)
    {
        var markInfo = markHandler.DataDictionary[markInfoNetwork.MarkName.Value];
        var mark = Instantiate(markModel);
        var rendererComponent = mark.GetComponent<Renderer>();
        rendererComponent.material = markInfo.markColor;
        _marks.Add((markInfoNetwork,rendererComponent));
        SetMarksPosition();
    }

    private void UnsetMark(MarkInfoNetwork markInfoNetwork)
    {
        int infoNetworkIndex =_marks.FindIndex(x => x.Item1.Equals(markInfoNetwork));
        var mark = _marks[infoNetworkIndex];
        _marks.Remove(mark);
        Destroy(mark.Item2.gameObject);
        SetMarksPosition();
    }

    private void SetMarksPosition()
    {
        for (int i = 0; i < _marks.Count; i++)
        {
            var startPos = startPositionMarks.transform.position;
            _marks[i].Item2.transform.position = new Vector3(startPos.x, startPos.y + markPositionOffset * i,
                startPos.z);
        }
    }
}