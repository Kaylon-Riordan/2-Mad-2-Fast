using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Checkpoint trigger, subject of the observer pattern. Subscribes to the observer, handles when the event is triggered and notifies them.
/// This script gets attached to each checkpoint.
/// </summary>
public class CheckpointTrigger : MonoBehaviour
{
    #region variables
    private List<ICheckpointObserver> observers = new List<ICheckpointObserver>();
    #endregion

    #region observer functions
    /// <summary>
    /// Subscribe an observer.
    /// </summary>
    /// <param name="observer"> Observer to be subscripted </param>
    public void Subscribe(ICheckpointObserver observer)
    {
        if (!observers.Contains(observer))
            observers.Add(observer);
    }

    /// <summary>
    /// Unsubscribe an observer.
    /// </summary>
    /// <param name="observer"> Observer to be unsubscribed </param>
    public void Unsubscribe(ICheckpointObserver observer)
    {
        if (observers.Contains(observer))
            observers.Remove(observer);
    }

    /// <summary>
    /// Notifies all subscribed observer.
    /// </summary>
    /// <param name="bike"> The bike that collided with the checkpoint. </param>
    private void NotifyObservers(GameObject bike)
    {
        foreach (var observer in observers)
        {
            observer.OnCheckpointTriggered(this, bike); // Pass this checkpoint as context
        }
    }
    #endregion

    #region trigger
    /// <summary>
    /// Triggers the logic when a GameObject from tag "Bike" collides with a checkpoint 
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bike"))
        {
            Debug.Log($"Checkpoint triggered: {name}");
            Debug.Log($"Checkpoint triggered by: {other.gameObject.name}");
            NotifyObservers(other.gameObject);
        }
    }
    #endregion
}
