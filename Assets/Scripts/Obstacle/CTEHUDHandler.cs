using Player;
using Race;
using BalapEgrang.Sound;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Obstacle
{
    public class CTEHUDHandler : MonoBehaviour
    {
        IInputCallback m_inputCallback;
        bool m_isStop = false;
        GameObject m_player;
        float m_increment;
        float m_time;

        [SerializeField] Slider m_slider;
        [SerializeField] Slider m_timeSlider;
        [SerializeField] RectTransform m_hitBox;
        [SerializeField] RectTransform m_bar;

        public GameObject Obstacle { get; set; }

        void OnEnable()
        {
            m_isStop = false;
            var data = StageManager.s_Instance.StageSelected;
            m_time = data.TimeCTEObstacle;
            m_increment = data.IncrementCTEObstacle;

        }
        private void OnDisable()
        {
            if (m_inputCallback != null) m_inputCallback.OnTap -= CheckMatch;
        }

        public void SetPlayer(GameObject player)
        {
            m_player = player;
            m_inputCallback ??= m_player.GetComponent<PlayerController>().InputBehaviour;
            m_inputCallback.OnTap += CheckMatch;
        }
        public void PlayCTE()
        {
            StartCoroutine(randomingBar());
        }

        void CheckMatch()
        {
            if (m_isStop) return;
            m_isStop = true;
            if (IsRectOverlaps(m_bar, m_hitBox))
            {
                m_player.GetComponent<PlayerController>().MovementBehaviour.IsMoveAllowed = true;
                m_player.GetComponent<PlayerController>().AbilityBehaviour.IncreaseSpeed();
                SoundManager.s_Instance.PlaySFX(SFXType.POWER_UP);

            }
            else
            {
                FailMatching();
                SoundManager.s_Instance.PlaySFX(SFXType.PLAYER_FALL);

            }

            gameObject.SetActive(false);
        }

        void FailMatching()
        {
            m_player.GetComponent<PlayerController>().DamageBehaviour.Crash(Obstacle.transform);
            RaceManager.s_Instance.RacerCrashed(m_player.GetComponent<IRacer>());
        }
        IEnumerator randomingBar()
        {
            m_player.GetComponent<PlayerController>().MovementBehaviour.IsMoveAllowed = false;
            var countDown = m_time;
            var increment = m_increment;
            m_timeSlider.maxValue = m_time;
            while (countDown > 0f && m_isStop == false)
            {
                countDown -= Time.deltaTime;
                m_timeSlider.value = countDown;
                m_slider.value += increment * Time.deltaTime;
                if (m_slider.value == 1) increment = -m_increment;
                else if (m_slider.value == 0) increment = m_increment;
                yield return null;

            }
            FailMatching();
            gameObject.SetActive(false);
        }

        bool IsRectOverlaps(RectTransform one, RectTransform two)
        {
            Rect rect1 = new(one.localPosition.x, one.localPosition.y, one.rect.width, one.rect.height);
            Rect rect2 = new(two.localPosition.x, two.localPosition.y, two.rect.width, two.rect.height);

            return rect1.Overlaps(rect2);
        }

    }

}

