using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteFlash : MonoBehaviour
{
    public SpriteRenderer sr;
    public Color defaultColor = Color.black;
    public Color flashColor = Color.blue;
    public float flashDuration = 0.5f;

    private float flashTimer = 0f;
    private bool isFlashing = false;

    // Start is called before the first frame update
    void Start()
    {
        if (sr == null)
            sr = GetComponent<SpriteRenderer>();

        sr.color = defaultColor;
    }

    // Update is called once per frame
    void Update()
    {
        if (isFlashing)
        {
            flashTimer += Time.deltaTime;
            float t = flashTimer / flashDuration;
            sr.color = Color.Lerp(flashColor, defaultColor, t);

            if (t >= 1f)
            {
                isFlashing = false;
                sr.color = defaultColor;
            }
        }
    }

    public void Flash()
    {
        sr.color = flashColor;
        flashTimer = 0f;
        isFlashing = true;
    }
}
