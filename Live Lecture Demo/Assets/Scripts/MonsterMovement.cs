using UnityEngine;

public class MonsterMovement : MonoBehaviour
{
    [SerializeField] private float rotateRate = 1.0f; // m/s
    [SerializeField] private Transform target;


    // Update is called once per frame
    void Update()
    {
        Vector3 myRight = transform.TransformDirection(Vector3.right);

        Debug.Log("my right: " + myRight);

        // TODO: Get the "toTarget" vector; the vector between my position and target position
        // TODO: Normalise the toTarget vector
        // TODO: get the Dot product of the normalised toTarget vector and myRight vector
        //       - if my right is at 90 degrees to the target, then I am facing the target straight on.

        // TODO: Update transform.Rotate to be modified by the Dot Product result.

        transform.Rotate(Vector3.up, rotateRate * Time.deltaTime, Space.Self);
    }
}
