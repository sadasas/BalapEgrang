using System;
using UnityEngine;

namespace Player
{
    public class InputBehaviour : IInputCallback
    {
        float m_movePressTime = 0;
        float m_turnTreshold;
        float m_turnMinLength;
        bool m_isMoved;

        public InputBehaviour(float turnTreshold, float turnMinLength)
        {
            m_turnTreshold = turnTreshold;
            m_movePressTime = m_turnTreshold;
            m_turnMinLength = turnMinLength;
        }

        public event Action OnHold;
        public event Action OnTap;
        public event Action<Vector3> OnSwipe;
        public event Action OnRelease;

        public void OnUpdate()
        {
            var tc = Input.touchCount;
            if (tc == 0)
                return;

            var t = Input.GetTouch(0);

            if (t.phase == TouchPhase.Stationary)
            {
                if (m_movePressTime > 0)
                    m_movePressTime -= Time.deltaTime;
                OnHold?.Invoke();
            }
            else if (t.phase == TouchPhase.Moved)
            {
                var tdir = t.deltaPosition.normalized;

                if (m_movePressTime > 0 || MathF.Abs(tdir.x) < m_turnMinLength || MathF.Abs(tdir.y) > 0.3)
                    return;
                m_movePressTime = m_turnTreshold;
                OnSwipe?.Invoke(tdir);
                m_isMoved = true;
            }
            else if (t.phase == TouchPhase.Began)
            {
                OnTap?.Invoke();
            }
            else if (t.phase == TouchPhase.Ended)
            {
                if (!m_isMoved)
                    OnRelease?.Invoke();
                m_isMoved = false;
            }
        }
    }
}

