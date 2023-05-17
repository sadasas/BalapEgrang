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
        AIAnimationBehaviour m_animationBehaviour;
        public AIDamageBehaviour DamageBehaviour;
        public string ID { get; set; }


        [Header("Movement Setting")]
        [SerializeField] float m_countdownMove;
        [SerializeField] float m_respawnDelay;
        [SerializeField] int m_respawnPosDis;
        [SerializeField] float m_turnSpeed;
        [SerializeField] float m_turnRange;

        [Header("Detect Obstacle Setting")]
        [SerializeField] float m_rayLength;
        [SerializeField] float m_rayHeight;
        [SerializeField] LayerMask m_obstacleLayer;

        [Header("Ability Setting")]
        [SerializeField] float m_timeFaster;
        [SerializeField] float m_speedIncrease;

        [Header("Animation Setting")]
        [SerializeField] float m_walkLerpTime;

        public AILevel Brain { get; set; }



        private void Start()
        {
            m_data ??= new();
            m_animationBehaviour = new(GetComponent<Animator>(), m_walkLerpTime);
            DamageBehaviour = new(m_respawnDelay, m_respawnPosDis, transform, m_data, m_animationBehaviour);
            m_movementBehaviour = new(transform, m_turnRange, m_turnSpeed, m_data, Brain.Speed, m_timeFaster, m_speedIncrease, m_animationBehaviour, m_countdownMove);
            m_brainBehaviour = new(m_rayLength, m_obstacleLayer, Brain, m_data, transform, DamageBehaviour, m_movementBehaviour, this, m_rayHeight);
        }

        private void Update()
        {
            m_brainBehaviour.Update();

            switch (m_data.State)
            {
                case AIState.IDLE:
                    m_movementBehaviour.Idle();
                    break;
                case AIState.CRASHED:
                    m_movementBehaviour.ForceStopMovement();
                    break;
                case AIState.MOVING:
                    m_movementBehaviour.Move();
                    m_brainBehaviour.ScanBlockObstacle();
                    break;
                case AIState.TURN_DECISING:
                    break;
                case AIState.MOVE_DECISING:
                    m_movementBehaviour.Idle();
                    m_brainBehaviour.SolveMatchBar(transform);
                    m_brainBehaviour.ScanBlockObstacle();
                    break;
                default:
                    break;
            }

        }
        private void OnDrawGizmos()
        {
            m_brainBehaviour.OnDrawGizmos();

        }


        public void OnEndAnimation(AIState state)
        {
            m_animationBehaviour.OnEndAnimation(state);
        }

        public void FinishRace()
        {
            m_data.State = AIState.IDLE;
        }

        public void StartRace()
        {
            m_data.State = AIState.MOVE_DECISING;
        }

        public void WaitStart(Pos currentPos)
        {
            m_data ??= new();
            m_data.State = AIState.IDLE;
            m_data.CurrentPos = currentPos;
        }
    }
}
