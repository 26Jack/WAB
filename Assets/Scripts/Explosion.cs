using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public float explosionRadius = 5f;
    public float explosionForce = 10f;

    public float damageAmount = 20f;

    // Start is called before the first frame update
    void Start()
    {
        Explode();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Explode()
    {
        // detect near colliders
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);

        foreach (Collider2D hit in colliders)
        {
            Rigidbody2D rb = hit.GetComponent<Rigidbody2D>();

            var explosionDamage = hit.GetComponent<IExplosionDamage>();
            if (explosionDamage != null)
            {
                explosionDamage.ExplosionDamage(damageAmount);
            }

            if (rb != null && rb != GetComponent<Rigidbody2D>())
            {
                // calc direction from center
                Vector2 direction = (rb.position - (Vector2)transform.position).normalized;

                rb.AddForce(direction * explosionForce, ForceMode2D.Impulse);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
