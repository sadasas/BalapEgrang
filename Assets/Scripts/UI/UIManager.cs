using System.Collections.Generic;
using UnityEngine;

namespace UI
{

    public enum HUDType
    {
        CTE,
        ABILITY
    }
    public class UIManager : MonoBehaviour
    {
        public static UIManager s_this;

        Transform m_mainCanvas;
        Dictionary<HUDType, GameObject> m_HUDs;
        [SerializeField] GameObject m_CTEhud;
        [SerializeField] GameObject m_abilityhud;


        void Awake()
        {
            if (s_this != null) Destroy(s_this.gameObject);
            s_this = this;

        }

        void Start()
        {
            m_mainCanvas = GameObject.FindWithTag("MainCanvas").transform;
            InitHUD();
        }

        public void ForceHUD(HUDType hud)
        {
            m_HUDs[hud].SetActive(true);

        }

        public GameObject GetHUD(HUDType type)
        {
            return m_HUDs[type];
        }

        void InitHUD()
        {
            m_HUDs = new();

            //cte hud
            var cteHUD = Instantiate(m_CTEhud, m_mainCanvas);
            cteHUD.SetActive(false);
            m_HUDs.Add(HUDType.CTE, cteHUD);

            //ability hud
            var abHUD = Instantiate(m_abilityhud, m_mainCanvas);
            abHUD.SetActive(false);
            m_HUDs.Add(HUDType.ABILITY, abHUD);
        }
    }

}