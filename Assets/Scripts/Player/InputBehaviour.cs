using System;
using UnityEngine;

namespace Player
{
    public class InputBehaviour : IInputCallback
    {
        float m_turnMinLength;
        Vector2 m_touchStart;
        Vector2 m_touchEnd;
        Vector2 m_touchDir;

        public InputBehaviour(float turnMinLength)
        {
            m_turnMinLength = turnMinLength;
        }

        public event Action OnHold;
        public event Action OnTap;
        public event Action<Vector2> OnSwipe;
        public event Action OnRelease;

        public void OnUpdate()
        {
            var tc = Input.touchCount;
            if (tc == 0)
                return;

            var t = Input.GetTouch(0);

            if (t.phase == TouchPhase.Began)
            {
                OnTap?.Invoke();
                m_touchStart = t.position;
                m_touchEnd = t.position;
            }
            else if (t.phase == TouchPhase.Moved)
            {
                m_touchEnd = t.position;
                m_touchDir = t.deltaPosition.normalized;
            }
            else if (t.phase == TouchPhase.Ended)
            {
                m_touchEnd = t.position;

                var distanceTouch = Mathf.Abs(Vector2.Distance(m_touchStart, m_touchEnd));

                if (distanceTouch < m_turnMinLength)
                    OnRelease?.Invoke();
                else
                {

                    OnSwipe?.Invoke(m_touchDir);

                }

            }
        }
    }
}

