using System.Collections;
using UnityEngine;

namespace Enemy
{
    public class AIDamageBehaviour
    {
        float m_respawnDelay;
        float m_respawnPosDis;
        Transform m_transform;
        AIData m_data;
        MonoBehaviour m_gameObject;

        public AIDamageBehaviour(float respawnDelay, float respawnPosDis, Transform transform, AIData data)
        {
            m_respawnDelay = respawnDelay;
            m_respawnPosDis = respawnPosDis;
            m_transform = transform;
            m_data = data;

            m_gameObject = m_transform.GetComponent<MonoBehaviour>();
        }
        public void Crash(GameObject obstacle)
        {
            m_data.State = AIState.CRASHED;
            m_gameObject.StartCoroutine(Crashing(obstacle.transform.position));
        }
        IEnumerator Crashing(Vector3 respawnPoin)
        {

            var countDown = m_respawnDelay;
            var pos = new Vector3(respawnPoin.x, m_transform.position.y, respawnPoin.z + m_respawnPosDis);
            while (countDown > 0.0f)
            {
                countDown -= Time.deltaTime;
                yield return null;
                m_transform.position = pos;
            }

            m_data.State = AIState.MOVING;
        }

    }
}