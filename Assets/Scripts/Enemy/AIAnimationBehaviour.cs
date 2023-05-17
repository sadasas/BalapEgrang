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
            m_animator.SetBool("IsMove", true);
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


        public void ForceStopAnim()
        {

            m_animator.SetBool("IsMove", false);
            m_animator.SetBool("IsJump", false);
        }
        public void Idle()
        {
            m_animator.SetBool("IsMove", false);
        }

        public void Crash()
        {
            m_animator.SetBool("IsMove", false);
            m_animator.SetTrigger("IsCrash");
        }

        public void OnEndAnimation(AIState state)
        {
            EndAnimation?.Invoke(state);
        }
    }
}
