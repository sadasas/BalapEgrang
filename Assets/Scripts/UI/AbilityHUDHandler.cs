using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class AbilityHUDHandler:MonoBehaviour
    {
        [SerializeField] Slider m_slider;

        public void UpdateSlider(float val,int maxValue)
        {
            m_slider.maxValue = maxValue;
            m_slider.value = val;
        }

        public void UpdateSlider(float val)
        {
           
            m_slider.value = val;
        }
    }

}