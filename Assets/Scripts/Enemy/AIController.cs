using Player;
using Race;
using UnityEngine;

namespace Enemy
{

    public class AIData
    {
        public AIState State;
        public Pos CurrentPos;
    }
    public class AIController : MonoBehaviour, IRacer
    {
        AIData m_data;
        AIMovementBehaviour m_movementBehaviour;
        AIBrainBehaviour m_brainBehaviour;
        public AIDamageBehaviour DamageBehaviour;


        [Header("Movement Setting")]
        [SerializeField] float m_speed;
        [SerializeField] float m_respawnDelay;
        [SerializeField] int m_respawnPosDis;
        [SerializeField] float m_turnSpeed;
        [SerializeField] float m_turnRange;

        [Header("Detect Obstacle Setting")]
        [SerializeField] float m_rayLength;
        [SerializeField] LayerMask m_obstacleLayer;

        [Header("Ability Setting")]
        [SerializeField] float m_timeFaster;
        [SerializeField] float m_speedIncrease;

        public AILevel Brain;



        private void Start()
        {
            m_data ??= new();
            DamageBehaviour = new(m_respawnDelay, m_respawnPosDis, transform, m_data);
            m_movementBehaviour = new(transform, m_turnRange, m_turnSpeed, m_data, m_speed,m_timeFaster,m_speedIncrease);
            m_brainBehaviour = new(m_rayLength, m_obstacleLayer, Brain, m_data, transform, DamageBehaviour, m_movementBehaviour, this);
        }

        private void Update()
        {
            m_brainBehaviour.Update();

            switch (m_data.State)
            {
                case AIState.IDLE:
                    break;
                case AIState.CRASHED:
                    break;
                case AIState.MOVING:
                    m_movementBehaviour.Move();
                    m_brainBehaviour.ScanBlockObstacle();
                    break;
                case AIState.TURNING:
                    m_brainBehaviour.TurnDecision();
                    break;
                default:
                    break;
            }

        }
        private void OnDrawGizmos()
        {
            m_brainBehaviour.OnDrawGizmos();
            
        }

        public void OnCTETriggered(GameObject obstacle)
        {
            m_data.State = AIState.DECISING;
            m_brainBehaviour.SolveCTE(obstacle);
        }



        public void FinishRace()
        {
            m_data.State = AIState.IDLE;
        }

        public void StartRace()
        {
            m_data.State = AIState.MOVING;
        }

        public void WaitStart(Pos currentPos)
        {
            m_data ??= new();
            m_data.State = AIState.IDLE;
            m_data.CurrentPos = currentPos;
        }
    }
}