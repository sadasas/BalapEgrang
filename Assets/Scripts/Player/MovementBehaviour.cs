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

    public enum MoveType
    {
        PERFECT,
        GOOD
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
        MoveType m_currentTypeMove;

        float m_distanceMove;
        float m_countdownMove;
        float m_perfectMove;
        float m_goodMove;
        float m_speed;
        float m_acceleration;
        float m_turnRange;
        float m_turnSpeed;

        public bool IsMoveAllowed = true;

        public MovementBehaviour(
            Transform player,
            IInputCallback inputCallback,
            float speed,
            float acceleration,
                        float distanceMove,
            float turnSpeed,
            float turnRange,
            PlayerAnimationBehaviour animationBehaviour,
            PlayerDataState dataState
            )
        {
            m_player = player;
            m_mono = player.GetComponent<MonoBehaviour>();
            m_inputCallback = inputCallback;
            m_speed = speed;
            m_acceleration = acceleration;
            m_distanceMove = distanceMove;
            m_turnSpeed = turnSpeed;


            m_inputCallback.OnSwipe += Turn;
            m_turnRange = turnRange;
            m_animationBehaviour = animationBehaviour;
            m_dataState = dataState;

        }

        public void Update()
        {
            if (m_dataState.State == PlayerState.IDLE) Idle();

        }


        public void ForceStopMovement()
        {
            if (m_currentCoro != null)
            {
                m_mono.StopCoroutine(m_currentCoro);
                m_currentCoro = null;

                m_isTurning = false;
            }
        }

        void IncreaseSpeed()
        {

            m_animationBehaviour.Faster();
            m_speed = m_speed * m_acceleration;
        }

        void DecreaseSpeed()
        {
            m_animationBehaviour.ResetSpeed();
            m_speed = m_speed / m_acceleration;
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


        public void Move(MoveType moveType)
        {

            if (!IsMoveAllowed) return;
            if (m_currentCoro != null)
            {
                m_countdownMove += m_distanceMove;
                if (moveType == MoveType.PERFECT)
                {
                    if (m_currentTypeMove != MoveType.PERFECT)
                    {
                        IncreaseSpeed();
                    }

                }
                else
                {
                    m_currentTypeMove = moveType;
                    DecreaseSpeed();
                }

            }
            else
            {
                m_currentTypeMove = moveType;
                m_countdownMove = m_distanceMove;
                m_currentCoro = m_mono.StartCoroutine(Moving());
            }

        }
        IEnumerator Moving()
        {
            if (m_currentTypeMove == MoveType.PERFECT)
            {
                IncreaseSpeed();
            }
            m_dataState.State = PlayerState.WALKING;
            m_animationBehaviour.Walk();
            while (m_countdownMove > 0)
            {
                m_countdownMove -= Time.deltaTime;
                m_player.Translate((Vector3.forward * m_speed) * Time.deltaTime, Space.World);
                yield return null;

            }
            if (m_currentTypeMove == MoveType.PERFECT)
            {
                DecreaseSpeed();
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
            var dir = (input.x < 0 ? Pos.LEFT : Pos.RIGHT);

            Pos nextPosEnum;
            if (dir == Pos.RIGHT && m_dataState.CurrentPost != Pos.RIGHT)
            {
                nextPosEnum = m_dataState.CurrentPost == Pos.CENTER ? Pos.RIGHT : Pos.CENTER;
                m_currentCoro = m_mono.StartCoroutine(Turning(nextPosEnum, m_turnRange));
            }
            else if (dir == Pos.LEFT && m_dataState.CurrentPost != Pos.LEFT)
            {
                nextPosEnum = m_dataState.CurrentPost == Pos.CENTER ? Pos.LEFT : Pos.CENTER;
                m_currentCoro = m_mono.StartCoroutine(Turning(nextPosEnum, -m_turnRange));
            }
        }

        IEnumerator Turning(Pos nextPosEnum, float range)
        {
            m_isTurning = true;
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
            m_dataState.CurrentPost = nextPosEnum;
            m_animationBehaviour.Jump(false);
            m_currentCoro = null;
            m_isTurning = false;

        }
    }
}

