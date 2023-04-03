using System.Collections.Generic;
using NetworkFramework;
using NetworkFramework.LobbyCore;
using NetworkFramework.MonoBehaviour_Components;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyMenuController : MonoBehaviour
{
    [SerializeField] private TMP_Text lobbyCodeText;
    [SerializeField] private LobbyRefresherComponent refresherComponent;

    private void Start()
    {
        if(!LobbyData.Exist) return;
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

    public async void DebugChangeName()
    {
        var t =await LobbyService.Instance.UpdatePlayerAsync(LobbyData.Current.Id, AuthenticationService.Instance.PlayerId,
            new UpdatePlayerOptions()
            {
                Data = new Dictionary<string, PlayerDataObject>()
                {
                    {
                        DataKeysConstants.PlayerReady.Key,
                        new PlayerDataObject(DataKeysConstants.PlayerReady.Visibility, "True")
                    }
                }
            });
    }
}