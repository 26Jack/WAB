using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ExplosionVisual : MonoBehaviour
{
    [Header("Colors")]
    public Color startColor = Color.white;
    public Color endColor = new Color(1f, 0.5f, 0f); // orange

    [Header("Pulse")]
    public float pulseInterval = 0.05f;
    public float pulseScaleMultiplier = 1.2f;

    [Header("Flicker")]
    public float flickerInterval = 0.04f;

    [Header("Lifetime")]
    public float lifetime = 0.5f;

    private SpriteRenderer sr;
    private Vector3 baseScale;
    private Vector3 altScale;
    private float timeElapsed;
    private float pulseTimer;
    private float flickerTimer;
    private bool isPulsed;
    private bool isVisible = true;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        baseScale = transform.localScale;
        altScale = baseScale * pulseScaleMultiplier;
    }

    void Update()
    {
        timeElapsed += Time.deltaTime;
        pulseTimer += Time.deltaTime;
        flickerTimer += Time.deltaTime;

        float t = timeElapsed / lifetime;

        // Color blend (white to orange)
        Color targetColor = Color.Lerp(startColor, endColor, t);

        // Alpha flicker
        if (flickerTimer >= flickerInterval)
        {
            flickerTimer = 0f;
            isVisible = !isVisible;
        }

        targetColor.a = isVisible ? 1f : 0f;
        sr.color = targetColor;

        // Instant pulse toggle
        if (pulseTimer >= pulseInterval)
        {
            pulseTimer = 0f;
            isPulsed = !isPulsed;
            transform.localScale = isPulsed ? altScale : baseScale;
        }

        // Lifetime check
        if (timeElapsed >= lifetime)
        {
            Destroy(gameObject);
        }
    }
}