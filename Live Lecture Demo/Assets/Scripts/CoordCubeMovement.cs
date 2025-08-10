using UnityEngine;
using UnityEngine.InputSystem;

public class CoordCubeMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5.0f; // meters per sec
    [SerializeField] private Vector3 defaultMoveDirection = new Vector3(1.0f, 0.0f, 0.0f); // right
    private Vector3 moveDirection;

    void Awake()
    {
        moveDirection = defaultMoveDirection;
    }

    // Update is called once per frame
    void Update()
    {
        //every frame move player by the speed and direction values.
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.Self);
    }

    void OnTriggerEnter(Collider collider)
    {
        // change move direction to a different vector.
        moveDirection = Vector3.forward;
        Debug.Log("trigger event detected from collision with: " + collider.gameObject.name + ". New direction = " + moveDirection);
    }
}
