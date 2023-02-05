using UnityEngine;
using TMPro;
using Utility;

namespace Race
{
    public class StatisticPlayerHandlerUI : MonoBehaviour
    {
        [SerializeField] GameObject[] m_stars;
        [SerializeField] TextMeshProUGUI m_rankText;
        [SerializeField] TextMeshProUGUI m_timeText;
        [SerializeField] TextMeshProUGUI m_deadText;
        [SerializeField] GameObject m_nextBtn;


        void Start()
        {

            if (StageManager.s_Instance.CheckNextStage()) m_nextBtn.SetActive(true);
            else m_nextBtn.SetActive(false);
        }
        public void UpdateText(int star, int rank, float second, int dead)
        {
            for (int i = 0; i < star; i++)
            {
                m_stars[i].SetActive(true);

            }
            m_rankText.text = rank.ToString();
            m_timeText.text = Helper.FloatToTimeSpan(second);
            m_deadText.text = dead.ToString();
        }

        public void NextStage()
        {
            StageManager.s_Instance.NextStage();

        }

        public void Home()
        {
            GameManager.s_Instance.LoadScene(SceneType.MAIN_MENU);
        }

    }

}

