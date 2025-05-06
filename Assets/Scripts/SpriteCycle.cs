using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteCycle : MonoBehaviour
{
    [SerializeField] private Sprite[] sprites;
    [SerializeField] private float frameRate = 0.2f;

    private SpriteRenderer spriteRenderer;
    private int currentFrame = 0;
    private float timer;

    public bool loop = false;
    private bool animationFinished = false;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (sprites.Length > 0)
        {
            spriteRenderer.sprite = sprites[0];
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (animationFinished || sprites.Length == 0) return;

        timer += Time.deltaTime;
        if (timer >= frameRate)
        {
            timer -= frameRate;

            currentFrame++;

            if (currentFrame >= sprites.Length)
            {
                if (loop)
                {
                    currentFrame = 0;
                }
                else
                {
                    currentFrame = sprites.Length - 1;
                    animationFinished = true;
                }
            }

            spriteRenderer.sprite = sprites[currentFrame];
        }
    }
}
