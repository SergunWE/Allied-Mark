using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class PuzzleTimeView : MonoBehaviour
{
    [SerializeField] private PuzzleNetwork puzzleNetwork;
    [SerializeField] private TMP_Text puzzleTimeText;

    private TimeSpan _puzzleTime;
    private DateTime _startPuzzleTime;
    private TimeSpan _elapsedTime = TimeSpan.MinValue;

    private void OnEnable()
    {
        puzzleNetwork.StartPuzzleTimeTicks.OnValueChanged += OnFirstTurnMade;
        puzzleNetwork.OnPuzzleComplete += OnPuzzleComplete;
    }

    private void OnDisable()
    {
        puzzleNetwork.StartPuzzleTimeTicks.OnValueChanged -= OnFirstTurnMade;
        puzzleNetwork.OnPuzzleComplete -= OnPuzzleComplete;
    }

    private void Awake()
    {
        _puzzleTime = TimeSpan.FromSeconds(puzzleNetwork.PuzzleTimeSeconds);
        puzzleTimeText.text = _puzzleTime.ToString(@"mm\:ss");
    }

    private void OnFirstTurnMade(long prevValue, long newValue)
    {
        if(newValue == 0) return;
        _startPuzzleTime = new DateTime(newValue);
        StartCoroutine(TimeUpdate());
    }

    private void OnPuzzleComplete()
    {
        StopAllCoroutines();
    }

    private IEnumerator TimeUpdate()
    {
        while (_elapsedTime < _puzzleTime)
        {
            _elapsedTime = DateTime.UtcNow - _startPuzzleTime;
            puzzleTimeText.text = (_puzzleTime - _elapsedTime).ToString(@"mm\:ss");
            yield return new WaitForSeconds(.5f);
        }
    }
}