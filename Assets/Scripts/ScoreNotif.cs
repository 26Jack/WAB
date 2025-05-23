using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreNotif : MonoBehaviour
{
    public List<Color> colors;
    public float colorSwitchInterval = 0.2f;
    public float shrinkRate = 0.1f;
    public float minSize = 0.1f;
    public float shrinkSpeedUp = 0.05f;

    private SpriteRenderer spriteRenderer;
    private int colorIndex = 0;


    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (colors.Count > 0)
        {
            spriteRenderer.color = colors[0];
            StartCoroutine(ChangeColorRoutine());
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale -= Vector3.one * shrinkRate * Time.deltaTime;
        shrinkRate += shrinkSpeedUp;

        if (transform.localScale.x <= minSize || transform.localScale.y <= minSize)
        {
            Destroy(gameObject);
        }
    }

    IEnumerator ChangeColorRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(colorSwitchInterval);

            // next color
            colorIndex = (colorIndex + 1) % colors.Count;
            spriteRenderer.color = colors[colorIndex];
        }
    }
}
