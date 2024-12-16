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
        LapManager.instance.ps[0].finished = false;
        LapManager.instance.ps[1].finished = false;
        LapManager.instance.ps[0].currentLap = 1;
        LapManager.instance.ps[1].currentLap = 1;
        LapManager.instance.ps[0].currentCheckpoint = 0;
        LapManager.instance.ps[1].currentCheckpoint = 0;
        LapManager.instance.ps[0].currentLapTime = 0;
        LapManager.instance.ps[1].currentLapTime = 0;
        //start race
        LapManager.instance.ps[0].started = true;
        LapManager.instance.ps[1].started = true;
    }
    #endregion

    #region UpdateMethod
    /// <summary>
    /// Update method takes responsibility to update the time of each players for the laps.
    /// </summary>
    private void Update()
    {
        for (int i = 0; i < 2; i++)
        {
            //ps[0]
            if (LapManager.instance.ps[i].started && !LapManager.instance.ps[0].finished)
            {
                LapManager.instance.ps[i].currentLapTime += Time.deltaTime; //sums up current lap time
                if (LapManager.instance.ps[i].bestLap == 0) //if lap time is 0 no best lap was made yet, so the first one is the best one
                {
                    LapManager.instance.ps[i].bestLap = 1;
                }
            }

            if (LapManager.instance.ps[i].started)
            {
                if (LapManager.instance.ps[i].bestLap == LapManager.instance.ps[0].currentLap) //if the game is started and no best lap time is made yet the best lap timer goes up equal to the current lap timer
                {
                    LapManager.instance.ps[i].bestLapTime = LapManager.instance.ps[0].currentLapTime;
                }
            }
        }

    }
    #endregion

    #region GUI for debugging
    
    /// <summary>
    /// OnGUI function to show time and laps on screen for debugging
    /// </summary>
    private void OnGUI()
    {
        //shows the current time on gui
        string formattedCurrentTime = $"Current: {Mathf.FloorToInt(LapManager.instance.ps[0].currentLapTime / 60)}:{LapManager.instance.ps[0].currentLapTime % 60:00.000} - (Lap {LapManager.instance.ps[0].currentLap})";
        GUI.Label(new Rect(50, 10, 250, 100), formattedCurrentTime);

        //shows the best time on gui
        string formattedBestTime = $"Best: {Mathf.FloorToInt(LapManager.instance.ps[0].bestLapTime / 60)}:{LapManager.instance.ps[0].bestLapTime % 60:00.000} - (Lap {LapManager.instance.ps[0].bestLap})";
        GUI.Label(new Rect(250, 10, 250, 100), formattedBestTime);

        //shows the current time on gui
        string formattedCurrentTime2 = $"Current: {Mathf.FloorToInt(LapManager.instance.ps[1].currentLapTime / 60)}:{LapManager.instance.ps[1].currentLapTime % 60:00.000} - (Lap {LapManager.instance.ps[1].currentLap})";
        GUI.Label(new Rect(Screen.width - 400, 10, 250, 100), formattedCurrentTime2);

        //shows the best time on gui
        string formattedBestTime2 = $"Best: {Mathf.FloorToInt(LapManager.instance.ps[1].bestLapTime / 60)}:{LapManager.instance.ps[1].bestLapTime % 60:00.000} - (Lap {LapManager.instance.ps[1].bestLap})";
        GUI.Label(new Rect(Screen.width - 200, 10, 250, 100), formattedBestTime2);
    }
    
    #endregion
}