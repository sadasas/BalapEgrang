using Player;
using UI;
using UnityEngine;
using System.Collections;
using TMPro;

public class TutorialManager : MonoBehaviour
{

    PlayerController m_playerController;
    bool m_isTap = false;
    bool m_isSwiped = false;

    TextMeshProUGUI m_text;
    GameObject m_panel;

    void Start()
    {
        m_panel = UIManager.s_Instance.GetHUD(HUDType.TUTORIAL);
        m_text = m_panel.transform.GetChild(1).GetComponent<TextMeshProUGUI>();

        m_playerController = PlayerManager.s_Instance.SpawnPlayablePlayer();


        StartCoroutine(PlayingTutorial());
    }

    IEnumerator PlayingTutorial()
    {
        yield return null;
        m_playerController.InputBehaviour.OnTap += () => m_isTap = true;
        m_playerController.InputBehaviour.OnSwipe += (pos) => m_isSwiped = true;

        var tap = "tap screen to move";

        m_text.text = tap;
        m_isTap = false;
        while (!m_isTap)
        {
            m_panel.SetActive(true);
            yield return null;
        }
        m_panel.SetActive(false);
        yield return new WaitForSeconds(1);

        m_panel.SetActive(true);
        var swipe = "swipe left or rigth to turn";
        m_text.text = swipe;
        m_isSwiped = false;
        while (!m_isSwiped)
        {
            yield return null;
        }

        m_panel.SetActive(false);
        PlayerPrefs.SetInt("Tutorial", 1);
        yield return new WaitForSeconds(2);
        GameManager.s_Instance.LoadScene(SceneType.STAGE_1);

    }

}
