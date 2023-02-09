using System.Collections.Generic;
using UnityEngine;

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
        NEW_CHARACTER
    }
    public class UIManager : MonoBehaviour
    {
        public static UIManager s_this;

        Transform m_mainCanvas;
        Dictionary<HUDType, GameObject> m_HUDs;

        [Header("Race HUD")]
        [SerializeField] GameObject m_CTEHUD;
        [SerializeField] GameObject m_abilityHUD;
        [SerializeField] GameObject m_countDownStartHUD;
        [SerializeField] GameObject m_statisticPlayerFinishRaceHUD;
        [SerializeField] GameObject m_rankRacerHUD;
        [SerializeField] GameObject m_newCharacterUIHUD;




        void Awake()
        {
            if (s_this != null) Destroy(s_this.gameObject);
            s_this = this;

        }

        void Start()
        {
            m_mainCanvas = GameObject.FindWithTag("MainCanvas").transform;
            InitRaceHUD();
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


        void InitRaceHUD()
        {

            m_HUDs ??= new();
            //cte hud
            var cteHUD = Instantiate(m_CTEHUD, m_mainCanvas);
            cteHUD.SetActive(false);
            m_HUDs.Add(HUDType.CTE, cteHUD);

            //ability hud
            var abHUD = Instantiate(m_abilityHUD, m_mainCanvas);
            abHUD.SetActive(false);
            m_HUDs.Add(HUDType.ABILITY, abHUD);

            var csHUD = Instantiate(m_countDownStartHUD, m_mainCanvas);
            csHUD.SetActive(false);
            m_HUDs.Add(HUDType.COUNTDOWN_START, csHUD);

            var sprfHUD = Instantiate(m_statisticPlayerFinishRaceHUD, m_mainCanvas);
            sprfHUD.SetActive(false);
            m_HUDs.Add(HUDType.STATISTIC_PLAYER_RACE_FINISHED, sprfHUD);

            var rrHUD = Instantiate(m_rankRacerHUD, m_mainCanvas);
            rrHUD.SetActive(false);
            m_HUDs.Add(HUDType.RANK_RACER, rrHUD);

            var ncHUD = Instantiate(m_newCharacterUIHUD, m_mainCanvas);
            ncHUD.SetActive(false);
            m_HUDs.Add(HUDType.NEW_CHARACTER, ncHUD);
        }

    }

}
