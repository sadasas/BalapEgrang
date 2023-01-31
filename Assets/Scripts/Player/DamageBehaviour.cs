using UnityEngine;

namespace Player
{
    public class DamageBehaviour : IDamagable
    {
        PlayerDataState m_dataState;
        Transform m_player;
        Transform m_obstacle;
        float m_respawnPosDis;
        MovementBehaviour m_movementBehaviour;
        PlayerAnimationBehaviour m_animationBehaviour;

        public DamageBehaviour(Transform player, float respawnPosDis, MovementBehaviour movementBehaviour, PlayerAnimationBehaviour animationBehaviour, PlayerDataState dataState)
        {
            m_player = player;
            m_respawnPosDis = respawnPosDis;
            m_movementBehaviour = movementBehaviour;
            m_animationBehaviour = animationBehaviour;
            m_dataState = dataState;

            m_animationBehaviour.OnEndAnim += (state) =>
            {
                if (state == PlayerState.CRASHING) Respawn();
            };
        }

        public void Crash(Transform obstacle)
        {
            m_movementBehaviour.IsMoveAllowed = false;
            m_dataState.State = PlayerState.CRASHING;
            m_obstacle = obstacle;
            m_animationBehaviour.Crash();

        }

        public void Respawn()
        {
            var pos = new Vector3(m_player.position.x, m_player.position.y, m_obstacle.position.z + m_respawnPosDis);
            m_player.transform.position = pos;
            m_movementBehaviour.IsMoveAllowed = true;
        }
    }
}
