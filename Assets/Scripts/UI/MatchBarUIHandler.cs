using System.Collections;
using Utility;
using Player;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace BalapEgrang.Player
{

    public class MatchBarUIHandler : MonoBehaviour
    {

        IInputCallback m_inputCallback;
        bool m_isWait = false;
        bool m_isStop;
        PlayerController m_player;

        [SerializeField] float m_increment;
        [SerializeField] Slider m_slider;
        [SerializeField] RectTransform m_perfectHitBox;
        [SerializeField] RectTransform m_goodHitBox;
        [SerializeField] RectTransform m_bar;
        [SerializeField] TextMeshProUGUI m_textNotif;

        void OnEnable()
        {
            StartCoroutine(randomingBar());
        }

        public void SetPlayer(GameObject player)
        {
            m_player = player.GetComponent<PlayerController>();
            m_inputCallback ??= m_player.InputBehaviour;
            m_inputCallback.OnRelease += CheckMatch;


        }


        IEnumerator ShowingText(string text)
        {
            m_textNotif.gameObject.SetActive(true);
            m_textNotif.text = text;

            yield return new WaitForSeconds(1);
            m_textNotif.text = "";
            m_textNotif.gameObject.SetActive(false);

        }
        void CheckMatch()
        {
            if (!m_player.MovementBehaviour.IsMoveAllowed || m_isWait) return;
            m_isWait = true;
            if (RectTransformExtensions.Overlaps(m_bar, m_perfectHitBox))
            {

                m_player.MovementBehaviour.Move(MoveType.PERFECT);
                StartCoroutine(ShowingText(MoveType.PERFECT.ToString()));
            }
            else if (RectTransformExtensions.Overlaps(m_bar, m_goodHitBox))
            {

                m_player.MovementBehaviour.Move(MoveType.GOOD);
                StartCoroutine(ShowingText(MoveType.GOOD.ToString()));
            }
            else
            {
                StartCoroutine(ShowingText("FAIL"));
                m_player.DamageBehaviour.Crash(m_player.transform);
            }

        }

        bool IsRectOverlaps(RectTransform one, RectTransform two)
        {
            Rect rect1 = new(one.localPosition.x, one.localPosition.y, one.rect.width, one.rect.height);
            Rect rect2 = new(two.localPosition.x, two.localPosition.y, two.rect.width, two.rect.height);


            return rect2.Overlaps(rect1, true);
        }

        IEnumerator randomingBar()
        {
            var increment = m_increment;
            while (m_isStop == false)
            {
                m_slider.value += increment * Time.deltaTime;
                if (m_slider.value == 1)
                {
                    m_isWait = false;
                    increment = -m_increment;
                }
                else if (m_slider.value == 0)
                {
                    m_isWait = false;
                    increment = m_increment;
                }
                yield return null;

            }
        }


    }

}
