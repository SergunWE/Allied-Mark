using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private TMP_Text stateText;

    public void OnLobbyCreated(bool successful)
    {
        stateText.text = successful ? "Lobby created" : "Lobby creation error";

        GoLobbyScene(successful);
    }

    public void OnLobbyJoined(bool successful)
    {
        stateText.text = successful ? "Joined to the lobby" : "Error joining the lobby";

        GoLobbyScene(successful);
    }

    private void GoLobbyScene(bool state)
    {
        if (state)
        {
            SceneManager.LoadSceneAsync(2);
        }
    }
}