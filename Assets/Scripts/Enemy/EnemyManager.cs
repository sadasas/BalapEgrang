﻿using Race;
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
        TURN_DECISING,
        MOVE_DECISING,
        FASTER,

    }
    public class EnemyManager : MonoBehaviour
    {
        const int max_EnemySpawned = 2;
        [SerializeField] List<AIController> m_enemys;
        [SerializeField] GameObject m_playableEnemy;

        private void Start()
        {
            m_enemys = new();
        }
        public void SpawnPlayableEnemy()
        {
            for (int i = 0; i < max_EnemySpawned; i++)
            {
                var parent = GameObject.FindGameObjectWithTag("RacersParent").transform;
                var level = StageManager.s_Instance.StageSelected.Level;
                var guid = $"AI{i}";
                var enemy = Instantiate(m_playableEnemy, parent).GetComponent<AIController>();
                enemy.ID = guid;
                enemy.Brain = level;
                m_enemys.Add(enemy);
                RaceManager.s_Instance.RegisterRacer(guid, enemy.gameObject, false);
            }
        }
    }
}
