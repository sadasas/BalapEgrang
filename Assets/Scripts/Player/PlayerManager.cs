using Race;
using System.Collections;
using UnityEngine;
using Utility;

namespace Player
{
    public class PlayerManager : MonoBehaviour
    {

        public static PlayerManager s_Instance;

        public IO m_ioHandler;
        PlayerData m_dataPlayer;
        PlayerController m_playerController;

        [SerializeField] GameObject m_playablePlayerPrefab;

        public PlayerData DataPlayer { get => m_dataPlayer; }


        void Awake()
        {
            if (s_Instance != null) Destroy(gameObject);
            s_Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        void Start()
        {
            m_ioHandler = new();
            LoadData();

        }



        #region DATA PLAYER API 
        void LoadData()
        {
            m_dataPlayer = m_ioHandler.LoadData();

        }

        void SaveDataPlayer()
        {
            m_ioHandler.SaveData(m_dataPlayer);
        }

        public StageData GetStageRecord(SceneType index)
        {
            if (m_dataPlayer.StageRecords == null) return default;
            foreach (var item in m_dataPlayer.StageRecords)
            {
                if (item.StageIndex == index) return item;
            }
            return default;
        }

        public void UpdateStageRecord(StageData newRecord)
        {

            if (m_dataPlayer.StageRecords == null)
            {
                m_dataPlayer.StageRecords = new();
                m_dataPlayer.StageRecords.Add(newRecord);
            }
            else
            {
                foreach (var item in m_dataPlayer.StageRecords.ToArray())
                {
                    if (item.StageIndex == newRecord.StageIndex)
                    {
                        m_dataPlayer.StageRecords.Remove(item);
                    }

                    m_dataPlayer.StageRecords.Add(newRecord);
                }
            }

            SaveDataPlayer();
        }

        public void UpdateCurrentSelection(PlayerType selection)
        {

            m_dataPlayer.CurrentCharacterSelection = selection.Name;
            SaveDataPlayer();
        }
        #endregion

        #region RACE API
        public void SpawnPlayablePlayer()
        {
            StartCoroutine(SpawningPlayablePlayer());
        }
        IEnumerator SpawningPlayablePlayer()
        {

            var parent = GameObject.FindGameObjectWithTag("RacersParent").transform;
            m_playerController = Instantiate(m_playablePlayerPrefab, parent).GetComponent<PlayerController>();

            var guid = "PLAYER";
            m_playerController.Type = Helper.GetPlayerType(m_dataPlayer.CurrentCharacterSelection);

            m_playerController.ID = guid;
            yield return null;
            RaceManager.s_Instance.RegisterRacer(guid, m_playerController.gameObject, true);

            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraController>().Player = m_playerController.transform;
        }
        #endregion

    }
}
