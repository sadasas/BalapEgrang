using System;
using UnityEngine;

namespace Enemy
{
    public class AIAnimationBehaviour
    {
        Animator m_animator;
        float m_walkLerpTime;

        public event Action<AIState> EndAnimation;

        public AIAnimationBehaviour(Animator animator, float walkLerpTime)
        {
            m_animator = animator;
            m_walkLerpTime = walkLerpTime;
        }

        public void Walk()
        {
            var cwv = m_animator.GetFloat("Movement");
            var val = Mathf.Lerp(cwv, 1, m_walkLerpTime);
            m_animator.SetFloat("Movement", val);
        }

        public void Jump(bool isJump)
        {
            m_animator.SetBool("IsJump", isJump);
        }

        public void Faster()
        {
            m_animator.speed = 2;
        }

        public void ResetSpeed()
        {
            m_animator.speed = 1;
        }

        public void Idle()
        {
            var cwv = m_animator.GetFloat("Movement");
            if (cwv > 0.0f)
            {
                var val = Mathf.Lerp(cwv, 0, m_walkLerpTime);
                m_animator.SetFloat("Movement", val);
            }
        }

        public void Crash()
        {
            m_animator.SetFloat("Movement", 0);
            m_animator.SetTrigger("IsCrash");
        }

        public void OnEndAnimation(AIState state)
        {
            EndAnimation?.Invoke(state);
        }
    }
}
