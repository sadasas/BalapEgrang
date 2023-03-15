using Enemy;
using Player;
using Race;
using UnityEngine;

public class BlockObstacle : MonoBehaviour
{

    /// <summary>
    /// TODO:
    /// calculating what happen when crashing in here
    /// not depend object
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerEnter(Collider other)
    {
        if (RaceManager.s_Instance == null || RaceManager.s_Instance.s_State != RaceState.PLAYING) return;
        if (other.CompareTag("Player"))
        {
            var player = other.GetComponent<PlayerController>();
            player.DamageBehaviour.Crash(transform);

            RaceManager.s_Instance.RacerCrashed(player.GetComponent<IRacer>());
        }
        else if (other.CompareTag("Enemy"))
        {
            var enemy = other.transform.GetComponent<AIController>();
            enemy.DamageBehaviour.Crash(gameObject);
            RaceManager.s_Instance.RacerCrashed(enemy.GetComponent<IRacer>());
        }
    }

}
