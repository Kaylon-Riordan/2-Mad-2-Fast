using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Manages the options menu panel
/// </summary>
public class OptionsMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject player1Obj; 
    [SerializeField]
    private GameObject player2Obj;

    [HideInInspector]
    public PlayerPanel player1Panel;
    [HideInInspector]
    public PlayerPanel player2Panel;

    public GameObject eventSystem;

    private void Awake()
    {
        player1Panel = player1Obj.GetComponent<PlayerPanel>();
        player2Panel = player2Obj.GetComponent<PlayerPanel>();
    }

    /// <summary>
    /// Hides player 2's options if either scheme is set to shared.
    /// </summary>
    public void changeLayout()
    {
        // 2 is the index of shared on the drop down
        if(player1Panel.getControlScheme() == ControlScheme.Shared || player2Panel.getControlScheme() == ControlScheme.Shared)
        {
            if (player2Panel.getControlScheme() == ControlScheme.Shared)
            {
                // The menu gets locked if player 2 is left on shared
                player1Panel.setControlScheme(ControlScheme.Shared);
                player2Panel.setControlScheme(ControlScheme.Default);
            }

            player2Obj.SetActive(false);
            player1Panel.sharedLayout();
        } 
        // Neither is set to shared so set it to default
        else
        {
            player2Obj.SetActive(true);
            player1Panel.defaultLayout();
        }
    }

    /// <summary>
    /// Highlights the leftHandToggle button. Allowes controllers and keyboards to function.
    /// </summary>
    public void highlightP1LeftHandToggle()
    {
        eventSystem.GetComponent<EventSystem>().SetSelectedGameObject(player1Panel.leftHandToggle);
    }
}
