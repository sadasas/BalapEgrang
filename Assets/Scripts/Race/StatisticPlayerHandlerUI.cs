using UnityEngine;
using TMPro;
using System;

namespace Race
{
    public class StatisticPlayerHandlerUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI m_rankText;
        [SerializeField] TextMeshProUGUI m_timeText;
        [SerializeField] TextMeshProUGUI m_deadText;

        public void UpdateText(int rank, float second, int dead)
        {
            TimeSpan time = TimeSpan.FromSeconds(second);
            m_rankText.text = rank.ToString();
            m_timeText.text = time.ToString("hh':'mm':'ss");
            m_deadText.text = dead.ToString();
        }


    }

}

