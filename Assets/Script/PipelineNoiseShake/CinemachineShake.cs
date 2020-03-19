using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Serialization;

[DocumentationSorting(DocumentationSortingAttribute.Level.UserRef)]
[AddComponentMenu("")] // Don't display in add component menu
[SaveDuringPlay]
public class CinemachineShake : CinemachineComponentBase
{
    public ISignalSource6D ShakeSetting;

    public override bool IsValid { get { return enabled; } }

    public override CinemachineCore.Stage Stage { get { return CinemachineCore.Stage.Noise; } }

    private float mNoiseTime;
    private Matrix4x4 shakeMatrix = new Matrix4x4();

    //VirtualCamera用来在流水线中计算State的接口
    public override void MutateCameraState(ref CameraState curState, float deltaTime)
    {
        if (!IsValid || deltaTime < 0)
            return;
        if (ShakeSetting == null || mNoiseTime > ShakeSetting.SignalDuration)
            return;
        mNoiseTime += deltaTime;
        ShakeSetting.GetSignal(mNoiseTime, out Vector3 pos, out Quaternion rot);
        //因为这里是希望实现的是震屏功能，所以需要将ShakeSetting计算出的相机空间中的偏移量，转化为世界坐标中的偏移量。
        //直接用相机的旋转生成的矩阵乘一下就可以了
        shakeMatrix.SetTRS(Vector3.zero, curState.FinalOrientation, Vector3.one);
        //把位置偏移量应用到State上
        curState.PositionCorrection += shakeMatrix.MultiplyPoint(-pos);
        rot = Quaternion.SlerpUnclamped(Quaternion.identity, rot, -1);
        //把旋转偏移量应用到State上
        curState.OrientationCorrection = curState.OrientationCorrection * rot;
    }

    public void Shake(ISignalSource6D shakeSetting)
    {
        ShakeSetting = shakeSetting;
        mNoiseTime = 0;
    }

    public void Shake(float delay, Vector3 strength, float cycleTime, float duration)
    {
        Shake(new GameShakeSource(delay, strength, cycleTime, duration));
    }

    public void Shake()
    {
        mNoiseTime = 0;
    }
}
