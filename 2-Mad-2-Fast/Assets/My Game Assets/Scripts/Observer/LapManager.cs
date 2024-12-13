using UnityEngine;

public class LapManager : MonoBehaviour, ICheckpointObserver
{
    [Header("Checkpoints")]
    public GameObject[] checkpoints;
    public GameObject start;
    public GameObject end; //defines the end checkpoint

    [Header("Settings")]
    public float laps = 1; //amount of laps

    public void OnCheckpointTriggered(CT checkpoint, GameObject bike)
    {
        
            PlayerStats playerstats = null;

            if (bike.transform.parent.name.Equals("Player 1(Clone)"))
            {
                playerstats = PlayerStat.ps1;
            }
            else
            {
                playerstats = PlayerStat.ps2;
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
                            print("Already finished");
                        }
                        else
                        {
                            playerstats.finished = true;
                            print("Finished");
                        }
                    }
                    //player passed less checkpoints than exist
                    else
                    {
                        print("Did not go through all checkpoints");
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
                        print($"Started lap {playerstats.currentLap} - {Mathf.FloorToInt(playerstats.currentLapTime / 60)}:{playerstats.currentLapTime % 60:00.000}");
                    }
                    //player passed less checkpoints than exist
                    else
                    {
                        print("Did not go through all checkpoints");
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
                    print($"Correct checkpoint - {Mathf.FloorToInt(playerstats.currentLapTime / 60)}:{playerstats.currentLapTime % 60:00.000}");
                    playerstats.currentCheckpoint++;
                }
                //not going through the right checkpoint -> in our case that means going backwards so reduce the current checkpoint by 1
                else if (checkpoint.gameObject == 
                    checkpoints[i] && i != playerstats.currentCheckpoint)
                {
                    print("Incorrect checkpoint - wrong direction, turn around"); //In our case, because you shouldn't be able to leave the track and miss one it has to mean that the player turned and is driving the wrong direction //TODO
                    playerstats.currentCheckpoint--;
                }
            }

        
    }
}

