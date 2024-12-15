using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Bumper : MonoBehaviour
{
    public bool contact;

    void Start()
    {
        contact = false;
    }
    /// <summary>
    /// When the bumper hits an object set collision to true
    /// </summary>
    /// <param name="other"> Object the collider is toutching </param>
    void OnTriggerEnter(Collider other)
    {
        if (!other.isTrigger)
        {
            contact = true;
        }
    }
    /// <summary>
    /// When the bumper stops toutching something set contact to false
    /// </summary>
    /// <param name="other"> Object the collider is toutching </param>
    void OnTriggerExit(Collider other)
    {
        contact = false;
    }
}