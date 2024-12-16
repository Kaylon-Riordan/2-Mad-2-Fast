using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages state of the game e.g whether a player has won the race or not.
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField]
    private AudioClip gameFinished;

    private LapManager lm;

    private void Awake()
    {
        instance = Singleton<GameManager>.get();
        lm = LapManager.instance;

        UIManager.instance.winScreen.enabled = false;
        LapManager.gameFinished += GameFinished;
    }

    /// <summary>
    /// Undoes changes made to singletons that carry over between scenes
    /// </summary>
    public void GameReset()
    {
        AudioManager.instance.gameObject.GetComponent<DynamicMusic>().enabled = true;

        for (int i = 0; i < 2; i++)
            UIManager.instance.winScreen.canvasImages[i].gameObject.SetActive(false);

        SceneManager.LoadScene(1);
    }

    /// <summary>
    /// On game finished, take away player's control and display win screens.
    /// </summary>
    void GameFinished()
    {
        Debug.Log("Game Over");
        DynamicMusic.instance.MuteAll();
        DynamicMusic.instance.enabled = false;
        AudioManager.instance.PlaySound(gameFinished, AudioMixerGroupName.SFX, transform.position);

        for (int i = 0; i < 2; i++)
        {
            PlayerManager.instance.players[i].GetComponent<PlayerInputHandler>().enabled = false;
            UIManager.instance.winScreen.SetWinScreen(i + 1, LapManager.instance.ps[i].finished);
            UIManager.instance.winScreen.canvasImages[i].gameObject.SetActive(true);
        }
    }
}
