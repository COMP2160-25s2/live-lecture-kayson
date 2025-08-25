using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.iOS;

public class MonsterBehaviour : MonoBehaviour
{
    
    [SerializeField] private Transform target;

    [SerializeField] private LayerMask layers;

    [SerializeField] private Fireball fireball;

    [SerializeField] private float attackInterval = 5.0f; // /seconds

    [SerializeField] private float timeUntilSleep = 10.0f; // how long until i sleep in seconds
    [SerializeField] private float timeUntilSearching = 2.0f; // how long until i search for in seconds

    

    private int targetLayer;

    [SerializeField] private float viewDistance = 5.0f; // meters

    private float rotationModifier = 0.0f;

    private bool canSeePlayer = false;

    private float timer;


    private enum State
    {
        Searching,
        Sleeping,
        Attacking
    }

    private State defaultState = State.Searching;

    private State monsterState;
    private State MonsterState
    {
        get
        {
            return monsterState;
        }
        set
        {
            monsterState = value;
            Debug.Log("Someone changed my monsterState");
        }
    }

    #region  Move them

    //TODO: Move to movement
    [SerializeField] private float rotateRate = 1.0f; // m/s

    #endregion  Move them

    #region Init & Destruction

    void Awake()
    {
        monsterState = defaultState;
        timer = timeUntilSleep;
    }

    void Start()
    {
        if (!fireball)
        {
            //Debug.LogError("No fireball assigned!!!");
        }

        if (timer == timeUntilSleep)
        {
            Debug.Assert(timer == timeUntilSleep, "The timer has progressed in timer since Awake()");
        }

        PlayerMove player = target.gameObject.GetComponent<PlayerMove>();
        player.VisionEvent += OnVisionEvent;
        targetLayer = Dictionary.PlayerLayer;
    }

    #endregion Init & Destryction

    // Update is called once per frame
    void Update()
    {
        if (timer == timeUntilSleep)
        {
            Debug.Assert(timer == timeUntilSleep, "The timer has progressed in timer since Awake()");
        }

        ///DoRotation();
        DoBehaviour();

        timer -= Time.deltaTime;
    }

    private void DoBehaviour()
    {
        switch (monsterState)
        {
            case State.Searching:
                DoSearching();
                break;

            case State.Sleeping:
                DoSleeping();
                break;

            case State.Attacking:
                if (canSeePlayer)
                {
                    DoAttacking();
                }
                else
                {
                    SetSearchingState();
                }
                break;
        }
    }

    private void DoSleeping()
    {
        if (timer <= 0)
        {
            SetSearchingState();
        }
    }

    private void DoRotation()
    {
        //TODO: Move to movement
        transform.Rotate(Vector3.up, rotationModifier * rotateRate * Time.deltaTime, Space.Self);
    }

    void DoSearching()
    {
        // rotate in consistent direction until player is within field of vision
        rotationModifier = 1.0f;

        if (canSeePlayer)
        {
            SetAttackingState();
        }

        if (timer <= 0)
        {
            SetSleepingState();
        }
    }

    private void SetAttackingState()
    {
        monsterState = State.Attacking;
        timer = attackInterval;
    }

    private void SetSearchingState()
    {
        monsterState = State.Searching;
        timer = timeUntilSleep;
    }

    private void SetSleepingState()
    {
        monsterState = State.Sleeping;
        timer = timeUntilSearching;
    }

    private void DoAttacking()
    {
        Vector3 myRight = transform.TransformDirection(Vector3.right);
        Vector3 toTarget = Vector3.Normalize(target.position - transform.position);
        rotationModifier = Vector3.Dot(myRight, toTarget);

        if (timer <= 0)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, toTarget, out hit, viewDistance, layerMask: layers))
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);

                Debug.Log("hit.collider.gameObject.layer = " + hit.collider.gameObject.layer);
                if (hit.collider.gameObject.layer == targetLayer)
                {
                    Shoot();
                }
            }
            else
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000.0f, Color.magenta);
            }

            timer = attackInterval;
        }
    }

    // ever 5 seconds, shoot a fireball
    void Shoot()
    {
        //Debug.Log("Fireball is shooting");
        Instantiate(fireball, this.transform.position, this.transform.rotation);
        timer = attackInterval;
    }

    void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Vision Event: collision detected with: " + other.gameObject.name);
        canSeePlayer = true;
    }

    void OnTriggerExit(Collider other)
    {
        //Debug.Log("Vision Event: collision exit detected.");
        canSeePlayer = false;
    }

    public void OnVisionEvent(bool isVisible)
    {
        canSeePlayer = isVisible;
    }
}

#region Hoarding


// private string defaultPassword = "123";

// private string currentPassword = "666";


// public string Password
// {
//     get
//     {
//         return currentPassword;
//     }
//     set
//     {
//         Debug.Log("Someone **tried** changed my password");
//         bool meetsCriteria = false;
//         if (meetsCriteria)
//         {
//             currentPassword = value;
//         }


//     }
// }

#endregion Hoarding

