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
            m_animator.SetBool("IsMove", true);

        }
        public void Crash()
        {
            ForceStopAllAnimation();
            m_animator.SetTrigger("IsCrash");
        }
        public void ForceStopAllAnimation()
        {
            m_animator.SetFloat("Movement", 0);
            m_animator.SetBool("IsJump", false);
        }
        public void Faster()
        {
            m_animator.speed = 2;
        }
        public void ResetSpeed()
        {
            m_animator.speed = 1;
        }

        public void Jump(bool isJump)
        {
            m_animator.SetBool("IsJump", isJump);
        }

        public void Idle()
        {
            m_animator.SetBool("IsMove", false);

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
