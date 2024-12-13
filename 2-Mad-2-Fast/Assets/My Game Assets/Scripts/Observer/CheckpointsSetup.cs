using UnityEngine;

public class CheckpointsSetup : MonoBehaviour
{
    
    [SerializeField] private CT[] checkpoints;
    [SerializeField] private LapManager lapManager;

    

    #region StartMethod
    private void Start()
    {
        foreach(var checkpoint in checkpoints)
        {
            checkpoint.Subscribe(lapManager);

        }

        StartRace();
    }

    public void StartRace()
    {
        //reset player stats
        PlayerStat.ps1.finished = false;
        PlayerStat.ps2.finished = false;
        PlayerStat.ps1.currentLap = 1;
        PlayerStat.ps2.currentLap = 1;
        PlayerStat.ps1.currentCheckpoint = 0;
        PlayerStat.ps2.currentCheckpoint = 0;
        PlayerStat.ps1.currentLapTime = 0;
        PlayerStat.ps2.currentLapTime = 0;
        //best lap stays saved
        //best lap time stays saved

        //start race
        PlayerStat.ps1.started = true;
        PlayerStat.ps2.started = true;
    }
    #endregion

    #region UpdateMethod
    private void Update()
    {
        //ps1
        if (PlayerStat.ps1.started && !PlayerStat.ps1.finished)
        {
            PlayerStat.ps1.currentLapTime += Time.deltaTime; //sums up current lap time
            if (PlayerStat.ps1.bestLap == 0) //if lap time is 0 no best lap was made yet, so the first one is the best one
            {
                PlayerStat.ps1.bestLap = 1;
            }
        }

        if (PlayerStat.ps1.started)
        {
            if (PlayerStat.ps1.bestLap == PlayerStat.ps1.currentLap) //if the game is started and no best lap time is made yet the best lap timer goes up equal to the current lap timer
            {
                PlayerStat.ps1.bestLapTime = PlayerStat.ps1.currentLapTime;
            }
        }

        //ps2
        if (PlayerStat.ps2.started && !PlayerStat.ps2.finished)
        {
            PlayerStat.ps2.currentLapTime += Time.deltaTime; //sums up current lap time
            if (PlayerStat.ps2.bestLap == 0) //if lap time is 0 no best lap was made yet, so the first one is the best one
            {
                PlayerStat.ps2.bestLap = 1;
            }
        }

        if (PlayerStat.ps2.started)
        {
            if (PlayerStat.ps2.bestLap == PlayerStat.ps2.currentLap) //if the game is started and no best lap time is made yet the best lap timer goes up equal to the current lap timer
            {
                PlayerStat.ps2.bestLapTime = PlayerStat.ps2.currentLapTime;
            }
        }


    }
    #endregion

    private void OnGUI()
    {
        //shows the current time on gui
        string formattedCurrentTime = $"Current: {Mathf.FloorToInt(PlayerStat.ps1.currentLapTime / 60)}:{PlayerStat.ps1.currentLapTime % 60:00.000} - (Lap {PlayerStat.ps1.currentLap})";
        GUI.Label(new Rect(50, 10, 250, 100), formattedCurrentTime);

        //shows the best time on gui
        string formattedBestTime = $"Best: {Mathf.FloorToInt(PlayerStat.ps1.bestLapTime / 60)}:{PlayerStat.ps1.bestLapTime % 60:00.000} - (Lap {PlayerStat.ps1.bestLap})";
        GUI.Label(new Rect(250, 10, 250, 100), formattedBestTime);

        //shows the current time on gui
        string formattedCurrentTime2 = $"Current: {Mathf.FloorToInt(PlayerStat.ps2.currentLapTime / 60)}:{PlayerStat.ps2.currentLapTime % 60:00.000} - (Lap {PlayerStat.ps2.currentLap})";
        GUI.Label(new Rect(600, 10, 250, 100), formattedCurrentTime2);

        //shows the best time on gui
        string formattedBestTime2 = $"Best: {Mathf.FloorToInt(PlayerStat.ps2.bestLapTime / 60)}:{PlayerStat.ps2.bestLapTime % 60:00.000} - (Lap {PlayerStat.ps2.bestLap})";
        GUI.Label(new Rect(800, 10, 250, 100), formattedBestTime2);
    }
}