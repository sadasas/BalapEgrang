using Player;
using System.Collections;
using UnityEngine;

namespace Enemy
{
    public class AIBrainBehaviour
    {
        AIDamageBehaviour m_damageBehaviour;
        AIMovementBehaviour m_movementBehaviour;
        Vector3 m_rayStartPoin;
        float m_rayLength;
        Transform m_transform;
        LayerMask m_layerMask;
        AILevel m_brain;
        AIData m_data;
        Coroutine m_coroutine;

        MonoBehaviour m_gameObject;

        public AIBrainBehaviour(float rayLength, LayerMask layerMask, AILevel brain, AIData data, Transform transform, AIDamageBehaviour damageBehaviour, AIMovementBehaviour movementBehaviour, MonoBehaviour gameObject)
        {
            m_gameObject = gameObject;
            m_rayLength = rayLength;
            m_layerMask = layerMask;
            m_brain = brain;
            m_data = data;
            m_transform = transform;

            m_damageBehaviour = damageBehaviour;
            m_movementBehaviour = movementBehaviour;

        }


        public void Update()
        {
            m_rayStartPoin = m_gameObject.GetComponent<Collider>().bounds.center;
        }

        public void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
           Gizmos.DrawLine(m_transform.position, m_transform.position + m_transform.forward * m_rayLength);
        }

        public void SolveCTE(GameObject obstacle)
        {
            m_coroutine = m_gameObject.StartCoroutine(SolvingCTE(obstacle));
        }
        public void ScanBlockObstacle()
        {
           

            if (Physics.Raycast(m_transform.position,  m_transform.forward, m_rayLength,m_layerMask))
            {

              
                if (MakeDecision(0, m_brain.DecisionVarian) == 0)
                {
                    m_data.State = AIState.TURNING;
                }

            }
        }

        public int MakeDecision(int minRnd, int maxRnd)
        {
            var rand = UnityEngine.Random.Range(minRnd, maxRnd);

            return rand;
        }

        public void TurnDecision()
        {
            if (m_movementBehaviour.Coroutine != null) return;

            if (m_data.CurrentPos == Pos.RIGHT)
            {
                m_data.CurrentPos = Pos.CENTER;
                m_movementBehaviour.Turn(-1);

            }
            else if (m_data.CurrentPos == Pos.LEFT)
            {
                m_data.CurrentPos = Pos.CENTER;
                m_movementBehaviour.Turn(1);

            }
            else
            {
                var rand = MakeDecision(0, 2);
                if (rand == 0)
                {
                    m_data.CurrentPos = Pos.LEFT;
                    m_movementBehaviour.Turn(-1);
                }
                else
                {
                    m_data.CurrentPos = Pos.RIGHT;
                    m_movementBehaviour.Turn(1);
                }
            }



        }

        IEnumerator SolvingCTE(GameObject obstacle)
        {
            var countDown = (float)MakeDecision((int)m_brain.DecisionMinCost, (int)m_brain.DecisionMaxCost);
            var pos = new Vector3(m_transform.position.x, m_transform.position.y, m_transform.position.z);
            while (countDown > 0)
            {
                m_transform.position = pos;
                countDown -= Time.deltaTime;
                yield return null;
            }
            if (MakeDecision(0, m_brain.DecisionVarian) != 0)
            {
                m_damageBehaviour.Crash(obstacle);
            }
            else
            {
               
                ///if collapse ?
                m_data.State = AIState.MOVING;
                m_movementBehaviour.IncreaseSpeed();

            }
            m_coroutine = null;

        }

    }
}