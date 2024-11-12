using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// https://github.com/onewheelstudio/Adventures-in-C-Sharp/blob/main/Split%20Screen/PlayerManager.cs
public class PlayerManager : MonoBehaviour
{
    [SerializeField]
    private List<PlayerInput> players = new List<PlayerInput>();
    [SerializeField]
    private List<Transform> spawnPoints;
    [SerializeField]
    private List<LayerMask> playerLayers;

    private PlayerInputManager playerInputManager;

    private void Awake()
    {
        playerInputManager = GetComponent<PlayerInputManager>();
    }

    private void OnEnable()
    {
        playerInputManager.onPlayerJoined += AddPlayer;
    }

    private void OnDisable()
    {
        playerInputManager.onPlayerJoined -= AddPlayer;
    }

    // https://github.com/onewheelstudio/Adventures-in-C-Sharp/blob/main/Split%20Screen/PlayerManager.cs
    public void AddPlayer(PlayerInput player)
    {
        players.Add(player);
    }
}
