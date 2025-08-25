using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5.0f; // meters per sec

    private Vector3 moveDirection = new Vector3(0.0f, 0.0f, 0.0f);
    private PlayerActions actions;
    private InputAction movementAction;

    public delegate void VisionEventHandler(bool isVisible);
    public event VisionEventHandler VisionEvent;


    [SerializeField] private MonsterBehaviour monster;

    void Awake()
    {
        actions = new PlayerActions();
        movementAction = actions.gameWorld.movement;
    }

    void OnEnable()
    {
        movementAction.Enable();
    }

    void OnDisable()
    {
        movementAction.Disable();
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        moveDirection.x = movementAction.ReadValue<Vector2>().x;
        moveDirection.z = movementAction.ReadValue<Vector2>().y;

        //every frame move player by the speed and direction values.
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.Self);
    }

    void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Player trigger detected" + other.gameObject.name);
        // invoke player in in vision event
        VisionEvent?.Invoke(true);
    }

    void OnTriggerExit(Collider other)
    {
        VisionEvent?.Invoke(false);
    }
}


#region Hoarding
//private string password = "def not password";
// if (monster.Password == password)
// {
//     Debug.Log("yay");
// }
// else
// {
//     Debug.Log("very sad. Let's do crime");
//     monster.Password = password;
// }
#endregion