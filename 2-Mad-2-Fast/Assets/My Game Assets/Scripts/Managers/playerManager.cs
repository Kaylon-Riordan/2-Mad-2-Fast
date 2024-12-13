using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// https://github.com/onewheelstudio/Adventures-in-C-Sharp/blob/main/Split%20Screen/PlayerManager.cs
public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    private static InputPreferenceValidator validator;
    private static PlayerFactory factory;
    public InputPreferences ip;

    [SerializeField]
    private List<GameObject> players = new List<GameObject>();

    private void Awake()
    {
        instance = Singleton<PlayerManager>.get();
        validator = GetComponent<InputPreferenceValidator>();
        factory = GetComponent<PlayerFactory>();
    }

    private void Start()
    {
        // Ensure all input schemes are valid
        ip = validator.validateParameters();

        spawnPlayer(1); spawnPlayer(2);
    }

    private void spawnPlayer(int PlayerNo)
    {
        InputDevice device = GetComponent<InputDevicePlayerMatcher>().getInputDeviceByPlayerNumber(PlayerNo);

        // If the control scheme is set to default or onehanded for a player, determine whether it's keyboard or mouse
        string controlSchemeName = GetComponent<ControlSchemePlayerMatcher>().getControlSchemeByPlayerNumber(PlayerNo);

        players[PlayerNo - 1] = factory.instantiatePlayer(PlayerNo, controlSchemeName, device);

        players[PlayerNo-1].gameObject.transform.position = new Vector3(-30, 5, 45);
    }
}