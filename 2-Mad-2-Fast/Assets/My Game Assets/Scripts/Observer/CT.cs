using System;
using System.Collections.Generic;
using UnityEngine;

public class CT : MonoBehaviour
{
    private List<ICheckpointObserver> observers = new List<ICheckpointObserver>();



    // Subscribe an observer
    public void Subscribe(ICheckpointObserver observer)
    {
        if (!observers.Contains(observer))
            observers.Add(observer);
    }

    // Unsubscribe an observer
    public void Unsubscribe(ICheckpointObserver observer)
    {
        if (observers.Contains(observer))
            observers.Remove(observer);
    }

    // Notify all subscribed observers
    private void NotifyObservers(GameObject bike)
    {
     
        foreach (var observer in observers)
        {
            observer.OnCheckpointTriggered(this, bike); // Pass this checkpoint as context
        }
    }




    // Trigger logic
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bike"))
        {
            Debug.Log($"Checkpoint triggered: {name}");
            NotifyObservers(other.gameObject); // Notify observers when checkpoint is triggered
        }
    }
}
