using UnityEngine;

public interface ICheckpointObserver
{
    void OnCheckpointTriggered(CT checkpoint, GameObject bike);
}