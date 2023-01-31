using System;
using UnityEngine;

namespace Enemy
{
    public class AIDamageBehaviour
    {
        AIAnimationBehaviour m_animationBehaviour;
        float m_respawnDelay;
        float m_respawnPosDis;
        Transform m_transform;
        AIData m_data;
        MonoBehaviour m_gameObject;
        Transform m_obstacle;

        public event Action OnRespawn;
        public AIDamageBehaviour(float respawnDelay, float respawnPosDis, Transform transform, AIData data, AIAnimationBehaviour animationBehaviour)
        {
            m_respawnDelay = respawnDelay;
            m_respawnPosDis = respawnPosDis;
            m_transform = transform;
            m_data = data;
            m_animationBehaviour = animationBehaviour;

            m_gameObject = m_transform.GetComponent<MonoBehaviour>();
            m_animationBehaviour.EndAnimation += state =>
            {
                if (state == AIState.CRASHED) Respawn();
            };
        }
        public void Crash(GameObject obstacle)
        {
            
            m_obstacle = obstacle.transform;
            m_data.State = AIState.CRASHED;
            m_animationBehaviour.Crash();
        }

        void Respawn()
        {
            var pos = new Vector3(m_transform.position.x, m_transform.position.y, m_obstacle.position.z + m_respawnPosDis);
            m_transform.position = pos;
            m_data.State = AIState.MOVING;
            OnRespawn?.Invoke();
        }


    }
}