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
        float m_rayheight;
        Transform m_transform;
        LayerMask m_layerMask;
        AILevel m_brain;
        AIData m_data;
        Coroutine m_coroutine;
        bool isDecided = false;
        bool isLastStateCrash = false;
        MonoBehaviour m_gameObject;

        public AIBrainBehaviour(float rayLength, LayerMask layerMask, AILevel brain, AIData data, Transform transform, AIDamageBehaviour damageBehaviour, AIMovementBehaviour movementBehaviour, MonoBehaviour gameObject, float rayheight)
        {
            m_gameObject = gameObject;
            m_rayLength = rayLength;
            m_layerMask = layerMask;
            m_brain = brain;
            m_data = data;
            m_transform = transform;
            m_rayheight = rayheight;

            m_damageBehaviour = damageBehaviour;
            m_movementBehaviour = movementBehaviour;


            damageBehaviour.OnRespawn += () => isDecided = false;
        }


        public void Update()
        {
            if (m_data.State == AIState.CRASHED && !isLastStateCrash) isLastStateCrash = true;
            m_rayStartPoin = m_gameObject.GetComponent<Collider>().bounds.center;
        }

        public void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(m_transform.position + Vector3.up * m_rayheight, (m_transform.position + Vector3.up * m_rayheight) + m_transform.forward * m_rayLength);
        }

        public void SolveMatchBar(Transform pos)
        {
            if (m_coroutine != null) return;
            m_coroutine = m_gameObject.StartCoroutine(SolvingCTE(pos));
        }
        public void ScanBlockObstacle()
        {

            if (isDecided) return;

            if (Physics.Raycast(m_transform.position + Vector3.up * m_rayheight, m_transform.forward, m_rayLength, m_layerMask))
            {

                if (MakeDecision(0, m_brain.DecisionVarian) == 0 || isLastStateCrash)
                {
                    isLastStateCrash = false;
                    TurnDecision();
                }
                isDecided = true;

            }
        }

        public int MakeDecision(int minRnd, int maxRnd)
        {
            var rand = UnityEngine.Random.Range(minRnd, maxRnd);

            return rand;
        }

        public void TurnDecision()
        {
            m_data.State = AIState.TURN_DECISING;

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

            isDecided = false;

        }

        IEnumerator SolvingCTE(Transform currentPos)
        {
            var decision = MakeDecision(0, m_brain.DecisionVarian);
            var countDown = (float)MakeDecision((int)m_brain.DecisionMinCost, (int)m_brain.DecisionMaxCost);
            var pos = new Vector3(m_transform.position.x, m_transform.position.y, m_transform.position.z);
            while (countDown > 0)
            {
                m_transform.position = pos;
                countDown -= Time.deltaTime;
                yield return null;
            }
            if (decision != 0 && !isLastStateCrash)
            {
                isLastStateCrash = true;
                m_damageBehaviour.Crash(currentPos);
            }
            else
            {
                ///if collapse ?
                m_data.State = AIState.MOVING;
            }
            m_coroutine = null;

        }

    }
}
