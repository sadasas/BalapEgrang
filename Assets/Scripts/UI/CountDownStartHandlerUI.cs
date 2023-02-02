using TMPro;
using UnityEngine;

namespace UI
{
    public class CountDownStartHandlerUI:MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI m_countText;

        public void UpdateCountDown(int count)
        {
            m_countText.text = count.ToString();
        }
    }

}