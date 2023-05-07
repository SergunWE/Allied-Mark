using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SignInMenuController : MonoBehaviour
{
    [SerializeField] private TMP_Text stateText;
    [SerializeField] private GameObject signInButtonPanel;
    [SerializeField] private GameObject initButton;
    [SerializeField] private GameObject loadScreen;

    private void Awake()
    {
        stateText.text = "";
    }

    public void OnInitComplete(bool successful)
    {
        signInButtonPanel.SetActive(successful);
        initButton.SetActive(!successful);
        
        stateText.text = successful ? "" 
            : "Unsuccessful initialization. Try again";
    }

    public void OnSignInComplete(bool successful)
    {
        stateText.text = successful ? "" : "Failed login. Try again.";

        if (successful)
        {
            loadScreen.SetActive(true);
            SceneManager.LoadSceneAsync(1);
        }
        else
        {
            loadScreen.SetActive(false);
        }
    }
}
