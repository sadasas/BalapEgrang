using System;
using UnityEngine;

namespace Player
{

    public class PlayerAnimationBehaviour
    {
        Animator m_animator;
        float m_walkLerpTime;
        PlayerDataState m_dataState;
        public event Action<PlayerState> OnEndAnim;
        public PlayerAnimationBehaviour(float walkLerpTime, Animator animator, PlayerDataState dataState)
        {
            m_walkLerpTime = walkLerpTime;
            m_animator = animator;
            m_dataState = dataState;
        }


        public void Walk()
        {
            var cwv = m_animator.GetFloat("Movement");
            var val = Mathf.Lerp(cwv, 1, m_walkLerpTime);
            m_animator.SetFloat("Movement", val);

        }
        public void Crash()
        {
            m_animator.SetTrigger("IsCrash");
        }
        public void ForceStopAllAnimation()
        {
            m_animator.SetFloat("Movement", 0);
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

        public void Turn(bool isTurn)
        {
            m_animator.SetBool("IsTurn", isTurn);
        }
        public void OnEndAnimCallback(PlayerState state)
        {

            OnEndAnim?.Invoke(state);
        }
    }
}
