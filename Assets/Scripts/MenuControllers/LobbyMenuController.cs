using NetworkFramework.LobbyCore;
using NetworkFramework.MonoBehaviour_Components;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyMenuController : MonoBehaviour
{
    [SerializeField] private TMP_Text lobbyCodeText;
    [SerializeField] private LobbyRefresherComponent refresherComponent;

    private void Start()
    {
        lobbyCodeText.text =  $"Lobby code:{LobbyData.Current.LobbyCode}";
    }

    public void LeaveLobby()
    {
        refresherComponent.LeaveLobby();
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadSceneAsync(1);
    }
}
