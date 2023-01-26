using Race;
using UnityEngine;

namespace Player
{

    public class PlayerController : MonoBehaviour, IRacer
    {
        public InputBehaviour InputBehaviour;
        public MovementBehaviour MovementBehaviour;
        public AbilityBehaviour AbilityBehaviour;
        public DamageBehaviour DamageBehaviour { get; private set; }

        [Header("Input Setting")]
        [SerializeField]
        float m_turnTreshold;
        [SerializeField]
        float m_turnMinLength;

        [Header("Movement Setting")]
        [SerializeField]
        float m_speed;
        [SerializeField]
        float m_turnSpeed;
        [SerializeField]
        float m_turnRange;

        [Header("Damage Setting")]
        [SerializeField]
        float m_respawnPosDis;

        [Header("Ability Setting")]
        [SerializeField]
        int m_maxPushVal;
        [SerializeField]
        int m_abilityTime;
        void Start()
        {
            InputBehaviour = new(m_turnTreshold, m_turnMinLength);
            MovementBehaviour = new(transform, InputBehaviour, m_speed, m_turnSpeed, m_turnRange);
            DamageBehaviour = new(transform, m_respawnPosDis, MovementBehaviour);
            AbilityBehaviour = new(m_maxPushVal,this, m_abilityTime, MovementBehaviour);
        }

        void Update()
        {
            InputBehaviour.OnUpdate();
        }

        public void WaitStart(Pos currentPos)
        {
            MovementBehaviour.IsPlaying = false;
            MovementBehaviour.CurrentPost = currentPos;
        }

        public void StartRace()
        {
            MovementBehaviour.IsPlaying = true;
        }

        public void FinishRace()
        {
            MovementBehaviour.IsPlaying = false;
            Debug.Log("finis");
        }
    }
}
