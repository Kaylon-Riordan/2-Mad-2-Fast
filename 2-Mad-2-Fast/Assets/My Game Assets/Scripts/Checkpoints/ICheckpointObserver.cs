using UnityEngine;

/// <summary>
/// Interface for the managers, defines the triggers for the observer.
/// </summary>
public interface ICheckpointObserver
{
    #region trigger functions
    /// <summary>
    /// This is called when a checkpoint is triggered and contains information about the trigger (checkpoint) and the bike that triggered it.
    /// </summary>
    /// <param name="checkpoint"> The checkpoint as subject of the observer </param>
    /// <param name="bike"> The bike that triggered the checkpoint (player 1 or player 2) </param>
    void OnCheckpointTriggered(CheckpointTrigger checkpoint, GameObject bike);
    #endregion
}