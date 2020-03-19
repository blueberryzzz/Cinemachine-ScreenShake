using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GameShakeSource : ISignalSource6D
{
    public float Delay;
    public Vector3 Strength;
    public float CycleTime;
    public float Duration;

    public GameShakeSource(float delay, Vector3 strength, float cycleTime, float duration)
    {
        Delay = delay;
        Strength = strength;
        CycleTime = cycleTime;
        Duration = duration;
    }

    //噪音持续的总时间，用于判断这个噪音是否结束
    public float SignalDuration
    {
        get
        {
            return Delay + Duration;
        }
    }

    //根据当前噪音经过的时间，获取噪音产生的位置和旋转偏移量。
    //因为表格中没有旋转相关的数据，所以直接返回identity值。
    public void GetSignal(float timeSinceSignalStart, out Vector3 pos, out Quaternion rot)
    {
        if (timeSinceSignalStart <= Delay)
        {
            pos = Vector3.zero;
        }
        else
        {
            float times = timeSinceSignalStart / (CycleTime / 4);
            int cycle25Count = Mathf.FloorToInt(times);
            float inCycle25Time = times - cycle25Count;
            if (cycle25Count % 4 == 0)
            {
                pos = Vector3.Lerp(Vector3.zero, Strength, inCycle25Time);
            }
            else if (cycle25Count % 4 == 1)
            {
                pos = Vector3.Lerp(Strength, Vector3.zero, inCycle25Time);
            }
            else if (cycle25Count % 4 == 2)
            {
                pos = Vector3.Lerp(Vector3.zero, -Strength, inCycle25Time);
            }
            else
            {
                pos = Vector3.Lerp(-Strength, Vector3.zero, inCycle25Time);
            }
        }
        rot = Quaternion.identity;
    }
}