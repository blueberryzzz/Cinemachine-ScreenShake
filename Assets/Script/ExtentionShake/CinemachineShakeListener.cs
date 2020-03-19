using UnityEngine;
using Cinemachine;

[SaveDuringPlay]
[AddComponentMenu("")] // Hide in menu
[DocumentationSorting(DocumentationSortingAttribute.Level.UserRef)]
#if UNITY_2018_3_OR_NEWER
[ExecuteAlways]
#else
    [ExecuteInEditMode]
#endif
public class CinemachineShakeListener : CinemachineExtension
{
    /// <summary>
    /// Impulse events on channels not included in the mask will be ignored.
    /// </summary>
    [Tooltip("Impulse events on channels not included in the mask will be ignored.")]
    [CinemachineImpulseChannelProperty]
    public int m_ChannelMask = 1;

    /// <summary>
    /// Gain to apply to the Impulse signal.
    /// </summary>
    [Tooltip("Gain to apply to the Impulse signal.  1 is normal strength.  Setting this to 0 completely mutes the signal.")]
    public float m_Gain = 1;

    /// <summary>
    /// Enable this to perform distance calculation in 2D (ignore Z).
    /// </summary>
    [Tooltip("Enable this to perform distance calculation in 2D (ignore Z)")]
    public bool m_Use2DDistance = false;

    private Matrix4x4 shakeMatrix = new Matrix4x4();

    //VirtualCamera用来在流水线中计算State的接口
    protected override void PostPipelineStageCallback(
        CinemachineVirtualCameraBase vcam,
        CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
        //由于这个接口在么个阶段后都会调用，所以要加这个判断。
        //保证只在Aim结束后指调用一次
        if (stage == CinemachineCore.Stage.Aim)
        {
            Vector3 impulsePos = Vector3.zero;
            Quaternion impulseRot = Quaternion.identity;
            //直接调ImpulseManager的接口获取gameShakeChannel产生的震动数据,
            //位置填zero,保证噪音不会衰减
            if (CinemachineImpulseManager.Instance.GetImpulseAt(
                Vector3.zero, m_Use2DDistance, m_ChannelMask, out impulsePos, out impulseRot))
            {
                //转换到世界坐标                      
                shakeMatrix.SetTRS(Vector3.zero, state.FinalOrientation, Vector3.one);
                //增加强度参数的影响后，应用到当前State上
                state.PositionCorrection += shakeMatrix.MultiplyPoint(impulsePos * -m_Gain);
                impulseRot = Quaternion.SlerpUnclamped(Quaternion.identity, impulseRot, -m_Gain);
                state.OrientationCorrection = state.OrientationCorrection * impulseRot;
            }
        }
    }
}
