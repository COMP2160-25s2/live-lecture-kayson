using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.iOS;

public class MonsterBehaviour : MonoBehaviour
{
    [SerializeField] private float attackInterval = 5.0f; // /s
    [SerializeField] private Fireball fireball;

    [SerializeField] private float timeUntilSleep = 10.0f; // how long until i sleep in seconds
    [SerializeField] private float timeUntilSearching = 2.0f; // how long until i search for in seconds

    [SerializeField] private float rotateRate = 1.0f; // m/s
    [SerializeField] private Transform target;

    [SerializeField] private LayerMask layers;

    private int playerLayer = 6;

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

    private State monsterState;





    void Awake()
    {
        if (!fireball)
        {
            //Debug.LogError("No fireball assigned!!!");
        }

        timer = timeUntilSleep;
        monsterState = State.Searching;
        PlayerMove player = target.gameObject.GetComponent<PlayerMove>();
        player.VisionEvent += OnVisionEvent;
        targetLayer = playerLayer;
    }


    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up, rotationModifier * rotateRate * Time.deltaTime, Space.Self);
        timer -= Time.deltaTime;
        switch (monsterState)
        {
            case State.Searching:

                // rotate in consistent direction until player is within field of vision
                rotationModifier = 1.0f;


                if (canSeePlayer)
                {
                    monsterState = State.Attacking;
                    timer = attackInterval;
                }

                if (timer <= 0)
                {
                    monsterState = State.Sleeping;
                    timer = timeUntilSearching;
                }

                break;

            case State.Sleeping:
                if (timer <= 0)
                {
                    SetSearchingState();
                }

                break;

            case State.Attacking:

                DoAttack();

                if (!canSeePlayer)
                {
                    SetSearchingState();
                }

                break;
        }


    }

    private void DoAttack()
    {
        Vector3 myRight = transform.TransformDirection(Vector3.right);
        Vector3 toTarget = Vector3.Normalize(target.position - transform.position);
        rotationModifier = Vector3.Dot(myRight, toTarget);

        if (timer <= 0)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, toTarget, out hit, viewDistance, layerMask: layers))
            {
                Debug.Log("hit.collider.gameObject.layer = " + hit.collider.gameObject.layer);
                if (hit.collider.gameObject.layer == targetLayer)
                {
                    Shoot();
                }

            }



        }
    }

    private void SetSearchingState()
    {
        monsterState = State.Searching;
        timer = timeUntilSleep;
    }


    // ever 5 seconds, shoot a fireball
    void Shoot()
    {
        //Debug.Log("Fireball is shooting");
        //Instantiate(fireball, this.transform);
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
