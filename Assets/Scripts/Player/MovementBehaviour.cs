using System;
using UnityEngine;
using System.Collections;

namespace Player
{
    public class MovementBehaviour
    {
        public enum Pos
        {
            LEFT,
            CENTER,
            RIGHT
        }

        Transform m_player;
        MonoBehaviour m_mono;
        Coroutine m_currentState;
        IInputCallback m_inputCallback;
        Pos m_currentPost = Pos.CENTER;

        float m_speed;
        float m_turnRange;
        float m_turnSpeed;

        public bool IsPlaying = true;

        public MovementBehaviour(
            Transform player,
            IInputCallback inputCallback,
            float speed,
            float turnSpeed,
            float turnRange)
        {
            m_player = player;
            m_mono = player.GetComponent<MonoBehaviour>();
            m_inputCallback = inputCallback;
            m_speed = speed;
            m_turnSpeed = turnSpeed;

            m_inputCallback.OnHold += MoveForward;
            m_inputCallback.OnSwipe += Turn;
            m_turnRange = turnRange;
        }

        void MoveForward()
        {
            if (!IsPlaying) return;
            m_player.Translate((m_player.forward * m_speed) * Time.deltaTime, Space.World);
        }

        void Turn(Vector3 input)
        {
            if (!IsPlaying) return;
            var nextPos = (input.x < 0 ? Pos.LEFT : Pos.RIGHT);

            if (nextPos == Pos.RIGHT && m_currentPost != Pos.RIGHT)
            {
                m_currentPost = m_currentPost == Pos.CENTER ? Pos.RIGHT : Pos.CENTER;
                m_mono.StartCoroutine(Turning(m_turnRange));
            }
            else if (nextPos == Pos.LEFT && m_currentPost != Pos.LEFT)
            {
                m_currentPost = m_currentPost == Pos.CENTER ? Pos.LEFT : Pos.CENTER;
                m_mono.StartCoroutine(Turning(-m_turnRange));
            }
        }

        IEnumerator Turning(float range)
        {
            var dis = 0f;
            var lastPos = m_player.position.x;
            while (dis < MathF.Abs(range))
            {
                dis += MathF.Abs(m_player.position.x - lastPos);
                var dir = Vector3.Lerp(
                    m_player.position,
                   m_player.position + (m_player.right * range),
                    m_turnSpeed
                );
                m_player.position = dir;
                yield return null;
            }
        }
    }
}

