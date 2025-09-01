using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 20.0f;


    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime, Space.Self);
        // Debug.Log("Projectile: moving");
    }

    // destroy if Projectile hits object
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == GameManager.PlayerLayer)
        {
            Debug.Log("Projectile hit object: " + other.gameObject.name);
            Destroy(gameObject);
        }
    }


}
