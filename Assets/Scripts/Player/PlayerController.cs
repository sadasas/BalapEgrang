using Race;
using UnityEngine;

namespace Player
{
    public enum PlayerState
    {
        IDLE,
        WALKING,
        CRASHING,
        TURNING,

    }
    public class PlayerDataState
    {
        public PlayerState State = PlayerState.IDLE;
        public Pos CurrentPost = Pos.CENTER;
    }
    public class PlayerController : MonoBehaviour, IRacer
    {
        PlayerDataState m_dataState;
        public InputBehaviour InputBehaviour;
        public MovementBehaviour MovementBehaviour;
        public AbilityBehaviour AbilityBehaviour;
        PlayerAnimationBehaviour m_animatonBehaviour;
        public DamageBehaviour DamageBehaviour { get; private set; }

        [Header("Input Setting")]
        [SerializeField]
        float m_turnTreshold;
        [SerializeField]
        float m_turnMinLength;

        [Header("Animation Setting")]
        [SerializeField]
        float m_walkLerpTime;

        public string ID { get; set; }
        public PlayerType Type;

        void Start()
        {
            m_dataState ??= new();
            m_animatonBehaviour = new(m_walkLerpTime, this.GetComponent<Animator>(), m_dataState);
            InputBehaviour = new(m_turnTreshold, m_turnMinLength);
            MovementBehaviour = new(transform, InputBehaviour, Type.Speed, Type.TurnSpeed, Type.TurnRange, m_animatonBehaviour, m_dataState, Type.DistanceMove);
            DamageBehaviour = new(transform, Type.RespawnPosDis, MovementBehaviour, m_animatonBehaviour, m_dataState);
            AbilityBehaviour = new(Type.MaxPushVal, this, Type.AbilityTime, MovementBehaviour);
        }

        void Update()
        {

            InputBehaviour.OnUpdate();
            MovementBehaviour.Update();
        }

        public void WaitStart(Pos currentPos)
        {
            m_dataState ??= new();
            MovementBehaviour.IsMoveAllowed = false;
            m_dataState.CurrentPost = currentPos;
            m_dataState.State = PlayerState.IDLE;
        }

        public void StartRace()
        {
            MovementBehaviour.IsMoveAllowed = true;
        }

        public void FinishRace()
        {
            MovementBehaviour.IsMoveAllowed = false;
            m_animatonBehaviour.ForceStopAllAnimation();
        }

        public void OnEndAnimation(PlayerState state)
        {
            m_animatonBehaviour.OnEndAnimCallback(state);
        }
        public void Reposition(Pos repos)
        {
            Transform pos;
            if (repos == Pos.LEFT) pos = RaceManager.s_Instance.StartPos[0];
            else if (repos == Pos.CENTER) pos = RaceManager.s_Instance.StartPos[1];
            else pos = RaceManager.s_Instance.StartPos[2];

            transform.position = new Vector3(pos.position.x, transform.position.y, transform.position.z);
            m_dataState.CurrentPost = repos;
        }

        public void Reposition()
        {
            Transform pos;
            if (m_dataState.CurrentPost == Pos.LEFT) pos = RaceManager.s_Instance.StartPos[0];
            else if (m_dataState.CurrentPost == Pos.CENTER) pos = RaceManager.s_Instance.StartPos[1];
            else pos = RaceManager.s_Instance.StartPos[2];

            transform.position = new Vector3(pos.position.x, transform.position.y, transform.position.z);
        }
    }
}
