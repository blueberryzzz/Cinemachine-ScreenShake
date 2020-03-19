#define DEBUG_LOG_NAME
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PipelineNoiseShakeTestView : MonoBehaviour
{
    public List<CinemachineShake> ShakeComponents;
    public CinemachineFreeLook FreeLook;

    private float shakeTime = 0;

    public float Delay = 0;
    public Vector3 Strength = new Vector3(0.3f, 0.3f, 0);
    public float CycleTime = 0.2f;
    public float Duration = 0.1f;

    private void Start()
    {
        ShakeComponents = new List<CinemachineShake>();
        ShakeComponents.Add(FreeLook.GetRig(0).GetCinemachineComponent<CinemachineShake>());
        ShakeComponents.Add(FreeLook.GetRig(1).GetCinemachineComponent<CinemachineShake>());
        ShakeComponents.Add(FreeLook.GetRig(2).GetCinemachineComponent<CinemachineShake>());
    }

    private void Update()
    {
        if (ShakeComponents.Count > 0 && Input.GetKey(KeyCode.Space) && Time.time - shakeTime > 0.3f)
        {
            shakeTime = Time.time;
            foreach(var shakeComponent in ShakeComponents)
            {
                shakeComponent.Shake(Delay, Strength, CycleTime, Duration);
            }
        }
    }
}
