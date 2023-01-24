using UnityEngine;

namespace Player
{

    public interface IDamagable
    {
        public void Crash(Transform obstacle);
    }

    public class DamageBehaviour : IDamagable
    {
        Transform m_player;
        float m_respawnPosDis;
        MovementBehaviour m_movementBehaviour;

        public DamageBehaviour(Transform player, float respawnPosDis, MovementBehaviour movementBehaviour)
        {
            m_player = player;
            m_respawnPosDis = respawnPosDis;
            m_movementBehaviour = movementBehaviour;
        }

        public void Crash(Transform obstacle)
        {
            var pos = new Vector3(m_player.position.x, m_player.position.y, m_player.position.z * m_respawnPosDis);
            m_player.transform.position = pos;
            m_movementBehaviour.IsPlaying = true;

        }
    }

    public class PlayerController : MonoBehaviour
    {
        public InputBehaviour InputBehaviour;
        public MovementBehaviour MovementBehaviour;
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

        void Start()
        {
            InputBehaviour = new(m_turnTreshold, m_turnMinLength);
            MovementBehaviour = new(transform, InputBehaviour, m_speed, m_turnSpeed, m_turnRange);
            DamageBehaviour = new(transform, m_respawnPosDis, MovementBehaviour);
        }

        void Update()
        {
            InputBehaviour.OnUpdate();
        }
    }
}
