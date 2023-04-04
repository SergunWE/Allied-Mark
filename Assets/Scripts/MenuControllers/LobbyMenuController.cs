using System.Collections.Generic;
using NetworkFramework.Data;
using NetworkFramework.SO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyMenuController : MonoBehaviour
{
    [SerializeField] private TMP_Text lobbyCodeText;
    [SerializeField] private TMP_Text readyButtonText;
    [SerializeField] private GameObject hostPanel;
    [SerializeField] private TMP_Dropdown levelDifficultyDropdown;

    [SerializeField] private LobbyInternalPlayerData internalPlayerData;
    [SerializeField] private LevelDifficultyHandler levelDifficultyHandler;

    private void Start()
    {
        if (!LobbyData.Exist) return;
        lobbyCodeText.text = $"{LobbyData.Current.Name} - {LobbyData.Current.LobbyCode}";
        hostPanel.SetActive(internalPlayerData.PlayerHost);
        int levelsCount = levelDifficultyHandler.DifficultyCount;
        var options = new List<TMP_Dropdown.OptionData>();
        for (int i = 0; i < levelsCount; i++)
        {
            var difficulty = levelDifficultyHandler.GetDifficult(i);
            options.Add(new TMP_Dropdown.OptionData(difficulty.DifficultName));
        }

        levelDifficultyDropdown.options = options;
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadSceneAsync(1);
    }

    public void OnPlayerDataUpdated()
    {
        readyButtonText.text = internalPlayerData.PlayerReady ? "Not ready" : "Ready";
    }
}