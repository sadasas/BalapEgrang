using Player;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Obstacle
{
    public class CTEHandler : MonoBehaviour
    {
        IInputCallback m_inputCallback;
        bool m_isStop = false;

        [SerializeField] Slider m_slider;
        [SerializeField] float m_time;
        [SerializeField] float m_increment;
        [SerializeField] RectTransform m_hitBox;
        [SerializeField] RectTransform m_bar;

        public GameObject Obstacle { get; set; }
        public GameObject Player { get; set; }

        void Start()
        {

            m_inputCallback ??= GameObject.FindWithTag("Player").GetComponent<PlayerController>().InputBehaviour;
            m_inputCallback.OnTap += CheckMatch;
        }

        void OnEnable()
        {
            m_isStop = false;

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
                Player.GetComponent<PlayerController>().MovementBehaviour.IsPlaying = true;
            }
            else
            {
                Player.GetComponent<PlayerController>().DamageBehaviour.Crash(Obstacle.transform);

            }

            gameObject.SetActive(false);
        }

        IEnumerator randomingBar()
        {
            Player.GetComponent<PlayerController>().MovementBehaviour.IsPlaying = false;
            var countDown = m_time;
            var increment = m_increment;
            while (countDown > 0f && m_isStop == false)
            {
                countDown -= Time.deltaTime;
                m_slider.value += increment;
                if (m_slider.value == 1) increment = -m_increment;
                else if (m_slider.value == 0) increment = m_increment;
                yield return null;

            }
            Player.GetComponent<PlayerController>().DamageBehaviour.Crash(Obstacle.transform);
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

