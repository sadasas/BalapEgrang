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
            if (m_dataPlayer.CharacterCollections == null || m_dataPlayer.CharacterCollections.Count == 0) AddDefaultCharacter();

        }

        void AddDefaultCharacter()
        {
            m_dataPlayer.CharacterCollections = new();
            var defaultCharacter = Helper.GetPlayerType("Student");
            m_dataPlayer.CharacterCollections.Add(defaultCharacter);

            SaveDataPlayer();

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
                var isNew = true;
                foreach (var item in m_dataPlayer.StageRecords.ToArray())
                {
                    if (item.StageIndex == newRecord.StageIndex)
                    {
                        isNew = false;
                        m_dataPlayer.StageRecords.Remove(item);
                        m_dataPlayer.StageRecords.Add(newRecord);
                        break;
                    }
                }
                if (isNew) m_dataPlayer.StageRecords.Add(newRecord);
            }

            SaveDataPlayer();
        }

        public void UpdateCurrentSelection(PlayerType selection)
        {

            m_dataPlayer.CurrentCharacterSelection = selection.Name;
            SaveDataPlayer();
        }

        public void AddNewCharacter(PlayerType newCharacter)
        {
            m_dataPlayer.CharacterCollections.Add(newCharacter);
            SaveDataPlayer();
        }

        public void AddReward(SceneType type)
        {
            m_dataPlayer.RewardUnCollecteds ??= new();
            m_dataPlayer.RewardUnCollecteds.Add(type);

            SaveDataPlayer();
        }

        public void CollectReward(SceneType type)
        {
            m_dataPlayer.RewardCollecteds ??= new();
            m_dataPlayer.RewardCollecteds.Add(type);
            m_dataPlayer.RewardUnCollecteds.Remove(type);

            SaveDataPlayer();
        }
        #endregion

        #region RACE API

        IEnumerator SetupPlayablePlayerForRace()
        {
            var guid = "PLAYER";

            m_playerController = SpawnPlayablePlayer();

            m_playerController.ID = guid;
            yield return null;
            RaceManager.s_Instance.RegisterRacer(guid, m_playerController.gameObject, true);
        }
        public void SetupPlayerForRace()
        {
            StartCoroutine(SetupPlayablePlayerForRace());
        }

        public PlayerController SpawnPlayablePlayer()
        {
            var parent = GameObject.FindGameObjectWithTag("RacersParent").transform;
            var pc = Instantiate(m_playablePlayerPrefab, parent).GetComponent<PlayerController>();
            pc.Type = Helper.GetPlayerType(m_dataPlayer.CurrentCharacterSelection);

            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraController>().Player = pc.transform;
            return pc;
        }
        #endregion

    }
}
