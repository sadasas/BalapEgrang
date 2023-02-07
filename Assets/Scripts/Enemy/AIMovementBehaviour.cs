using System;
using System.Collections;
using UnityEngine;

namespace Enemy
{
    public class AIMovementBehaviour
    {
        Transform m_transform;
        MonoBehaviour m_gameObject;
        AIAnimationBehaviour m_animationBehaviour;
        float m_speed;
        float m_turnRange;
        float m_turnSpeed;
        //faster ability
        float m_timeFaster;
        float m_speedIncrease;

        AIData m_data;


        public Coroutine Coroutine { get; private set; }

        public AIMovementBehaviour(Transform transform, float turnRange, float turnSpeed, AIData data, float speed, float timeFaster, float speedIncrease, AIAnimationBehaviour animationBehaviour)
        {
            m_transform = transform;
            m_speed = speed;
            m_turnRange = turnRange;
            m_turnSpeed = turnSpeed;
            m_data = data;
            m_timeFaster = timeFaster;
            m_speedIncrease = speedIncrease;
            m_animationBehaviour = animationBehaviour;

            m_gameObject = transform.GetComponent<MonoBehaviour>();
        }


        public void IncreaseSpeed()
        {
            Coroutine = m_gameObject.StartCoroutine(Faster());
        }
        public void Move()
        {
            m_animationBehaviour.Walk();
            m_transform.Translate((m_transform.forward * m_speed) * Time.deltaTime, Space.World);
        }

        public void Turn(float dirX)
        {
            if (Coroutine != null) return;
            Coroutine = m_gameObject.StartCoroutine(Turning(dirX));
        }

        public void Idle()
        {
            m_animationBehaviour.Idle();
        }
        IEnumerator Turning(float dirX)
        {
            m_animationBehaviour.Jump(true);
            var nextPos = new Vector3(m_transform.position.x + dirX * m_turnRange, m_transform.position.y, m_transform.position.z + 0.5f);
            if (dirX > 0)
            {
                while (m_transform.position.x < nextPos.x - 0.2f)
                {
                    var dir = Vector3.Lerp(
                        m_transform.position,
                    nextPos,
                        m_turnSpeed
                    );
                    m_transform.position = dir;
                    yield return null;
                }

            }
            else
            {
                while (m_transform.position.x > nextPos.x + 0.2f)
                {
                    var dir = Vector3.Lerp(
                        m_transform.position,
                    nextPos,
                        m_turnSpeed
                    );
                    m_transform.position = dir;
                    yield return null;
                }
            }
            m_animationBehaviour.Jump(false);
            m_data.State = AIState.MOVING;

            Coroutine = null;


        }

        IEnumerator Faster()
        {
            m_animationBehaviour.Faster();
            var countDown = m_timeFaster;
            m_speed += m_speedIncrease;
            while (countDown > 0.0f)
            {

                countDown -= Time.deltaTime;
                yield return null;
            }
            m_speed -= m_speedIncrease;
            m_animationBehaviour.ResetSpeed();
            Coroutine = null;
        }
    }
}
