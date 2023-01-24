using Player;
using UnityEngine;

public class BlockObstacle : MonoBehaviour
{


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var player = other.GetComponent<PlayerController>().DamageBehaviour;
            player.Crash(transform);
        }
    }

}
