using UnityEngine;

/// <summary>
/// Setup class to setup the checkpoints, laps, managers
/// </summary>
public class CheckpointsSetup : MonoBehaviour
{
    #region variables
    [SerializeField] private CheckpointTrigger[] checkpoints; //checkpoints
    [SerializeField] private LapManager lapManager; //manager for everything
    #endregion


    #region StartMethod
    /// <summary>
    /// Start method, takes all subjects and subscribes them to the manager
    /// </summary>
    private void Start()
    {
        foreach (var checkpoint in checkpoints)
        {
            checkpoint.Subscribe(lapManager);

        }

        StartRace(); //start race
    }

    /// <summary>
    /// Sets all values for the PlayerStats of each player
    /// </summary>
    public void StartRace()
    {
        //reset player stats
        PlayerStatsStorage.ps1.finished = false;
        PlayerStatsStorage.ps2.finished = false;
        PlayerStatsStorage.ps1.currentLap = 1;
        PlayerStatsStorage.ps2.currentLap = 1;
        PlayerStatsStorage.ps1.currentCheckpoint = 0;
        PlayerStatsStorage.ps2.currentCheckpoint = 0;
        PlayerStatsStorage.ps1.currentLapTime = 0;
        PlayerStatsStorage.ps2.currentLapTime = 0;
        //start race
        PlayerStatsStorage.ps1.started = true;
        PlayerStatsStorage.ps2.started = true;
    }
    #endregion

    #region UpdateMethod
    /// <summary>
    /// Update method takes responsibility to update the time of each players for the laps.
    /// </summary>
    private void Update()
    {
        //ps1
        if (PlayerStatsStorage.ps1.started && !PlayerStatsStorage.ps1.finished)
        {
            PlayerStatsStorage.ps1.currentLapTime += Time.deltaTime; //sums up current lap time
            if (PlayerStatsStorage.ps1.bestLap == 0) //if lap time is 0 no best lap was made yet, so the first one is the best one
            {
                PlayerStatsStorage.ps1.bestLap = 1;
            }
        }

        if (PlayerStatsStorage.ps1.started)
        {
            if (PlayerStatsStorage.ps1.bestLap == PlayerStatsStorage.ps1.currentLap) //if the game is started and no best lap time is made yet the best lap timer goes up equal to the current lap timer
            {
                PlayerStatsStorage.ps1.bestLapTime = PlayerStatsStorage.ps1.currentLapTime;
            }
        }

        //ps2
        if (PlayerStatsStorage.ps2.started && !PlayerStatsStorage.ps2.finished)
        {
            PlayerStatsStorage.ps2.currentLapTime += Time.deltaTime; //sums up current lap time
            if (PlayerStatsStorage.ps2.bestLap == 0) //if lap time is 0 no best lap was made yet, so the first one is the best one
            {
                PlayerStatsStorage.ps2.bestLap = 1;
            }
        }

        if (PlayerStatsStorage.ps2.started)
        {
            if (PlayerStatsStorage.ps2.bestLap == PlayerStatsStorage.ps2.currentLap) //if the game is started and no best lap time is made yet the best lap timer goes up equal to the current lap timer
            {
                PlayerStatsStorage.ps2.bestLapTime = PlayerStatsStorage.ps2.currentLapTime;
            }
        }


    }
    #endregion

    #region GUI for debugging
    /**
    /// <summary>
    /// OnGUI function to show time and laps on screen for debugging
    /// </summary>
    private void OnGUI()
    {
        //shows the current time on gui
        string formattedCurrentTime = $"Current: {Mathf.FloorToInt(PlayerStatsStorage.ps1.currentLapTime / 60)}:{PlayerStatsStorage.ps1.currentLapTime % 60:00.000} - (Lap {PlayerStatsStorage.ps1.currentLap})";
        GUI.Label(new Rect(50, 10, 250, 100), formattedCurrentTime);

        //shows the best time on gui
        string formattedBestTime = $"Best: {Mathf.FloorToInt(PlayerStatsStorage.ps1.bestLapTime / 60)}:{PlayerStatsStorage.ps1.bestLapTime % 60:00.000} - (Lap {PlayerStatsStorage.ps1.bestLap})";
        GUI.Label(new Rect(250, 10, 250, 100), formattedBestTime);

        //shows the current time on gui
        string formattedCurrentTime2 = $"Current: {Mathf.FloorToInt(PlayerStatsStorage.ps2.currentLapTime / 60)}:{PlayerStatsStorage.ps2.currentLapTime % 60:00.000} - (Lap {PlayerStatsStorage.ps2.currentLap})";
        GUI.Label(new Rect(600, 10, 250, 100), formattedCurrentTime2);

        //shows the best time on gui
        string formattedBestTime2 = $"Best: {Mathf.FloorToInt(PlayerStatsStorage.ps2.bestLapTime / 60)}:{PlayerStatsStorage.ps2.bestLapTime % 60:00.000} - (Lap {PlayerStatsStorage.ps2.bestLap})";
        GUI.Label(new Rect(800, 10, 250, 100), formattedBestTime2);
    }
    */
    #endregion
}