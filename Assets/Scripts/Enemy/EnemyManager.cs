using Race;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{

    public enum AIState
    {
        IDLE,
        MOVING,
        TURNING,
        CRASHED,
        DECISING,
        FASTER,

    }
    public class EnemyManager : MonoBehaviour
    {
        const int max_EnemySpawned = 2;
        List<AIController> m_enemys;
        [SerializeField] GameObject m_playableEnemy;

        private void Start()
        {
            m_enemys = new();
        }
        public void SpawnPlayableEnemy()
        {
            for (int i = 0; i < max_EnemySpawned; i++)
            {
                var enemy = Instantiate(m_playableEnemy).GetComponent<AIController>();
                m_enemys.Add(enemy);
                RaceManager.s_Instance.RegisterRacer(enemy.gameObject);
            }
        }
    }
}