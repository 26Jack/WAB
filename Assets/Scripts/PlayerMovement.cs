using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using TMPro;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Assignments")]
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    public SpriteFlash barLeft;
    public SpriteFlash barRight;
    public TextMeshProUGUI wallHitText;

    [Header("Keybinds")]
    public KeyCode up = KeyCode.UpArrow;
    public KeyCode right = KeyCode.RightArrow;
    public KeyCode left = KeyCode.LeftArrow;
    public KeyCode down = KeyCode.DownArrow;
    public KeyCode boost = KeyCode.Space;

    [Header("Movement")]
    public float moveSpeed = 25f;

    [Header("Wall Attack")]
    public bool wallAttack = false;
    public float wallAttackTimer = 0f;
    public float wallAttackDuration = 1f;

    [Header("Boost")]
    public bool boosting = false;
    public float chargeAmount = 100;
    public float boostMult = 3;
    public float boostTimerInt = 0f;

    [Header("Speed")]
    public float currentSpeed = 100f;
    public float speedRate = 10f;

    [Header("Invincibility")]
    public bool invincible = false;
    public float invincibilityTimer = 0;
    public float invincibilityDuration = 1f;
    public float invincibilityFlashTimer = 0;
    public float invincibilityFlashThresh = 0.2f;

    public int bumps = 0;

    public event Action OnHitWall;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (wallAttack)
        {
            wallAttackTimer += Time.deltaTime;
            if (wallAttackTimer >= wallAttackDuration)
            {
                wallAttack = false;
                wallAttackTimer = 0f;
            }
        }

        if (boosting)
        {
            FindObjectOfType<CameraShake>().Shake(0.01f, 0.75f);

            currentSpeed += speedRate * boostMult * Time.deltaTime;
        }
        else
        {
            currentSpeed += speedRate * Time.deltaTime;
        }

        if (!Input.GetKey(boost))
        {
            chargeAmount += Time.deltaTime;
            boosting = false;
        }

        if (Input.GetKey(boost))
        {
            if (chargeAmount > 1)
            {
                boosting = true;
                chargeAmount -= Time.deltaTime;
            }
            else
            {
                boosting = false;
            }
        }

        if (wallAttack)
        {
            sr.color = Color.cyan;
        }
        else
        {
            sr.color = Color.white;
        }

        if (invincible)
        {
            invincibilityTimer += Time.deltaTime;
            invincibilityFlashTimer += Time.deltaTime;

            if (invincibilityFlashTimer >= invincibilityFlashThresh)
            {
                invincibilityFlashTimer = 0;
                
                if (sr.enabled)
                {
                    sr.enabled = false;
                }
                else
                {
                    sr.enabled = true;
                }
            }

            if (invincibilityTimer >= invincibilityDuration)
            {
                invincible = false;
                invincibilityTimer = 0f;
                invincibilityFlashTimer = 0;
                sr.enabled = true;
            }
        }
    }

    void FixedUpdate()
    {
        Vector2 movementDirection = Vector2.zero;

        if (Input.GetKey(up))
        {
            movementDirection += Vector2.up;
        }
        if (Input.GetKey(right))
        {
            movementDirection += Vector2.right;
        }
        if (Input.GetKey(left))
        {
            movementDirection += Vector2.left;
        }
        if (Input.GetKey(down))
        {
            movementDirection += Vector2.down;
        }

        if (movementDirection.magnitude > 1f)
        {
            movementDirection.Normalize();
        }

        rb.AddForce(movementDirection * moveSpeed);
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            // contactpoint to find direction of bounce (hitting left wall = 1, hitting right wall = -1)
            ContactPoint2D contact = collision.GetContact(0);
            Vector2 normal = contact.normal;
            //Debug.Log("Bounce direction: " + normal);

            HitWall();

            if (normal == Vector2.left){
                barRight.Flash();
            }
            if (normal == Vector2.right)
            {
                barLeft.Flash();
            }
        }

        bumps++;
        UpdateWallHitText();

    }

    public void HitWall()
    {
        wallAttack = true;
        wallAttackTimer = 0f;
        OnHitWall?.Invoke();
        FindObjectOfType<CameraShake>().Shake(0.15f, 3);

    }

    public void TookDamage()
    {
        if (!invincible)
        {
            //Debug.Log("ate it");
            FindObjectOfType<CameraShake>().Shake(0.15f, 4);
            invincible = true;
            invincibilityTimer = 0f;
        }
    }
    void UpdateWallHitText()
    {
        wallHitText.text = "Bumps: " + bumps.ToString();
    }
}

