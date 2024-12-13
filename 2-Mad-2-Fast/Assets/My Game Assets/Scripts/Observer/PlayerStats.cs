using System;
using UnityEngine;

/// <summary>
/// Storage class that contains two player stats object for each player
/// </summary>
public class PlayerStatsStorage
{
    #region variables
    [Header("Player Stats")]
    public static PlayerStats ps1 = new PlayerStats(0, 1, false, false, 0, 0, 0);
    public static PlayerStats ps2 = new PlayerStats(0, 1, false, false, 0, 0, 0);
    #endregion
}

/// <summary>
/// Playerstats class that contains information about the player stats like time and laps
/// </summary>
public class PlayerStats
{
    #region variables
    public float currentCheckpoint;
    public float currentLap;
    public bool started;
    public bool finished;
    public float currentLapTime;
    public float bestLapTime;
    public float bestLap;
    #endregion

    #region constructor
    /// <summary>
    /// Contrsuctor for player stats
    /// </summary>
    /// <param name="currentCheckpoint"> The current checkpoint the player has passed, used to check the next valid checkpoint </param>
    /// <param name="currentLap"> The current lap the player is in </param>
    /// <param name="started"> If the player has started </param>
    /// <param name="finished"> If the player has finished </param>
    /// <param name="currentLapTime"> The current time for the lap the player is driving in </param>
    /// <param name="bestLapTime"> The best time the player has driven in this round </param>
    /// <param name="bestLap"> The lap in which the player has driven the best time in this round </param>
    public PlayerStats(float currentCheckpoint, float currentLap, bool started, bool finished, float currentLapTime, float bestLapTime, float bestLap)
    {
        this.currentCheckpoint = currentCheckpoint;
        this.currentLap = currentLap;
        this.started = started;
        this.finished = finished;
        this.currentLapTime = currentLapTime;
        this.bestLapTime = bestLapTime;
        this.bestLap = bestLap;
    }
    #endregion
}
