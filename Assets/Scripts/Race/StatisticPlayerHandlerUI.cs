using UnityEngine;
using TMPro;
using Utility;

namespace Race
{
    public class StatisticPlayerHandlerUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI m_rankText;
        [SerializeField] TextMeshProUGUI m_timeText;
        [SerializeField] TextMeshProUGUI m_deadText;

        public void UpdateText(int rank, float second, int dead)
        {

            m_rankText.text = rank.ToString();
            m_timeText.text = Helper.FloatToTimeSpan(second);
            m_deadText.text = dead.ToString();
        }


    }

}

