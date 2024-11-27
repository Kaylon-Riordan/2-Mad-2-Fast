using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    //public GameObject bike;

    // https://github.com/Ben-Keev/Tower/blob/main/Assets/Scripts/GameManager.cs.cs
    public static CheckpointManager instance;

    private void initSingleton()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Awake()
    {
        initSingleton();
    }



    [Header("Checkpoints")]
    public GameObject start; //defines the start checkpoint
    public GameObject end; //defines the end checkpoint
    public GameObject[] checkpoints; //all other checkpoints

    [Header("Settings")]
    public float laps = 1; //amount of laps

    [Header("Player Stats")]
    PlayerStats ps1 = new PlayerStats(0,1,false,false,0,0,0);
    PlayerStats ps2 = new PlayerStats(0, 1, false, false, 0, 0, 0);



    private void Start()
    {
        StartRace();
    }

    private void Update()
    {
        //ps1
        if (ps1.started && !ps1.finished)
        {
            ps1.currentLapTime += Time.deltaTime; //sums up current lap time
            if (ps1.bestLap == 0) //if lap time is 0 no best lap was made yet, so the first one is the best one
            {
                ps1.bestLap = 1;
            }
        }

        if (ps1.started)
        {
            if (ps1.bestLap == ps1.currentLap) //if the game is started and no best lap time is made yet the best lap timer goes up equal to the current lap timer
            {
                ps1.bestLapTime = ps1.currentLapTime;
            }
        }



        //ps2
        if (ps2.started && !ps2.finished)
        {
            ps2.currentLapTime += Time.deltaTime; //sums up current lap time
            if (ps2.bestLap == 0) //if lap time is 0 no best lap was made yet, so the first one is the best one
            {
                ps2.bestLap = 1;
            }
        }

        if (ps2.started)
        {
            if (ps2.bestLap == ps2.currentLap) //if the game is started and no best lap time is made yet the best lap timer goes up equal to the current lap timer
            {
                ps2.bestLapTime = ps2.currentLapTime;
            }
        }


    }



    public void StartRace()
    {
        //TODO reset game


        //TODO reset player stats
        ps1.finished = false;
        ps2.finished = false;
        ps1.currentLap = 1;
        ps2.currentLap = 1;
        ps1.currentCheckpoint = 0;
        ps2.currentCheckpoint = 0;
        ps1.currentLapTime = 0;
        ps2.currentLapTime = 0;
        //best lap?
        //best lap time?


        //start race
        ps1.started = true;
        ps2.started = true;
}



    public void OnCheckpointTriggered(GameObject checkpoint, GameObject bike)
    {

        EnterRoutine(checkpoint, bike);
    }




    private void EnterRoutine(GameObject thisCheckpoint, GameObject bike)
    {
        PlayerStats playerstats = null;

        if (bike.transform.parent.name.Equals("Player 1(Clone)")) //TODO naming
        {
            playerstats = ps1;
        }
        else
        {
            playerstats = ps2;
        }



        //end the lap or race
        if (thisCheckpoint == end && playerstats.started)
            {
                //if all laps are finished, end the race
                if (playerstats.currentLap == laps)
                {
                    //check if the currentCheckpoint matches the checkpoints length
                    if (playerstats.currentCheckpoint == checkpoints.Length)
                    {
                        //update best lap if a new best was made
                        if (playerstats.currentLapTime < playerstats.bestLapTime)
                        {
                        playerstats.bestLap = playerstats.currentLap;
                        }

                        //check if the race is already finished
                        if (playerstats.finished)
                        {
                            print("Already finished"); //TODO shouldnt happen because of the menu
                        }
                        else
                        {
                        playerstats.finished = true;
                            print("Finished"); //TODO finish game
                        } 
                    }
                    //player passed less checkpoints than exist
                    else
                    {
                        print("Did not go through all checkpoints"); //TODO 
                    }
                }



                //if all laps are not finished, start a new lap
                else if (playerstats.currentLap < laps)
                {
                    //check again if the currentCheckpoint matches the checkpoints length, if so then start a new lap
                    if (playerstats.currentCheckpoint == checkpoints.Length)
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
                        print($"Started lap {playerstats.currentLap} - {Mathf.FloorToInt(playerstats.currentLapTime / 60)}:{playerstats.currentLapTime % 60:00.000}"); //TODO
                    }
                    //player passed less checkpoints than exist
                    else
                    {
                        print("Did not go through all checkpoints"); //TODO
                    }
                }

            }



            //check which checkpoint the played passed and if its the right or false one
            for (int i = 0; i < checkpoints.Length; i++)
            {
                if (playerstats.finished || !playerstats.started)
                {
                    return; //the game is finished or has not started yet
                }

                //going through the right checkpoint -> increase the current checkpoint by 1
                if (thisCheckpoint == checkpoints[i] && i == playerstats.currentCheckpoint)
                {
                    print($"Correct checkpoint - {Mathf.FloorToInt(playerstats.currentLapTime / 60)}:{playerstats.currentLapTime % 60:00.000}"); //TODO
                playerstats.currentCheckpoint++;
                }
                //not going through the right checkpoint -> in our case that means going backwards so reduce the current checkpoint by 1
                else if (thisCheckpoint == checkpoints[i] && i != playerstats.currentCheckpoint)
                {
                    print("Incorrect checkpoint - wrong direction, turn around"); //In our case, because you shouldn't be able to leave the track and miss one it has to mean that the player turned and is driving the wrong direction //TODO
                playerstats.currentCheckpoint--;
                }
            }


            

        }





    //TODO replace with UI
    private void OnGUI()
    {
        //shows the current time on gui
        string formattedCurrentTime = $"Current: {Mathf.FloorToInt(ps1.currentLapTime / 60)}:{ps1.currentLapTime % 60:00.000} - (Lap {ps1.currentLap})";
        GUI.Label(new Rect(50, 10, 250, 100), formattedCurrentTime);

        //shows the best time on gui
        string formattedBestTime = $"Best: {Mathf.FloorToInt(ps1.bestLapTime / 60)}:{ps1.bestLapTime % 60:00.000} - (Lap {ps1.bestLap})";
        GUI.Label(new Rect(250, 10, 250, 100), formattedBestTime);





        //shows the current time on gui
        string formattedCurrentTime2 = $"Current: {Mathf.FloorToInt(ps2.currentLapTime / 60)}:{ps2.currentLapTime % 60:00.000} - (Lap {ps2.currentLap})";
        GUI.Label(new Rect(600, 10, 250, 100), formattedCurrentTime2);

        //shows the best time on gui
        string formattedBestTime2 = $"Best: {Mathf.FloorToInt(ps2.bestLapTime / 60)}:{ps2.bestLapTime % 60:00.000} - (Lap {ps2.bestLap})";
        GUI.Label(new Rect(800, 10, 250, 100), formattedBestTime2);
    }

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