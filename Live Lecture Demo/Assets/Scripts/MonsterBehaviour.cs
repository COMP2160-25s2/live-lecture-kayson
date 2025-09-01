using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.iOS;

public class MonsterBehaviour : MonoBehaviour
{
    [SerializeField] private Transform target;

    [SerializeField] private LayerMask layers;

    [SerializeField] private float attackInterval = 5.0f; // /seconds

    [SerializeField] private float timeUntilSleep = 10.0f; // how long until i sleep in seconds
    [SerializeField] private float timeUntilSearching = 2.0f; // how long until i search for in seconds


    private PlayerMove player;
    private MonsterAttack monsterAttack;

    [SerializeField] private float viewDistance = 5.0f; // meters

    private float rotationModifier = 0.0f;

    private bool canSeePlayer = false;

    private float stateTimer;


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

    [SerializeField] private float rotateRate = 1.0f; // m/s

    #endregion  Move them

    #region Init & Destruction

    void Awake()
    {
        monsterState = defaultState;
        stateTimer = timeUntilSleep;
        
        monsterAttack = GetComponent<MonsterAttack>();
        if (!monsterAttack)
        {
            Debug.LogError("Missing Monster Attack Component");
        }

    }

    void Start()
    {
        if (stateTimer == timeUntilSleep)
        {
            Debug.Assert(stateTimer == timeUntilSleep, "The timer has progressed in timer since Awake()");
        }


        player = target.gameObject.GetComponent<PlayerMove>();
        player.VisionEvent += OnVisionEvent; // Cheating the collision event - don't do this - it's just how the live coding trigger issue was quickly solved

    }

    #endregion Init & Destryction

    // Update is called once per frame
    void Update()
    {
        DoRotation();
        DoBehaviour();
    }

    private void DoBehaviour()
    {
        switch (monsterState)
        {
            case State.Searching:
                stateTimer -= Time.deltaTime;

                DoSearching();
                break;

            case State.Sleeping:
                stateTimer -= Time.deltaTime;
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
        if (stateTimer <= 0)
        {
            SetSearchingState();
        }
    }

    private void DoRotation()
    {
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

        if (stateTimer <= 0)
        {
            SetSleepingState();
        }
    }

    private void SetAttackingState()
    {
        monsterState = State.Attacking;
        stateTimer = attackInterval;
    }

    private void SetSearchingState()
    {
        monsterState = State.Searching;
        stateTimer = timeUntilSleep;
    }

    private void SetSleepingState()
    {
        monsterState = State.Sleeping;
        stateTimer = timeUntilSearching;
    }

    private void DoAttacking()
    {
        Vector3 myRight = transform.TransformDirection(Vector3.right);
        Vector3 toTarget = Vector3.Normalize(target.position - transform.position);
        monsterAttack.DoAttack(toTarget);
        rotationModifier = Vector3.Dot(myRight, toTarget);
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

    #region HOARDING

    private void DoDotRotation()
    {
        // transform the right vector from local to world space
        Vector3 myRight = transform.TransformDirection(Vector3.right);
        //Debug.Log("my right: " + myRight);

        // Get the "toTarget" vector; the vector between my position and target position
        Vector3 toTarget = target.position - transform.position;
        //Debug.Log("toTarget.magnitude = " + toTarget.magnitude);

        // Normalise the toTarget vector
        //      - we don't care how far away we are, we just care about the direction, so get the direction with the vector normalised (i.e., a vector with mag of 1)
        Vector3 toTargetNorm = Vector3.Normalize(toTarget);
        //Debug.Log("toTargetNorm.magnitude = " + toTargetNorm.magnitude);


        // Get the Dot product of the normalised toTarget vector and myRight vector
        //       - if my right is at 90 degrees to the target, then I am facing the target straight on.
        //  For normalized vectors in the same cood space, Dot returns a value between -1 and 1.
        //      1 if they point in exactly the same direction,
        //      -1 if they point in completely opposite directions, and
        //      zero if the vectors are perpendicular.
        float targetRelative = Vector3.Dot(myRight, toTargetNorm);
        //Debug.Log("target relative: " + targetRelative);


        // Update transform.Rotate to be modified by the Dot Product result.
        // float rotationDirection = 0.0f;

        // if (targetRelative < 0)
        // {
        //     Debug.Log("target to left of me.");

        //     rotationDirection = -1.0f;
        // }
        // else if (targetRelative > 0)
        // {
        //     Debug.Log("target to right of me.");

        //     rotationDirection = 1.0f;
        // }

        //transform.Rotate(Vector3.up, rotationDirection * rotateRate * Time.deltaTime, Space.Self);
        //Debug.Log("rotationDirection = " + rotationDirection + "\n targetRelative = " + targetRelative);
        transform.Rotate(Vector3.up, targetRelative * rotateRate * Time.deltaTime, Space.Self);
    }

    #endregion

}
