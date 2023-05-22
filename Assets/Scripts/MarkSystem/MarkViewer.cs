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
    }

    private void HideMarksRenderer()
    {
        foreach (var markRenderer in _marksRenderer)
        {
            markRenderer.enabled = false;
        }
    }
}