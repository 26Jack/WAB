using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostHitbox : MonoBehaviour
{
    private PlayerMovement player;
    private PolygonCollider2D polyCollider;
    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        player = GetComponent<PlayerMovement>();
        polyCollider = GetComponent<PolygonCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();

    }

    // Update is called once per frame
    void Update()
    {
        polyCollider.enabled = player.boosting;
        spriteRenderer.enabled = player.boosting;
    }
}
