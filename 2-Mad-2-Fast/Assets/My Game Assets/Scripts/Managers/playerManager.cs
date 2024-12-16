using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// https://github.com/onewheelstudio/Adventures-in-C-Sharp/blob/main/Split%20Screen/PlayerManager.cs
/// <summary>
/// Stores players, gathers parameters to initialise players in PlayerFactory
/// </summary>
public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    private static InputPreferenceValidator validator;
    private static PlayerFactory factory;
    public InputPreferences ip;

    [SerializeField]
    public GameObject[] players = new GameObject[2];

    private void Awake()
    {
        instance = Singleton<PlayerManager>.get();
        validator = GetComponent<InputPreferenceValidator>();
        factory = GetComponent<PlayerFactory>();
    }

    private void Start()
    {
        // Ensure all input schemes are valid
        validator.validateParameters();

        spawnPlayer(1); spawnPlayer(2);
    }

    /// <summary>
    /// Gets all necessary data to instantiate player and calls PlayerFactory to instantiate it
    /// </summary>
    /// <param name="PlayerNo">An int of value 1 or 2 denoting whether it's player 1 or player 2 to be spawned</param>
    private void spawnPlayer(int PlayerNo)
    {
        InputDevice device = GetComponent<InputDevicePlayerMatcher>().getInputDeviceByPlayerNumber(PlayerNo);

        // If the control scheme is set to default or onehanded for a player, determine whether it's keyboard or mouse
        string controlSchemeName = GetComponent<ControlSchemePlayerMatcher>().getControlSchemeByPlayerNumber(PlayerNo);

        players[PlayerNo - 1] = factory.instantiatePlayer(PlayerNo, controlSchemeName, device);

        players[PlayerNo - 1].gameObject.transform.position = new Vector3(-30, 5, 45);
    }
}