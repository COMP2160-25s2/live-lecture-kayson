using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5.0f; // meters per sec

    private Vector3 moveDirection = new Vector3(0.0f, 0.0f, 0.0f);
    private PlayerActions actions;
    private InputAction movementAction;


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

    // Update is called once per frame
    void Update()
    {
        moveDirection.x = movementAction.ReadValue<Vector2>().x;
        moveDirection.z = movementAction.ReadValue<Vector2>().y;

        //every frame move player by the speed and direction values.
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.Self);
    }
}
