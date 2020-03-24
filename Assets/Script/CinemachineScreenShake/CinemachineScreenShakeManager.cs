using System.Collections.Generic;
using UnityEngine;

namespace Cinemachine
{
    public class CinemachineScreenShakeManager
    {
        private CinemachineScreenShakeManager() {
            m_ShakeEventList = new List<CinemachineScreenShakeEvent>();
        }
        private static CinemachineScreenShakeManager sInstance = null;
        public static CinemachineScreenShakeManager Instance
        {
            get
            {
                if (sInstance == null)
                    sInstance = new CinemachineScreenShakeManager();
                return sInstance;
            }
        }

        private List<CinemachineScreenShakeEvent> m_ShakeEventList;

        public bool IgnoreTimeScale { get; set; }

        float CurrentTime { get { return IgnoreTimeScale ? Time.realtimeSinceStartup : Time.time; } }

        public void AddShakeEvent(CinemachineScreenShakeEvent e)
        {
            e.StartTime = CurrentTime;
            m_ShakeEventList.Add(e);
        }

        public void AddShakeEvent(ISignalSource6D signalSource)
        {
            AddShakeEvent(new CinemachineScreenShakeEvent(signalSource));
        }

        public bool GetShake(out Vector3 pos, out Quaternion rot)
        {
            pos = Vector3.zero;
            rot = Quaternion.identity;
            bool nontrivialResult = false;
            for (int i = m_ShakeEventList.Count - 1; i >= 0; --i)
            {
                CinemachineScreenShakeEvent e = m_ShakeEventList[i];
                if (e.Expired)
                {
                    m_ShakeEventList.RemoveAt(i);
                }
                else
                {
                    nontrivialResult = true;
                    Vector3 pos0 = Vector3.zero;
                    Quaternion rot0 = Quaternion.identity;
                    e.GetSignal(out pos0, out rot0);
                    pos += pos0;
                    rot *= rot0;
                }
            }
            return nontrivialResult;
        }

        public class CinemachineScreenShakeEvent
        {
            public CinemachineScreenShakeEvent() { }
            public CinemachineScreenShakeEvent(ISignalSource6D source)
            {
                ShakeSource = source;
            }
            public float StartTime = 0;
            public ISignalSource6D ShakeSource;

            public bool Expired
            {
                get
                {
                    return StartTime + ShakeSource.SignalDuration <= Instance.CurrentTime;
                }
            }

            public void GetSignal(out Vector3 pos, out Quaternion rot)
            {
                ShakeSource.GetSignal(Instance.CurrentTime - StartTime, out pos, out rot);
            }
        }
    }
}

