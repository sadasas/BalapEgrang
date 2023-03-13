using System.Collections;
using UnityEngine;

namespace Player
{
    public enum Pos
    {
        LEFT,
        CENTER,
        RIGHT
    }

    public class MovementBehaviour
    {
        PlayerDataState m_dataState;
        PlayerAnimationBehaviour m_animationBehaviour;
        Transform m_player;
        MonoBehaviour m_mono;
        IInputCallback m_inputCallback;
        Coroutine m_currentCoro;
        bool m_isTurning;

        float m_distanceMove;
        float m_speed;
        float m_turnRange;
        float m_turnSpeed;
        float m_countdownMove;

        public bool IsMoveAllowed = true;

        public MovementBehaviour(
            Transform player,
            IInputCallback inputCallback,
            float speed,
            float turnSpeed,
            float turnRange,
            PlayerAnimationBehaviour animationBehaviour,

            PlayerDataState dataState,
            float distanceMove)
        {
            m_player = player;
            m_mono = player.GetComponent<MonoBehaviour>();
            m_inputCallback = inputCallback;
            m_speed = speed;
            m_turnSpeed = turnSpeed;

            m_inputCallback.OnSwipe += Turn;
            m_inputCallback.OnRelease += () =>
            {
                if (!IsMoveAllowed) return;
                if (m_currentCoro != null) m_countdownMove = distanceMove;
                else
                    m_currentCoro = m_mono.StartCoroutine(Moving());
            };
            m_turnRange = turnRange;
            m_animationBehaviour = animationBehaviour;
            m_dataState = dataState;
            m_distanceMove = distanceMove;
        }

        public void Update()
        {
            if (m_dataState.State == PlayerState.IDLE) Idle();
        }
        public void IncreaseSpeed(int speed)
        {

            m_animationBehaviour.Faster();
            m_speed = m_speed * speed;
        }

        public void DecreaseSpeed(int speed)
        {
            m_animationBehaviour.ResetSpeed();
            m_speed = m_speed / speed;
        }


        public void Idle()
        {
            m_dataState.State = PlayerState.IDLE;
            if (m_currentCoro != null)
            {
                m_mono.StopCoroutine(m_currentCoro);
                m_currentCoro = null;
            }
            m_animationBehaviour.Idle();
        }


        IEnumerator Moving()
        {
            m_countdownMove = m_distanceMove;
            m_dataState.State = PlayerState.WALKING;
            m_animationBehaviour.Walk();
            while (m_countdownMove > 0)
            {
                m_countdownMove -= Time.deltaTime;
                m_player.Translate((Vector3.forward * m_speed) * Time.deltaTime, Space.World);
                yield return null;

            }
            m_currentCoro = null;
            m_dataState.State = PlayerState.IDLE;
        }


        void Turn(Vector3 input)
        {
            if (!IsMoveAllowed || m_isTurning) return;

            if (m_currentCoro != null)
            {
                m_mono.StopCoroutine(m_currentCoro);
                Idle();
            }
            m_dataState.State = PlayerState.TURNING;
            var nextPos = (input.x < 0 ? Pos.LEFT : Pos.RIGHT);

            if (nextPos == Pos.RIGHT && m_dataState.CurrentPost != Pos.RIGHT)
            {
                m_dataState.CurrentPost = m_dataState.CurrentPost == Pos.CENTER ? Pos.RIGHT : Pos.CENTER;
                m_currentCoro = m_mono.StartCoroutine(Turning(m_turnRange));
            }
            else if (nextPos == Pos.LEFT && m_dataState.CurrentPost != Pos.LEFT)
            {
                m_dataState.CurrentPost = m_dataState.CurrentPost == Pos.CENTER ? Pos.LEFT : Pos.CENTER;
                m_currentCoro = m_mono.StartCoroutine(Turning(-m_turnRange));
            }
        }

        IEnumerator Turning(float range)
        {
            m_animationBehaviour.Jump(true);
            var nextPos = new Vector3(m_player.position.x + range, m_player.position.y, m_player.position.z + 0.5f);
            if (range > 0)
            {
                while (m_player.transform.position.x < nextPos.x - 0.2f)
                {
                    var dir = Vector3.Lerp(
                        m_player.position,
                    nextPos,
                        m_turnSpeed
                    );
                    m_player.position = dir;
                    yield return null;
                }

            }
            else
            {
                while (m_player.transform.position.x > nextPos.x + 0.2f)
                {
                    var dir = Vector3.Lerp(
                        m_player.position,
                    nextPos,
                        m_turnSpeed
                    );
                    m_player.position = dir;
                    yield return null;
                }
            }
            m_animationBehaviour.Jump(false);
            m_currentCoro = null;
            m_isTurning = false;

        }
    }
}

