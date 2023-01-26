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
        int m_push;
        MonoBehaviour m_player;


        public AbilityBehaviour(int push, MonoBehaviour player, float time, MovementBehaviour movementBehaviour)
        {
            m_movementBehaviour = movementBehaviour;
            m_push = push;
            m_player = player;
            m_time = time;

            m_hudHandler = UIManager.s_this.GetHUD(HUDType.ABILITY).GetComponent<AbilityHUDHandler>();
        }
        public void IncreaseSpeed()
        {

            m_player.StartCoroutine(Faster());
        }



        IEnumerator Faster()
        {
            m_hudHandler.gameObject.SetActive(true);
            m_hudHandler.UpdateSlider(m_time, (int)m_time);
            var countDown = m_time;

            m_movementBehaviour.IncreaseSpeed(m_push);
            while (countDown > 0f)
            {
               
                countDown -= Time.deltaTime;
                m_hudHandler.UpdateSlider(countDown);
                yield return null;
            }
            m_movementBehaviour.DecreaseSpeed(m_push);
            m_hudHandler.gameObject.SetActive(false);
        }


    }
}
