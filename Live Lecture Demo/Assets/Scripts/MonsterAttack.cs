using UnityEngine;

public class MonsterAttack : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private LayerMask rayLayers;
    [SerializeField] private Projectile projectile;
    [SerializeField] private float attackInterval = 5f;
    private float timer = 0;

    internal void DoAttack(Vector3 toTarget)
    {
        if (timer <= 0)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, toTarget, out hit, Mathf.Infinity, rayLayers))
            {
                if (hit.collider.gameObject.layer == GameManager.PlayerLayer)
                {
                    Shoot();
                }
            }
        }

        timer -= Time.deltaTime;
    }

    void Shoot()
    {
        Projectile newProjectile = Instantiate(projectile, transform.position, transform.rotation);
        timer = attackInterval;
    }
}
