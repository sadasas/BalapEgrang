using Enemy;
using Player;
using Race;
using UnityEngine;
using System.Collections.Generic;

namespace Obstacle
{
    public class MoveBlockObstacle : MonoBehaviour
    {
        Vector3 m_startPoin;
        Vector3 m_endPoin;
        List<GameObject> m_objTrapped;

        [SerializeField] float m_speed;

        void Start()
        {
            m_startPoin = RaceManager.s_Instance.StartPos[0].position;
            m_endPoin = RaceManager.s_Instance.StartPos[2].position;

            m_objTrapped = new();


        }


        void Update()
        {
            if (transform.position.x >= m_endPoin.x)
            {
                if (m_speed > 0)
                    m_speed *= -1;
            }
            else if (transform.position.x <= m_startPoin.x)
            {
                if (m_speed < 0)
                    m_speed *= -1;
            }
            transform.Translate((Vector3.right * m_speed) * Time.deltaTime, Space.World);
        }
        void OnTriggerEnter(Collider other)
        {

            if (RaceManager.s_Instance.s_State != RaceState.PLAYING) return;
            if (m_objTrapped.Count > 0)
            {
                foreach (var item in m_objTrapped)
                {
                    if (item == other.gameObject) return;
                }
            }

            if (other.CompareTag("Player"))
            {
                var player = other.GetComponent<PlayerController>();
                player.DamageBehaviour.Crash(transform);

                RaceManager.s_Instance.RacerCrashed(player.GetComponent<IRacer>());
                m_objTrapped.Add(other.gameObject);
            }
            else if (other.CompareTag("Enemy"))
            {
                var enemy = other.transform.GetComponent<AIController>();
                enemy.DamageBehaviour.Crash(gameObject);
                RaceManager.s_Instance.RacerCrashed(enemy.GetComponent<IRacer>());
                m_objTrapped.Add(other.gameObject);
            }
        }

    }

}

