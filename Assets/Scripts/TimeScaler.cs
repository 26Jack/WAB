using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TimeScaler : MonoBehaviour
{
    public float defaultTimeScale = 1f;
    public float minTimeScale = 0.1f;
    public float maxTimeScale = 1f;
    public float timeScaleStep = 0.1f;
    public float timeScale = 1f;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = defaultTimeScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Comma))
        {
            DecreaseTimeScale();
        }

        if (Input.GetKeyDown(KeyCode.Period))
        {
            IncreaseTimeScale();
        }

        if (Input.GetKeyDown(KeyCode.Slash))
        {
            ResetTimeScale();
        }
    }
    void DecreaseTimeScale()
    {
        timeScale = Mathf.Clamp(Time.timeScale - timeScaleStep, minTimeScale, maxTimeScale);
        Time.timeScale = timeScale;
    }

    void IncreaseTimeScale()
    {
        timeScale = Mathf.Clamp(Time.timeScale + timeScaleStep, minTimeScale, maxTimeScale);
        Time.timeScale = timeScale;
    }

    void ResetTimeScale()
    {
        Time.timeScale = defaultTimeScale;
    }
}
