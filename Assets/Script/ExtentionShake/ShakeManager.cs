using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ShakeManager
{
    public static void Test()
    {
        AddShake(0, new Vector3(0.3f, 0.3f, 0), 0.2f, 0.1f);
    }

    public static void AddShake(float delay, Vector3 strength, float cycleTime, float duration)
    {
        CinemachineImpulseManager.ImpulseEvent e
                = CinemachineImpulseManager.Instance.NewImpulseEvent();
        e.m_Envelope = new CinemachineImpulseManager.EnvelopeDefinition();
        //开始和衰减阶段的时间都填0，只留下中间一段时间
        e.m_Envelope.m_AttackTime = 0;
        e.m_Envelope.m_DecayTime = 0;
        e.m_Envelope.m_SustainTime = delay + duration;

        e.m_SignalSource = new GameShakeSource(delay, strength, cycleTime, duration);
        //产生冲击的位置和影响半径，这里填Vector3.zero和float.MaxValue，
        //获取的震动数据的时候从Vector3.zero这个位置获取就可以获取全量没有衰减的数据。
        e.m_Position = Vector3.zero;
        e.m_Radius = float.MaxValue;
        //2就是刚定义的gameShakeChannel
        e.m_Channel = 2;
        //选Fixed，不希望震动的方向对相机产生额外影响
        e.m_DirectionMode = CinemachineImpulseManager.ImpulseEvent.DirectionMode.Fixed;
        //衰减方式随便填，这里用不到
        e.m_DissipationMode = CinemachineImpulseManager.ImpulseEvent.DissipationMode.LinearDecay;
        //这个也用不到
        e.m_DissipationDistance = 0;
        CinemachineImpulseManager.Instance.AddImpulseEvent(e);
    }
}
