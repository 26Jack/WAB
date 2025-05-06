using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class EnemyTest : MonoBehaviour, IExplosionDamage
{
    [Header("Assignments")]
    private Rigidbody2D rb;
    private PlayerMovement player;
    private SpriteRenderer sr;

    public GameObject explosionPrefab;

    [Header("Movement")]
    public int randomMoveAmount = 0;
    public int randomDirection;
    public float moveSpeed = 500;

    [Header("Health & Damage")]
    public float health = 50f;
    public float wallAttackDamage = 10;
    public float boostDamage = 1;
    public float damageToPlayer = 10f;
    public bool alive = true;

    [Header("Physics")]
    public float gravityOnDeath = 10f;
    public float gravityDefault = 0f;
    public float gravityNearFloor = -20f;
    public float gravityNearCeiling = 20f;
    public float dragOnDeath = 0f;
    public float massOnDeath = 3;

    [Header("Damage Flash")]
    public float damageFlashTimer = 0;
    public float damageFlashLength = 0.2f;
    public bool damageFlashing = false;

    [Header("Explosion")]
    public bool exploding = false;
    public float explodingTimer = 0;
    public float explodingThresh = 2;
    public float explodingFlashTimer = 0;
    public float explodingFlashThresh = 0.2f;


    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (player != null)
        {
            player.OnHitWall += ReactToWallHit;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (randomMoveAmount <= 0)
        {
            randomMoveAmount = Random.Range(10, 100);
            // 0 = left, 1 = right
            randomDirection = Random.Range(0, 2);
        }

        if (damageFlashing)
        {
            damageFlashTimer += Time.deltaTime;

            if ((damageFlashTimer > damageFlashLength) && alive)
            {
                sr.color = Color.white;
                damageFlashing = false;
                damageFlashTimer = 0;
                Debug.Log("should be white now");
            }
        }

        if (exploding)
        {
            explodingTimer += Time.deltaTime;
            explodingFlashTimer += Time.deltaTime;

            if (explodingFlashTimer >= explodingFlashThresh)
            {
                explodingFlashTimer = 0;

                if (sr.color == Color.blue)
                {
                    sr.color = Color.cyan;
                }
                else
                {
                    sr.color = Color.blue;
                }
            }

            if (explodingTimer >= explodingThresh)
            {
                Explode();
            }
        }
    }

    void FixedUpdate()
    {
        if (alive)
        {
            Vector2 movementDirection = Vector2.zero;

            if (randomDirection == 0)
            {
                movementDirection += Vector2.left;
            } else
            {
                movementDirection += Vector2.right;
            }

            if (movementDirection.magnitude > 1f)
            {
                movementDirection.Normalize();
            }

            randomMoveAmount--;

            rb.AddForce(movementDirection * moveSpeed);
        }
    }

    void ReactToWallHit()
    {
        //Debug.Log("WallHitListener player hit wall");
    }

    void OnDestroy()
    {
        // always unsubscribe to avoid memory leaks or null refs
        if (player != null)
        {
            player.OnHitWall -= ReactToWallHit;
        }
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && alive)
        {
            //Debug.Log("hit player");
            if (player.wallAttack)
            {
                health -= wallAttackDamage;
                sr.color = Color.red;
                damageFlashing = true;

                if (health <= 0)
                {
                    DieElectric();
                }
            }
            else
            {
                player.currentSpeed -= damageToPlayer;
                player.TookDamage();
            }
        }

        if (collision.gameObject.CompareTag("Wall"))
        {
            DieWall();
        }

        if (collision.gameObject.CompareTag("Kill Floor") && !alive)
        {
            Destroy(gameObject);
        }

        if (collision.gameObject.CompareTag("Enemy") && exploding)
        {
            Explode();
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Boost") && alive)
        {
            //Debug.Log("in boost");
            health -= Time.deltaTime * boostDamage;
            if (alive)
            {
                sr.color = Color.red;
            }

            if (health <= 0)
            {
                DieBoost();
            }

        }

        if (other.CompareTag("Left Detector") && alive)
        {
            //Debug.Log("near left");
            randomDirection = 1;
            randomMoveAmount = Random.Range(20, 80);
        }

        if (other.CompareTag("Right Detector") && alive)
        {
            //Debug.Log("near right");
            randomDirection = 0;
            randomMoveAmount = Random.Range(20, 80);
        }

        if (other.CompareTag("Floor Detector") && alive)
        {
            //Debug.Log("near floor");
            rb.gravityScale = gravityNearFloor;
        }

        if (other.CompareTag("Ceiling Detector") && alive)
        {
            //Debug.Log("near ceiling");
            rb.gravityScale = gravityNearCeiling;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Floor Detector") && alive)
        {
            //Debug.Log("near floor");
            rb.gravityScale = gravityDefault;
        }

        if (other.CompareTag("Ceiling Detector") && alive)
        {
            //Debug.Log("near ceiling");
            rb.gravityScale = gravityDefault;
        }

        if (other.CompareTag("Boost") && alive)
        {
            if (alive)
            {
                sr.color = Color.white;
            }
        }
    }

    public void ExplosionDamage(float damage)
    {
        Debug.Log("explosion damage: " + damage);
        health -= damage;

        sr.color = Color.red;
        damageFlashing = true;

        if (health <= 0f)
        {
            DieExplosion();
        }
    }

    public void DieElectric()
    {
        Corpse();
        exploding = true;
        sr.color = Color.blue;
    }

    public void DieBoost()
    {
        Corpse();
        sr.color = Color.gray;
    }

    public void DieExplosion()
    {
        Explode();
    }

    public void DieWall()
    {
        Explode();
    }

    public void Explode()
    {
        FindObjectOfType<CameraShake>().Shake(0.15f, 7);
        Instantiate(explosionPrefab, transform.position, transform.rotation);
        Destroy(gameObject);
        FindObjectOfType<BackgroundFlash>().Flash();
    }

    public void Corpse()
    {
        alive = false;
        rb.gravityScale = gravityOnDeath;
        rb.drag = dragOnDeath;
        rb.mass = massOnDeath;
    }
}
