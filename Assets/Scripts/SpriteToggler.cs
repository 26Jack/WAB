using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteToggler : MonoBehaviour
{
    private SpriteRenderer sr;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            if (sr != null)
            {
                sr.enabled = !sr.enabled;
            }
        }
    }
}
