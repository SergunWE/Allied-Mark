using System;
using UnityEngine;
using UnityEngine.UI;

public class PicturePuzzleTargetView : MonoBehaviour
{
    [SerializeField] private PicturePuzzleNetwork picturePuzzleNetwork;
    
    [SerializeField] private Button[] targetButtons;

    private void OnEnable()
    {
        picturePuzzleNetwork.OnGridCreated += OnGridChanged;
    }
    
    private void OnDisable()
    {
        picturePuzzleNetwork.OnGridCreated -= OnGridChanged;
    }
    
    private void OnGridChanged()
    {
        for (int i = 0; i < picturePuzzleNetwork.LocalTargetGrid.Length; i++)
        {
            Color color;
            switch (picturePuzzleNetwork.LocalTargetGrid[i].Value)
            {
                case 0:
                    color = Color.red;
                    ;
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


            targetButtons[i].colors = new ColorBlock
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
}