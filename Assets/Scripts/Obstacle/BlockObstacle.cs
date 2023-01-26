using Player;
using Race;
using UnityEngine;

public class BlockObstacle : MonoBehaviour
{


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var player = other.GetComponent<PlayerController>().DamageBehaviour;
            player.Crash(transform);
            RaceManager.s_Instance.RacerCrashed(other.gameObject);
        }
    }

}
