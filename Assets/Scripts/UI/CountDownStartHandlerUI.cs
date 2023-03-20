using TMPro;
using UnityEngine;

namespace UI
{
    public class CountDownStartHandlerUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI m_countText;

        public void UpdateCountDown(string text)
        {
            m_countText.text = text;
        }
    }

}
