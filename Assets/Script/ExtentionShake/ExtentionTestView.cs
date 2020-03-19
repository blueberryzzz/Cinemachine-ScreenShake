using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExtentionTestView : MonoBehaviour
{
    private float shakeTime = 0;

    public float Delay = 0;
    public Vector3 Strength = new Vector3(0.3f, 0.3f, 0);
    public float CycleTime = 0.2f;
    public float Duration = 0.1f;

    private void Start()
    {
        
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Space) && Time.time - shakeTime > 0.3f)
        {
            shakeTime = Time.time;
            ShakeManager.AddShake(Delay, Strength, CycleTime, Duration);
        }
    }
}
