using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoints : MonoBehaviour
{
    [Header("Checkpoints")]
    public GameObject start; //defines the start checkpoint
    public GameObject end; //defines the end checkpoint
    public GameObject[] checkpoints; //all other checkpoints

    [Header("Settings")]
    public float laps = 1; //amount of laps

    [Header("Information")]
    private float currentCheckpoint; //index of current checkpoint -> starts at 0
    private float currentLap; //couts the laps -> starts at 1
    private bool started; //false or true for the game to be started
    private bool finished; //false or true for the game to be finished

    private float currentLapTime; //time of the current lap
    private float bestLapTime; //time of the best lap
    private float bestLap; //number of the lap with the best time

    /**
     * Start method, called on start, sets all the variables to their default value in the beginning.
     */
    private void Start()
    {
        currentCheckpoint = 0;
        currentLap = 1;

        started = false;
        finished = false;

        currentLapTime = 0;
        bestLapTime = 0;
        bestLap = 0;
    }

    /**
     * Update method, called at regular intervalls. Tracks time for the laps.
     */
    private void Update()
    {
        if (started && !finished)
        {
            currentLapTime += Time.deltaTime; //sums up current lap time
            if (bestLap == 0) //if lap time is 0 no best lap was made yet, so the first one is the best one
            {
                bestLap = 1;
            }
        }

        if (started) 
        {
            if (bestLap == currentLap) //if the game is started and no best lap time is made yet the best lap timer goes up equal to the current lap timer
            {
                bestLapTime = currentLapTime;
            }
        }
    }


    /**
     * This method is triggered everytime a player passes a checkpoint and starts the checkpoint routine.
     */
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Checkpoint")) //starts the routine if player collides with a checkpoint
        {
            GameObject thisCheckpoint = other.gameObject; //get current checkpoint



            //start the race
            if (thisCheckpoint == start && !started)
            {
                print("Started");
                started = true;
            }
            //end the lap or race
            else if (thisCheckpoint == end && started)
            {
                //if all laps are finished, end the race
                if (currentLap == laps)
                {
                    //check if the currentCheckpoint matches the checkpoints length
                    if (currentCheckpoint == checkpoints.Length)
                    {
                        //update best lap if a new best was made
                        if (currentLapTime < bestLapTime)
                        {
                            bestLap = currentLap;
                        }

                        //check if the race is already finished
                        if (finished)
                        {
                            print("Already finished");
                        }
                        else
                        {
                            finished = true;
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
                else if (currentLap < laps)
                {
                    //check again if the currentCheckpoint matches the checkpoints length, if so then start a new lap
                    if (currentCheckpoint == checkpoints.Length)
                    {
                        //update best lap and best lap time if a new best was made
                        if (currentLapTime < bestLapTime)
                        {
                            bestLap = currentLap;
                            bestLapTime = currentLapTime;
                        }

                        currentLap++; //increase lap
                        currentCheckpoint = 0; //reset checkpoints 
                        currentLapTime = 0; //reset time
                        print($"Started lap {currentLap} - {Mathf.FloorToInt(currentLapTime / 60)}:{currentLapTime % 60:00.000}");
                    }
                    //player passed less checkpoints than exist
                    else
                    {
                        print("Did not go through all checkpoints");
                    }
                }
                
            }



            //check which checkpoint the played passed and if its the right or false one
            for (int i = 0; i < checkpoints.Length; i++)
            {
                if (finished || !started)
                {
                    return; //the game is finished or has not started yet
                }

                //going through the right checkpoint -> increase the current checkpoint by 1
                if (thisCheckpoint == checkpoints[i] && i == currentCheckpoint)
                {
                    print($"Correct checkpoint - {Mathf.FloorToInt(currentLapTime / 60)}:{currentLapTime % 60:00.000}");
                    currentCheckpoint++;
                }
                //not going through the right checkpoint -> in our case that means going backwards so reduce the current checkpoint by 1
                else if (thisCheckpoint == checkpoints[i] && i != currentCheckpoint)
                {
                    print("Incorrect checkpoint - wrong direction, turn around"); //In our case, because you shouldn't be able to leave the track and miss one it has to mean that the player turned and is driving the wrong direction
                    currentCheckpoint--;
                }
            }

        }
    }

    //TODO remove
    /**
     * Shows times on GUI, just for testing.
     */
    private void OnGUI()
    {
        //shows the current time on gui
        string formattedCurrentTime = $"Current: {Mathf.FloorToInt(currentLapTime / 60)}:{currentLapTime % 60:00.000} - (Lap {currentLap})";
        GUI.Label(new Rect(50, 10, 250, 100), formattedCurrentTime);

        //shows the best time on gui
        string formattedBestTime = $"Best: {Mathf.FloorToInt(bestLapTime / 60)}:{bestLapTime % 60:00.000} - (Lap {bestLap})";
        GUI.Label(new Rect(250, 10, 250, 100), formattedBestTime);
    }
}
