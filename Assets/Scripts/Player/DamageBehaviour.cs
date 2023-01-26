using UnityEngine;

namespace Player
{
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
}
