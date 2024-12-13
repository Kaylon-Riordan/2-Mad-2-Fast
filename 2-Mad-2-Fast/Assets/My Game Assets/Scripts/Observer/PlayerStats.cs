using System;
using UnityEngine;

public class PlayerStat
{
    [Header("Player Stats")]
    public static PlayerStats ps1 = new PlayerStats(0, 1, false, false, 0, 0, 0);
    public static PlayerStats ps2 = new PlayerStats(0, 1, false, false, 0, 0, 0);
}

public class PlayerStats
{
    public float currentCheckpoint; //index of current checkpoint -> starts at 0
    public float currentLap; //couts the laps -> starts at 1
    public bool started; //false or true for the game to be started
    public bool finished; //false or true for the game to be finished

    public float currentLapTime; //time of the current lap
    public float bestLapTime; //time of the best lap
    public float bestLap; //number of the lap with the best time

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


}
