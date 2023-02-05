using UnityEngine;
using System.Collections.Generic;
using TMPro;
using Utility;

public class ChooseStageHandlerUI : MonoBehaviour
{
    int m_currentSelection = 0;
    GameObject m_currentObj;
    GameObject[] m_stageObjSelection;
    Stage[] m_stages;


    [SerializeField] Transform m_posObj;
    [SerializeField] Vector3 m_nextObjOffset;

    //record panel
    [SerializeField] GameObject m_record;
    [SerializeField] GameObject[] m_stars;
    [SerializeField] TextMeshProUGUI m_timeText;
    [SerializeField] TextMeshProUGUI m_deadText;

    [SerializeField] TextMeshProUGUI m_nameStageText;

    void Start()
    {
        m_stages = StageManager.s_Instance.GetStages();
        InitStages();
        ShowCurrentSelection();
    }

    public void NextStage()
    {
        if (m_currentSelection + 1 < m_stages.Length)
        {
            m_currentSelection++;
        }
        else
        {
            m_currentSelection = 0;
        }
        ShowCurrentSelection();

    }

    public void PrevStage()
    {
        if (m_currentSelection - 1 >= 0)
        {
            m_currentSelection--;
        }
        else m_currentSelection = m_stages.Length - 1;
        ShowCurrentSelection();

    }

    public void SelectStage()
    {
        StageManager.s_Instance.SelectStage(m_stages[m_currentSelection], m_currentSelection);
        GameManager.s_Instance.LoadScene(SceneType.CHOOSE_CHARACTER);
    }

    void InitStages()
    {
        var tempLis = new List<GameObject>();

        var nextPos = m_posObj.position;
        foreach (var item in m_stages)
        {
            var obj = Instantiate(item.Prefab, nextPos, Quaternion.identity);
            tempLis.Add(obj);
            nextPos += m_nextObjOffset;

        }
        m_stageObjSelection = tempLis.ToArray();

    }

    void ShowCurrentSelection()
    {
        m_nameStageText.text = m_stages[m_currentSelection].Name;

        if (m_currentObj) m_currentObj.transform.position += m_nextObjOffset;
        m_currentObj = m_stageObjSelection[m_currentSelection];
        m_currentObj.transform.position = m_posObj.position;

        StageManager.s_Instance.SelectStage(m_stages[m_currentSelection], m_currentSelection);

        if (StageManager.s_Instance.CurrentStageData.Rating != 0)
            ShowRecordStage();
        else m_record.SetActive(false);

    }

    void ShowRecordStage()
    {
        var data = StageManager.s_Instance.CurrentStageData;
        m_record.SetActive(true);

        for (int i = 0; i < 3; i++)
        {
            if (i < data.Rating) m_stars[i].SetActive(true);
            else m_stars[i].SetActive(false);
        }

        m_timeText.text = Helper.FloatToTimeSpan(data.BestTime);
        m_deadText.text = data.BestDead.ToString();

    }

}
