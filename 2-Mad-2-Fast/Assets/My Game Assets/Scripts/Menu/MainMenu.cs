using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;


public class MainMenu : MonoBehaviour
{
    public GameObject eventSystem;
    public GameObject playButton;
    public GameObject optionsButton;

    public void playGame()
    {
        SceneManager.LoadSceneAsync(1);
    }

    public void highlightPlayerButton()
    {
        eventSystem.GetComponent<EventSystem>().SetSelectedGameObject(playButton);
    }

    public void highlightSettingsButton()
    {
        eventSystem.GetComponent<EventSystem>().SetSelectedGameObject(optionsButton);
    }
}
