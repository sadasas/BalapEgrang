using System.Collections;
using UI;
using UnityEngine;
namespace Player
{
    public class AbilityBehaviour
    {
        AbilityHUDHandler m_hudHandler;
        MovementBehaviour m_movementBehaviour;
        float m_time;
        float m_countDown;
        int m_push;
        MonoBehaviour m_player;
        Coroutine m_coroutine;


        public AbilityBehaviour(int push, MonoBehaviour player, float time, MovementBehaviour movementBehaviour)
        {
            m_movementBehaviour = movementBehaviour;
            m_push = push;
            m_player = player;
            m_time = time;

            m_hudHandler = UIManager.s_Instance.GetHUD(HUDType.ABILITY).GetComponent<AbilityHUDHandler>();
        }
        public void IncreaseSpeed()
        {
            if (m_coroutine != null) m_countDown = m_time;
            else
                m_coroutine = m_player.StartCoroutine(Faster());
        }



        IEnumerator Faster()
        {
            m_hudHandler.gameObject.SetActive(true);
            m_hudHandler.UpdateSlider(m_time, (int)m_time);
            m_countDown = m_time;

            m_movementBehaviour.IncreaseSpeed(m_push);
            while (m_countDown > 0f)
            {

                m_countDown -= Time.deltaTime;
                m_hudHandler.UpdateSlider(m_countDown);
                yield return null;
            }
            m_coroutine = null;
            m_movementBehaviour.DecreaseSpeed(m_push);
            m_hudHandler.gameObject.SetActive(false);
        }


    }
}
