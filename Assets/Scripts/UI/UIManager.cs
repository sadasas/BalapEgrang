using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public enum HUDType
    {
        COUNTDOWN_START,
        STATISTIC_PLAYER_RACE_FINISHED,
        RANK_RACER,
        SETTING,
        CREDIT,
        NEW_CHARACTER,
        LOADING_SCENE,
        TUTORIAL,
        RANDOM_BAR,
    }
    public class UIManager : MonoBehaviour
    {
        public static UIManager s_Instance;

        Transform m_mainCanvas;
        Dictionary<HUDType, GameObject> m_HUDs;

        [Header("Race HUD")]
        [SerializeField] GameObject m_countDownStartHUD;
        [SerializeField] GameObject m_statisticPlayerFinishRaceHUD;
        [SerializeField] GameObject m_rankRacerHUD;
        [SerializeField] GameObject m_newCharacterUIHUD;
        [SerializeField] GameObject m_loadSceneHUD;
        [SerializeField] GameObject m_tutorialHUD;
        [SerializeField] GameObject m_randomBarHUD;




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
            if (!m_HUDs.ContainsKey(type)) return null;
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
                case "Tutorial":
                    InitTutorialHUD();
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

            //countdown start hud 
            var csHUD = Instantiate(m_countDownStartHUD, m_mainCanvas);
            csHUD.SetActive(false);
            if (m_HUDs.ContainsKey(HUDType.COUNTDOWN_START)) m_HUDs[HUDType.COUNTDOWN_START] = csHUD;
            else
                m_HUDs.Add(HUDType.COUNTDOWN_START, csHUD);

            //statistic player hud 
            var sprfHUD = Instantiate(m_statisticPlayerFinishRaceHUD, m_mainCanvas);
            sprfHUD.SetActive(false);
            if (m_HUDs.ContainsKey(HUDType.STATISTIC_PLAYER_RACE_FINISHED)) m_HUDs[HUDType.STATISTIC_PLAYER_RACE_FINISHED] = sprfHUD;
            else
                m_HUDs.Add(HUDType.STATISTIC_PLAYER_RACE_FINISHED, sprfHUD);

            //rank racer hud
            var rrHUD = Instantiate(m_rankRacerHUD, m_mainCanvas);
            rrHUD.SetActive(false);
            if (m_HUDs.ContainsKey(HUDType.RANK_RACER)) m_HUDs[HUDType.RANK_RACER] = rrHUD;
            else
                m_HUDs.Add(HUDType.RANK_RACER, rrHUD);

            //random bar hud
            var rbHUD = Instantiate(m_randomBarHUD, m_mainCanvas);
            rbHUD.SetActive(false);
            if (m_HUDs.ContainsKey(HUDType.RANDOM_BAR)) m_HUDs[HUDType.RANDOM_BAR] = rbHUD;
            else
                m_HUDs.Add(HUDType.RANDOM_BAR, rbHUD);


        }

        void InitTutorialHUD()
        {
            m_HUDs ??= new();
            var tHUD = Instantiate(m_tutorialHUD, m_mainCanvas);
            m_HUDs.Add(HUDType.TUTORIAL, tHUD);

        }
    }

}
