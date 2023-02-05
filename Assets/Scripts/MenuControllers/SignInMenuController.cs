using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SignInMenuController : MonoBehaviour
{
    [SerializeField] private TMP_Text stateText;
    [SerializeField] private GameObject signInButtonPanel;
    [SerializeField] private GameObject initButton;
    
    public void OnInitComplete(bool successful)
    {
        signInButtonPanel.SetActive(successful);
        initButton.SetActive(!successful);
        
        stateText.text = successful ? "Successful initialization. Login" 
            : "Unsuccessful initialization. Try again";
    }

    public void OnSignInComplete(bool successful)
    {
        stateText.text = successful ? "Successful login" 
            : "Failed login. Try again.";

        if (successful)
        {
            SceneManager.LoadSceneAsync(1);
        }
    }
}
