using NetworkFramework.Data;
using NetworkFramework.MonoBehaviour_Components;
using NetworkFramework.SO;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyMenuController : MonoBehaviour
{
    [SerializeField] private TMP_Text lobbyCodeText;
    [SerializeField] private TMP_Text readyButtonText;
    [SerializeField] private LobbyRefresherComponent refresherComponent;
    
    [SerializeField] private LobbyInternalPlayerData internalPlayerData;

    private void Start()
    {
        if (!LobbyData.Exist) return;
        lobbyCodeText.text = $"{LobbyData.Current.Name} - code:{LobbyData.Current.LobbyCode}";
    }

    public void LeaveLobby()
    {
        refresherComponent.LeaveLobby();
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