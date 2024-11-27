using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointTrigger : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Bike"))
        {
            CheckpointManager.instance.OnCheckpointTriggered(gameObject, other.gameObject);
        }
    }
}