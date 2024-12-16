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

    /// <summary>
    /// Uses 2 colliders on the front of the boke to determin how it contacted other objects
    /// </summary>
    public void checkForContact()
    {
        // If both colliders make contact, a front on collision, apply a harsh penalty dependant on speed
        if (leftCollisionDetector.contact && rightCollisionDetector.contact)
        {
            if (pp.speedBracket == Speed.Fast)
            {
                StartCoroutine(s.Stumble(Penalty.Large, 0, true));
            }
            else if (pp.speedBracket == Speed.Medium)
            {
                StartCoroutine(s.Stumble(Penalty.Medium, 0, true));
            }
            else
            {
                StartCoroutine(s.Stumble(Penalty.Small, 0, true));
            }
        }
        // If one collider makes contact, a side swipe, apply a light penalty dependant on speed
        else if (leftCollisionDetector.contact || rightCollisionDetector.contact)
        {
            if (pp.speedBracket == Speed.Fast)
            {
                StartCoroutine(s.Stumble(Penalty.Medium, 0.05f, true));
            }
            else if (pp.speedBracket == Speed.Medium)
            {
                StartCoroutine(s.Stumble(Penalty.Small, 0.05f, true));
            }
        }
    }
}
