using Player;
using Race;
using UnityEngine;
using Enemy;

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
        if (other.CompareTag("Player"))
        {
            var player = other.GetComponent<PlayerController>().DamageBehaviour;
            player.Crash(transform);
            RaceManager.s_Instance.RacerCrashed(other.gameObject);
        }
        else if(other.CompareTag("Enemy"))
        {
            var enemy = other.transform.GetComponent<EnemyController>();
            enemy.Crashed(gameObject);
            RaceManager.s_Instance.RacerCrashed(other.gameObject);
        }
    }

}
