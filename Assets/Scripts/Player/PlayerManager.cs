using Race;
using System.Collections;
using UnityEngine;

namespace Player
{
    public class PlayerManager : MonoBehaviour
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

            var guid = "PLAYER";

            m_playerController.ID = guid;
            yield return null;
            RaceManager.s_Instance.RegisterRacer(guid, m_playerController.gameObject, true);

            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraController>().Player = m_playerController.transform;
        }
    }
}
