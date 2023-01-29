using Player;
using Race;
using System;
using System.Collections;
using UnityEngine;

namespace Enemy
{
    public class EnemyController : MonoBehaviour, IRacer
    {
        EnemyState m_state = EnemyState.IDLE;

        Vector3 m_raycastOrigin;
        Coroutine m_coroState;

        [Header("Movement Setting")]
        [SerializeField] float m_speed;
        [SerializeField] float m_respawnDelay;
        [SerializeField] int m_respawnPosDis;
        [SerializeField] float m_turnSpeed;
        [SerializeField] float m_turnRange;

        [Header("Detect Obstacle Setting")]
        [SerializeField] float m_raycastLength;
        [SerializeField] LayerMask m_obstacleLayer;

        [Header("Ability Setting")]
        [SerializeField] float m_timeFaster;
        [SerializeField] float m_speedIncrease;

        public EnemyLevel Brain;
        public Pos CurrentPost { get; set; }

        private void Update()
        {
            m_raycastOrigin = GetComponent<Collider>().bounds.center;
            switch (m_state)
            {
                case EnemyState.IDLE:
                    break;
                case EnemyState.CRASHED:
                    break;
                case EnemyState.MOVING:
                    Move();
                    DetecthBlockObstacle();
                    break;
                case EnemyState.TURNING:
                    Turn();
                    break;
                default:
                    break;
            }

        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(m_raycastOrigin, m_raycastOrigin + transform.forward * m_raycastLength);
        }
        void Move()
        {

            transform.Translate((transform.forward * m_speed) * Time.deltaTime, Space.World);
        }
        void Turn()
        {
            if (m_coroState != null) return;
          
            if (CurrentPost == Pos.RIGHT)
            {
                CurrentPost = Pos.CENTER;
                m_coroState = StartCoroutine(Turning(-m_turnRange));

            }
            else if (CurrentPost == Pos.LEFT)
            {
                CurrentPost = Pos.CENTER;
                m_coroState =  StartCoroutine(Turning(m_turnRange));

            }
            else
            {
                var rand = MakeDecision(0, 2);
                if (rand == 0)
                {
                    CurrentPost = Pos.LEFT;
                    m_coroState = StartCoroutine(Turning(-m_turnRange));
                }
                else
                {
                    CurrentPost = Pos.RIGHT;
                    m_coroState = StartCoroutine(Turning(m_turnRange));
                }
            }



        }

        void DetecthBlockObstacle()
        {

           
            if (Physics.Raycast(m_raycastOrigin, m_raycastOrigin + transform.forward, m_raycastLength, m_obstacleLayer))
            {


                if (MakeDecision(0, Brain.DecisionVarian) == 0)
                {
                    m_state = EnemyState.TURNING;
                }

            }
        }

        int MakeDecision(int minRnd, int maxRnd)
        {
            var rand = UnityEngine.Random.Range(minRnd, maxRnd);

            return rand;
        }
        IEnumerator SolvingCTE(GameObject obstacle)
        {
            var countDown = (float)MakeDecision((int)Brain.DecisionMinCost, (int)Brain.DecisionMaxCost);
            var pos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            while (countDown > 0)
            {
                transform.position = pos;
                countDown -= Time.deltaTime;
                yield return null;
            }
            if (MakeDecision(0, Brain.DecisionVarian) != 0)
            {
                Crashed(gameObject);
            }
            else
            {
                ///if collapse ?
                m_state = EnemyState.MOVING;
                StartCoroutine(Faster());

            }

        }

        IEnumerator Crashing(Vector3 respawnPoin)
        {

            var countDown = m_respawnDelay;
            var pos = new Vector3(respawnPoin.x, transform.position.y, respawnPoin.z + m_respawnPosDis);
            while (countDown > 0.0f)
            {
                countDown -= Time.deltaTime;
                yield return null;
                transform.position = pos;
            }

            m_state = EnemyState.MOVING;
        }

        IEnumerator Turning(float range)
        {
            var dis = 0f;
            var lastPos = transform.position.x;
            while (dis < MathF.Abs(range))
            {
                dis += MathF.Abs(transform.position.x - lastPos);
                var dir = Vector3.Lerp(
                    transform.position,
                   transform.position + (transform.right * range),
                    m_turnSpeed
                );
                transform.position = dir;
                yield return null;
            }
           
            m_state = EnemyState.MOVING;
            m_coroState = null;

        }

        IEnumerator Faster()
        {

            var countDown = m_timeFaster;
            m_speed += m_speedIncrease;
            while (countDown > 0.0f)
            {

                countDown -= Time.deltaTime;
                yield return null;
            }
            m_speed -= m_speedIncrease;
        }
        public void OnCTETriggered(GameObject obstacle)
        {
            m_state = EnemyState.DECISING;
            StartCoroutine(SolvingCTE(obstacle));
        }


        public void Crashed(GameObject obstacle)
        {
            m_state = EnemyState.CRASHED;
            StartCoroutine(Crashing(obstacle.transform.position));
        }
        public void FinishRace()
        {
            m_state = EnemyState.IDLE;
        }

        public void StartRace()
        {
            m_state = EnemyState.MOVING;
        }

        public void WaitStart(Pos currentPos)
        {
            m_state = EnemyState.IDLE;
            CurrentPost = currentPos;
        }
    }
}