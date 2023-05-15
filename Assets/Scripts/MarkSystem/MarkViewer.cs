using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class MarkViewer : MonoBehaviour
{
    [SerializeField] private MarkNetwork markNetwork;
    [SerializeField] private GameObject markModel;
    [SerializeField] private Transform startPositionMarks;
    [SerializeField] private float markPositionOffset;

    private readonly List<Renderer> _marksRenderer = new(10);

    private void OnEnable()
    {
        markNetwork.MarkChanged += SetMarks;
    }

    private void OnDisable()
    {
        markNetwork.MarkChanged -= SetMarks;
    }

    private void SetMarks()
    {
        HideMarksRenderer();
        var marks = markNetwork.LocalMarks;
        for (int i = 0; i < marks.Count; i++)
        {
            if (_marksRenderer.Count <= i)
            {
                var mark = Instantiate(markModel, startPositionMarks);
                var startPos = startPositionMarks.transform.position;
                mark.transform.position = new Vector3(startPos.x, startPos.y + markPositionOffset * i,
                    startPos.z);
                var rendererComponent = mark.GetComponent<Renderer>();
                _marksRenderer.Add(rendererComponent);
            }

            _marksRenderer[i].material = marks[i].Item2.markColor;
            _marksRenderer[i].enabled = true;
        }
        //SetMarksPosition();

        // var markInfo = markHandler.DataDictionary[markInfoNetwork.MarkName.Value];
        // var mark = Instantiate(markModel, startPositionMarks);
        // var rendererComponent = mark.GetComponent<Renderer>();
        // rendererComponent.material = markInfo.markColor;
        // _marks.Add((markInfoNetwork,rendererComponent));
        // SetMarksPosition();
    }

    // private void UnsetMark(MarkInfoNetwork markInfoNetwork)
    // {
    //     int infoNetworkIndex = _marks.FindIndex(x => x.Item1.Equals(markInfoNetwork));
    //     var mark = _marks[infoNetworkIndex];
    //     _marks.Remove(mark);
    //     Destroy(mark.Item2.gameObject);
    //     SetMarksPosition();
    // }

    private void SetMarksPosition()
    {
        // for (int i = 0; i < _marks.Count; i++)
        // {
        //     var startPos = startPositionMarks.transform.position;
        //     _marks[i].Item2.transform.position = new Vector3(startPos.x, startPos.y + markPositionOffset * i,
        //         startPos.z);
        // }
    }

    private void HideMarksRenderer()
    {
        foreach (var markRenderer in _marksRenderer)
        {
            markRenderer.enabled = false;
        }
    }
}