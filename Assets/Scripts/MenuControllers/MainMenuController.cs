using NetworkFramework.SO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private GameObject loadScreen;
    [SerializeField] private TMP_Text state;
    [SerializeField] private TMP_InputField lobbyName;
    [SerializeField] private Toggle lobbyPrivacy;
    [SerializeField] private TMP_InputField playerName;
    [SerializeField] private TMP_Dropdown levelDropdown;

    [SerializeField] private LobbyOptions lobbyOptions;
    [SerializeField] private LobbyInternalPlayerData internalPlayerData;
    [SerializeField] private LobbyInternalData internalData;

    private void Awake()
    {
        loadScreen.SetActive(false);
        state.text = "";
        OnLobbyOptionsChanged();
        OnPlayerOptionsChanged();
    }

    public void OnLobbyOptionsChanged()
    {
        lobbyName.text = lobbyOptions.LobbyName;
        lobbyPrivacy.isOn = lobbyOptions.Privacy;
        levelDropdown.value = internalData.Level;
    }

    public void OnPlayerOptionsChanged()
    {
        playerName.text = internalPlayerData.PlayerName;
    }

    public void OnLobbyCreated(bool successful)
    {
        state.text = successful ? "" : "Lobby creation error";

        GoLobbyScene(successful);
    }

    public void OnLobbyJoined(bool successful)
    {
        state.text = successful ? "" : "Error joining the lobby";

        GoLobbyScene(successful);
    }

    private void GoLobbyScene(bool joinedState)
    {
        if (!joinedState) return;
        loadScreen.SetActive(true);
        SceneManager.LoadSceneAsync(2);
    }
}