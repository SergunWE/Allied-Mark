using System;
using NetworkFramework.Data;
using NetworkFramework.MonoBehaviour_Components;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private TMP_Text state;
    [SerializeField] private TMP_InputField lobbyName;
    [SerializeField] private TMP_InputField lobbyMaxPlayer;
    [SerializeField] private Toggle lobbyPrivacy;

    [SerializeField] private LobbyOptionsSo lobbyOptions;
    
    private void Awake()
    {
        lobbyName.text = lobbyOptions.LobbyName;
        lobbyMaxPlayer.text = lobbyOptions.MaxPlayer.ToString();
        lobbyPrivacy.isOn = lobbyOptions.Privacy;
    }

    public void OnLobbyCreated(bool successful)
    {
        state.text = successful ? "Lobby created" : "Lobby creation error";

        GoLobbyScene(successful);
    }

    public void OnLobbyJoined(bool successful)
    {
        state.text = successful ? "Joined to the lobby" : "Error joining the lobby";

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