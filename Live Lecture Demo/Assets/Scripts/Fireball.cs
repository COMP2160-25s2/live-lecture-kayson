using UnityEngine;

public class Fireball : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 20.0f;


    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime, Space.Self);
        Debug.Log("fireball: moving");
    }

    // destroy if fireball hits object
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Fireball hit object: " + other.gameObject.name);
        Destroy(gameObject);
    }


}
