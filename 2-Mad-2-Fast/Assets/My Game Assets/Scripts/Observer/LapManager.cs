using UnityEngine;

/// <summary>
/// Manager for the laps routine, takes responsibility about the lap routine
/// </summary>
public class LapManager : MonoBehaviour, ICheckpointObserver
{
    #region variables
    [Header("Checkpoints")]
    public GameObject[] checkpoints;
    public GameObject start;
    public GameObject end; //defines the end checkpoint
    [Header("Settings")]
    public float laps = 1; //amount of laps
    #endregion

    #region routine function
    /// <summary>
    /// This function is called when the checkpoint was triggered, it defines the whole routine for laps
    /// </summary>
    /// <param name="checkpoint"> The checkpoint that was triggered </param>
    /// <param name="bike"> The bike that triggered the checkpoint (player 1 or player 2) </param>
    public void OnCheckpointTriggered(CheckpointTrigger checkpoint, GameObject bike)
    {
        PlayerStats playerstats = null;
        if (bike.transform.parent.name.Equals("Player 1(Clone)"))
        {
            playerstats = PlayerStatsStorage.ps1;
        }
        else
        {
            playerstats = PlayerStatsStorage.ps2;
        }
        //end the lap or race
        if (checkpoint.gameObject == end && playerstats.started)
        {
            //if all laps are finished, end the race
            if (playerstats.currentLap == laps)
            {
                //check if the currentCheckpoint matches the checkpoints length
                if (playerstats.currentCheckpoint ==
                checkpoints.Length)
                {
                    //update best lap if a new best was made
                    if (playerstats.currentLapTime < playerstats.bestLapTime)
                    {
                        playerstats.bestLap = playerstats.currentLap;
                    }
                    //check if the race is already finished
                    if (playerstats.finished)
                    {
                        Debug.Log($"Already finished");
                    }
                    else
                    {
                        playerstats.finished = true;
                        Debug.Log($"Finished");
                    }
                }
                //player passed less checkpoints than exist
                else
                {
                    Debug.Log($"Did not go through all checkpoints");
                }
            }
            //if all laps are not finished, start a new lap
            else if (playerstats.currentLap < laps)
            {
                //check again if the currentCheckpoint matches the checkpoints length, if so then start a new lap
                if (playerstats.currentCheckpoint ==
                checkpoints.Length)
                {
                    //update best lap and best lap time if a new best was made
                    if (playerstats.currentLapTime < playerstats.bestLapTime)
                    {
                        playerstats.bestLap = playerstats.currentLap;
                        playerstats.bestLapTime = playerstats.currentLapTime;
                    }

                    playerstats.currentLap++; //increase lap
                    playerstats.currentCheckpoint = 0; //reset checkpoints 
                    playerstats.currentLapTime = 0; //reset time

                    Debug.Log($"Started lap {playerstats.currentLap} - {Mathf.FloorToInt(playerstats.currentLapTime / 60)}:{playerstats.currentLapTime % 60:00.000}");
                }
                //player passed less checkpoints than exist
                else
                {

                    Debug.Log($"Did not go through all checkpoints");
                }
            }

        }
        //check which checkpoint the played passed and if its the right or false one
        for (int i = 0; i <
                checkpoints.Length; i++)
        {
            if (playerstats.finished || !playerstats.started)
            {
                return; //the game is finished or has not started yet
            }
            //going through the right checkpoint -> increase the current checkpoint by 1
            if (checkpoint.gameObject ==
                checkpoints[i] && i == playerstats.currentCheckpoint)
            {
                Debug.Log($"Correct checkpoint - {Mathf.FloorToInt(playerstats.currentLapTime / 60)}:{playerstats.currentLapTime % 60:00.000}");
                playerstats.currentCheckpoint++;
            }
            //not going through the right checkpoint -> in our case that means going backwards so reduce the current checkpoint by 1
            else if (checkpoint.gameObject ==
                checkpoints[i] && i != playerstats.currentCheckpoint)
            {
                Debug.Log($"Incorrect checkpoint - wrong direction, turn around");
                playerstats.currentCheckpoint--;
            }
        }
    }
    #endregion
}

