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

    [SerializeField] private LobbyInternalPlayerData internalPlayerData;

    private void Start()
    {
        if (!LobbyData.Exist) return;
        hostPanel.SetActive(internalPlayerData.PlayerHost);
        lobbyCodeText.text = $"{LobbyData.Current.Name} - {LobbyData.Current.LobbyCode}";
        
    }

    public void OnLobbyLeaved()
    {
        SceneManager.LoadSceneAsync(1);
    }

    public void OnLobbyRefreshed()
    {
        readyButtonText.text = internalPlayerData.PlayerReady ? "Не готов" : "Готов";
    }

    public void OnGameStarted()
    {
        SceneManager.LoadScene(3);
    }
}