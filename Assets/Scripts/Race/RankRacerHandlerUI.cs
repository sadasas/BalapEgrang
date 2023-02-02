using UnityEngine;
using TMPro;

namespace Race
{
    public class RankRacerHandlerUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI m_rankText;

        public void UpdateRank(int rank)
        {
            m_rankText.text = rank.ToString();
        }

    }

}

