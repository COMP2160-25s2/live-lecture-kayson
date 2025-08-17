using UnityEngine;
using UnityEngine.iOS;

public class MonsterBehaviour : MonoBehaviour
{
    [SerializeField] private float attackInterval = 5.0f; // /s
    [SerializeField] private Fireball fireball;

    [SerializeField] private float timeUntilSleep = 10.0f; // how long until i sleep in seconds
    [SerializeField] private float timeUntilSearching = 2.0f; // how long i sleep for in seconds

    [SerializeField] private float rotateRate = 1.0f; // m/s
    [SerializeField] private Transform target;

    private float rotationModifier = 0.0f;

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
            Debug.LogError("No fireball assigned!!!");
        }

        timer = timeUntilSleep;
        monsterState = State.Searching;
    }


    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up, rotationModifier * rotateRate * Time.deltaTime, Space.Self);

        switch (monsterState)
        {
            case State.Searching:

                // rotate in consistent direction until player is within field of vision
                rotationModifier = 1.0f;            

                //TODO if(see player)
                {
                    //monsterState = State.Attacking;
                    //timer = attackInterval;
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
                    monsterState = State.Searching;
                    timer = timeUntilSleep;
                }

                break;

            case State.Attacking:
                if (timer <= 0)
                {
                    FocusOnPlayer();
                    Shoot();
                }

                break;
        }

        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            Debug.Log("Fireball timer is depleted");
            Shoot();
        }
    }

    // ever 5 seconds, shoot a fireball
    void Shoot()
    {
        Debug.Log("Fireball is shooting");
        //Instantiate(fireball, this.transform);
        Instantiate(fireball, this.transform.position, this.transform.rotation);
        timer = attackInterval;
    }


    void FocusOnPlayer()
    {
        Vector3 myRight = transform.TransformDirection(Vector3.right);
        Vector3 toTarget = Vector3.Normalize(target.position - transform.position);
        rotationModifier = Vector3.Dot(myRight, toTarget);

    }
}
