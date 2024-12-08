using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollider : MonoBehaviour
{
    [Header("Colliders")]

    [SerializeField]
    private Bumper leftCollisionDetector;
    [SerializeField]
    private Bumper rightCollisionDetector;

    [Header("Speed Thresholds")]

    [SerializeField]
    private float fastSpeed;
    [SerializeField]
    private float moderateSpeed;

    PlayerPhysics pp;
    PlayerStumbler s;

    private void Awake()
    {
        pp = GetComponent<PlayerPhysics>();
        s = GetComponent<PlayerStumbler>();
    }

    // Moved from tiltBike() into its own method
    public void checkForContact()
    {
        if (leftCollisionDetector.contact && rightCollisionDetector.contact)
        {
            if (pp.speed >= fastSpeed)
                StartCoroutine(s.Stumble(Penalty.Large, 0));
            else if (pp.speed >= moderateSpeed)
                StartCoroutine(s.Stumble(Penalty.Medium, 0));
            else
                StartCoroutine(s.Stumble(Penalty.Small, 0));
        }
        else if (leftCollisionDetector.contact || rightCollisionDetector.contact)
        {
            if (pp.speed >= fastSpeed)
                StartCoroutine(s.Stumble(Penalty.Medium, 0.05f));
            else if (pp.speed >= moderateSpeed)
                StartCoroutine(s.Stumble(Penalty.Small, 0.05f));
        }
    }
}
