using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public enum HUDType
    {
        CTE,
        ABILITY,
        COUNTDOWN_START,
        STATISTIC_PLAYER_RACE_FINISHED,
        RANK_RACER,
        SETTING,
        CREDIT,
        NEW_CHARACTER,
        LOADING_SCENE
    }
    public class UIManager : MonoBehaviour
    {
        public static UIManager s_Instance;

        Transform m_mainCanvas;
        Dictionary<HUDType, GameObject> m_HUDs;

        [Header("Race HUD")]
        [SerializeField] GameObject m_CTEHUD;
        [SerializeField] GameObject m_abilityHUD;
        [SerializeField] GameObject m_countDownStartHUD;
        [SerializeField] GameObject m_statisticPlayerFinishRaceHUD;
        [SerializeField] GameObject m_rankRacerHUD;
        [SerializeField] GameObject m_newCharacterUIHUD;
        [SerializeField] GameObject m_loadSceneHUD;




        void Awake()
        {
            if (s_Instance != null) Destroy(gameObject);
            s_Instance = this;
            DontDestroyOnLoad(gameObject);

        }

        void Start()
        {
            InitLoadingSceneHUD();
            SceneManager.sceneLoaded += OnSceneLoaded;
        }


        public void ForceHUD(HUDType hud)
        {
            m_HUDs[hud].SetActive(true);

        }

        public GameObject GetHUD(HUDType type)
        {
            return m_HUDs[type];
        }
        public void DisableHUD(HUDType hud)
        {
            m_HUDs[hud].SetActive(false);
        }

        public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            m_mainCanvas = GameObject.FindWithTag("MainCanvas").transform;

            switch (scene.name)
            {
                case "Stage1":
                case "Stage2":
                    InitRaceHUD();
                    break;
                case "ChooseStage":
                    InitStageHUD();
                    break;

                default:
                    break;
            }
        }


        void InitLoadingSceneHUD()
        {
            m_HUDs ??= new();
            m_HUDs[HUDType.LOADING_SCENE] = Instantiate(m_loadSceneHUD, transform);

        }

        void InitStageHUD()
        {
            m_HUDs ??= new();
            var ncHUD = Instantiate(m_newCharacterUIHUD, m_mainCanvas);
            ncHUD.SetActive(false);
            if (m_HUDs.ContainsKey(HUDType.NEW_CHARACTER)) m_HUDs[HUDType.NEW_CHARACTER] = ncHUD;
            else

                m_HUDs.Add(HUDType.NEW_CHARACTER, ncHUD);

        }

        void InitRaceHUD()
        {

            m_HUDs ??= new();
            //cte hud
            var cteHUD = Instantiate(m_CTEHUD, m_mainCanvas);
            cteHUD.SetActive(false);
            if (m_HUDs.ContainsKey(HUDType.CTE)) m_HUDs[HUDType.CTE] = cteHUD;
            else
                m_HUDs.Add(HUDType.CTE, cteHUD);

            //ability hud
            var abHUD = Instantiate(m_abilityHUD, m_mainCanvas);
            abHUD.SetActive(false);
            if (m_HUDs.ContainsKey(HUDType.ABILITY)) m_HUDs[HUDType.ABILITY] = abHUD;
            else
                m_HUDs.Add(HUDType.ABILITY, abHUD);

            var csHUD = Instantiate(m_countDownStartHUD, m_mainCanvas);
            csHUD.SetActive(false);
            if (m_HUDs.ContainsKey(HUDType.COUNTDOWN_START)) m_HUDs[HUDType.COUNTDOWN_START] = csHUD;
            else
                m_HUDs.Add(HUDType.COUNTDOWN_START, csHUD);

            var sprfHUD = Instantiate(m_statisticPlayerFinishRaceHUD, m_mainCanvas);
            sprfHUD.SetActive(false);
            if (m_HUDs.ContainsKey(HUDType.STATISTIC_PLAYER_RACE_FINISHED)) m_HUDs[HUDType.STATISTIC_PLAYER_RACE_FINISHED] = sprfHUD;
            else
                m_HUDs.Add(HUDType.STATISTIC_PLAYER_RACE_FINISHED, sprfHUD);

            var rrHUD = Instantiate(m_rankRacerHUD, m_mainCanvas);
            rrHUD.SetActive(false);
            if (m_HUDs.ContainsKey(HUDType.RANK_RACER)) m_HUDs[HUDType.RANK_RACER] = rrHUD;
            else
                m_HUDs.Add(HUDType.RANK_RACER, rrHUD);

        }

    }

}
