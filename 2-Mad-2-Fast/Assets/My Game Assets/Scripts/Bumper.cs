using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Bumper : MonoBehaviour
{
    public bool contact;
    // Start is called before the first frame update
    void Start()
    {
        contact = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.isTrigger)
        {
            contact = true;
        }
    }
    void OnTriggerExit(Collider other)
    {
        contact = false;
    }
}