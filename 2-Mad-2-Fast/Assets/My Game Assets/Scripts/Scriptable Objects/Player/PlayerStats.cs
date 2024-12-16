using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStats", menuName = "PedalFury/PlayerStats")]

// Store player progress in a scriptable object
public class PlayerStats : ScriptableObject
{
    public float currentCheckpoint;
    public float currentLap;
    public bool started;
    public bool finished;
    public float currentLapTime;
    public float bestLapTime;
    public float bestLap;
}