using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CT : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Bike"))
        {
            CM.instance.OnCheckpointTriggered(gameObject, other.gameObject);
        }
    }
}
