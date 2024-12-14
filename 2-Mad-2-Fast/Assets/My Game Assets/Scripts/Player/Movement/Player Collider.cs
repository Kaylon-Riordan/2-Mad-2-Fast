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
            if (pp.speedBracket == Speed.Fast)
                StartCoroutine(s.Stumble(Penalty.Large, 0, true));
            else if (pp.speedBracket == Speed.Medium)
                StartCoroutine(s.Stumble(Penalty.Medium, 0, true));
            else
                StartCoroutine(s.Stumble(Penalty.Small, 0, true));
        }
        else if (leftCollisionDetector.contact || rightCollisionDetector.contact)
        {
            if (pp.speedBracket == Speed.Fast)
                StartCoroutine(s.Stumble(Penalty.Medium, 0.05f, true));
            else if (pp.speedBracket == Speed.Medium)
                StartCoroutine(s.Stumble(Penalty.Small, 0.05f, true));
        }
    }
}
