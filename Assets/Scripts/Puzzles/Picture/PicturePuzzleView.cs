using System;
using UnityEngine;
using UnityEngine.UI;

public class PicturePuzzleView : MonoBehaviour
{
    [SerializeField] private PicturePuzzleNetwork picturePuzzleNetwork;
    [SerializeField] private Button[] buttons;

    private void Awake()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            int i1 = i;
            buttons[i].onClick.AddListener(() => picturePuzzleNetwork.ChangeCell(i1));
        }
    }
    
    private void OnEnable()
    {
        OnGridCreated();
        picturePuzzleNetwork.OnGridCreated += OnGridCreated;
        picturePuzzleNetwork.OnCurrentGridChanged += OnCurrentGridChanged;
    }
    
    private void OnDisable()
    {
        picturePuzzleNetwork.OnGridCreated -= OnGridCreated;
        picturePuzzleNetwork.OnCurrentGridChanged -= OnCurrentGridChanged;
    }

    private void OnGridCreated()
    {
        if(picturePuzzleNetwork.LocalCurrentGrid == null) return;
        for (int i = 0; i < picturePuzzleNetwork.LocalCurrentGrid.Length; i++)
        {
            OnCurrentGridChanged(i);
        }
    }

    private void OnCurrentGridChanged(int index)
    {
        Color color;
        
        switch (picturePuzzleNetwork.LocalCurrentGrid[index].CellValue)
        {
            case 0:
                color = Color.red;;
                break;
            case 1:
                color = Color.green;
                break;
            case 2:
                color = Color.blue;
                break;
            case 3:
                color = Color.yellow;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
        
        buttons[index].colors = new ColorBlock
        {
            normalColor = color,
            disabledColor = color,
            pressedColor = color,
            selectedColor = color,
            highlightedColor = color,
            colorMultiplier = 1
        };
    }
}