using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

/// <summary>
/// Main menu object
/// </summary>
public class MainMenu : MonoBehaviour
{
    public GameObject eventSystem;
    public GameObject playButton;
    public GameObject optionsButton;

    /// <summary>
    /// Starts the game
    /// </summary>
    public void playGame()
    {
        SceneManager.LoadSceneAsync(1);
    }

    /// <summary>
    /// Highlights the play button. For Controllers and keyboards
    /// </summary>
    public void highlightPlayerButton()
    {
        eventSystem.GetComponent<EventSystem>().SetSelectedGameObject(playButton);
    }

    /// <summary>
    /// Highlights the settings button. Triggered on close of options panel
    /// </summary>
    public void highlightSettingsButton()
    {
        eventSystem.GetComponent<EventSystem>().SetSelectedGameObject(optionsButton);
    }
}
