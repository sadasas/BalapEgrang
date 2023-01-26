using Race;
using System.Collections;
using UnityEngine;

namespace Player
{
    public class PlayerManager :MonoBehaviour
    {
        [SerializeField] GameObject m_playablePlayerPrefab;

        PlayerController m_playerController;

        public void SpawnPlayablePlayer()
        {
            StartCoroutine(SpawningPlayablePlayer());
        }
        IEnumerator SpawningPlayablePlayer()
        {
            var parent = GameObject.FindGameObjectWithTag("RacersParent").transform;
            m_playerController = Instantiate(m_playablePlayerPrefab, parent).GetComponent<PlayerController>();
            yield return null;
            RaceManager.s_Instance.RegisterRacer(m_playerController.gameObject);

            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraController>().Player = m_playerController.transform;
        }
    }
}
