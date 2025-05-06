using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundFlash : MonoBehaviour
{
    public Color defaultColor = Color.black;
    public Color flashColor = Color.yellow;
    public float flashDuration = 0.5f;

    private float flashTimer = 0f;
    private bool isFlashing = false;
    private Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
        cam.backgroundColor = defaultColor;
    }

    // Update is called once per frame
    void Update()
    {
        if (isFlashing)
        {
            flashTimer += Time.deltaTime;
            float t = flashTimer / flashDuration;
            cam.backgroundColor = Color.Lerp(flashColor, defaultColor, t);

            if (t >= 1f)
            {
                isFlashing = false;
                cam.backgroundColor = defaultColor;
            }
        }
    }

    public void Flash()
    {
        cam.backgroundColor = flashColor;
        flashTimer = 0f;
        isFlashing = true;
    }
}
