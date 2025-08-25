using UnityEngine;

public class MonsterMovement : MonoBehaviour
{
    [SerializeField] private float rotateRate = 1.0f; // m/s
    [SerializeField] private Transform target;


    private float rotationModifier = 1.0f; // clockwise
    public float RotationModifier
    {
        get
        {
            return rotationModifier;
        }
        set
        {
            rotationModifier = value;
        }

    }


    void Start()
    {
        if (!target)
        {
            Debug.LogError("Target has not been assigned!!!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        DoRotation();
        DoDotRotation();
    }

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

    private void DoRotation()
    {
        //TODO: Move to movement
        transform.Rotate(Vector3.up, rotationModifier * rotateRate * Time.deltaTime, Space.Self);
    }
    



}
