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

       

        m_text.text = "welcome to tutorial";
        m_isTap = false;
        while (!m_isTap)
        {
            m_panel.SetActive(true);
            yield return null;
        }
        m_panel.SetActive(false);
        yield return new WaitForSeconds(1);
         
          var panelRect = m_panel.GetComponent<RectTransform>();
        panelRect.anchorMin = new Vector2(1, 1);
        panelRect.anchorMax = new Vector2(1, 1);
        panelRect.pivot = new Vector2(1, 1);
        panelRect.anchoredPosition = new Vector2(-20, -20);

        m_text.text = "dipojok kanan terdapat timing bar";
        m_isTap = false;
        while (!m_isTap)
        {
            m_panel.SetActive(true);
            yield return null;
        }
        m_panel.SetActive(false);
        yield return new WaitForSeconds(1);

        panelRect.anchorMin = new Vector2(1, 1);
        panelRect.anchorMax = new Vector2(1, 1);
        panelRect.pivot = new Vector2(1, 1);
        panelRect.anchoredPosition = new Vector2(-20, -70);

        m_text.text = "fungsinya adalah untuk menentukan seberapa jauh kamu akan melangkah";
        m_isTap = false;
        while (!m_isTap)
        {
            m_panel.SetActive(true);
            yield return null;
        }
        m_panel.SetActive(false);

         yield return new WaitForSeconds(1);

        panelRect.anchorMin = new Vector2(1, 1);
        panelRect.anchorMax = new Vector2(1, 1);
        panelRect.pivot = new Vector2(1, 1);
        panelRect.anchoredPosition = new Vector2(-20, -70);

        m_text.text = "fungsinya adalah untuk menentukan seberapa jauh kamu akan melangkah";
        m_isTap = false;
        while (!m_isTap)
        {
            m_panel.SetActive(true);
            yield return null;
        }
        m_panel.SetActive(false);
        yield return new WaitForSeconds(1);
        var tap = "pastikan timing bar berada di area merah atau putih agar langkahmu maksimal";


        m_text.text = tap;
        m_isTap = false;
        while (!m_isTap)
        {
            m_panel.SetActive(true);
            yield return null;
        }
        m_panel.SetActive(false);
        yield return new WaitForSeconds(1);

        panelRect.anchorMin = new Vector2(0.5f, 0.5f);
        panelRect.anchorMax = new Vector2(0.5f, 0.5f);
        panelRect.pivot = new Vector2(0.5f, 0.5f);
        panelRect.anchoredPosition = new Vector2(0, 0);

        m_text.text = "tap screen to jalan";;
        m_isTap = false;
        while (!m_isTap)
        {
            m_panel.SetActive(true);
            yield return null;
        }
        m_panel.SetActive(false);
        yield return new WaitForSeconds(1);


        m_panel.SetActive(true);
        var swipe = "swipe kanan atau  kiri to belok";
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
