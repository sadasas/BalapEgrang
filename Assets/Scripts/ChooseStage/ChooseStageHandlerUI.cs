using UnityEngine;
using TMPro;
using Utility;
using UnityEngine.UI;
using Player;
using BalapEgrang.Sound;

public class ChooseStageHandlerUI : MonoBehaviour
{
    int m_currentSelection = 0;
    int m_stageUncloked = 0;
    Stage[] m_stages;

    [Header("Record Setting")]
    [SerializeField] GameObject m_record;
    [SerializeField] TextMeshProUGUI m_timeText;
    [SerializeField] TextMeshProUGUI m_deadText;
    [SerializeField] GameObject[] m_starFilled;
    [SerializeField] GameObject[] m_star;
    [SerializeField] Transform[] m_starPos;

    [SerializeField] TextMeshProUGUI m_nameStageText;
    [SerializeField] Image m_stageImg;

    [Header("Quest Setting")]
    [SerializeField] TextMeshProUGUI m_descQuestText;
    [SerializeField] Image m_rewardQuestImg;
    [SerializeField] GameObject m_questAvailableHUD;
    [SerializeField] GameObject m_questCompletedHUD;
    [SerializeField] GameObject m_questCollectedHUD;


    [Header("Reward Setting")]
    [SerializeField] GameObject m_rewardHUD;
    [SerializeField] Image m_rewardImg;
    [SerializeField] TextMeshProUGUI m_titleRewardText;


    [SerializeField] GameObject m_lockHUD;
    [SerializeField] GameObject m_selectBtn;

    void Start()
    {
        m_stages = StageManager.s_Instance.GetStages();
        m_stageUncloked = PlayerManager.s_Instance.GetStageUnlocked();
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

    public void Back()
    {
        GameManager.s_Instance.LoadScene(SceneType.MAIN_MENU);
    }

    public void CollectReward()
    {
        PlayerManager.s_Instance.CollectReward(m_stages[m_currentSelection].StageIndex);
        PlayerManager.s_Instance.AddNewCharacter(m_stages[m_currentSelection].Quests[0].CharacterReward);

        SoundManager.s_Instance.PlaySFX(SFXType.NEW_CHARACTER);

        m_questAvailableHUD.SetActive(false);
        m_questCollectedHUD.SetActive(true);

        InitReward(m_stages[m_currentSelection].Quests[0]);

    }


    void InitReward(Quest quest)
    {
        m_rewardHUD.SetActive(true);
        m_rewardImg.sprite = quest.CharacterImageReward;
        m_titleRewardText.text = quest.CharacterReward.Name;

    }

    bool IsStageLocked()
    {
        if (m_stageUncloked > m_currentSelection) return false;
        return true;

    }

    void ShowCurrentSelection()
    {
        m_lockHUD.SetActive(false);
        m_selectBtn.SetActive(true);

        var data = m_stages[m_currentSelection];
        m_nameStageText.text = data.Name;
        m_stageImg.sprite = data.Image;
        StageManager.s_Instance.SelectStage(m_stages[m_currentSelection], m_currentSelection);

        if (StageManager.s_Instance.CurrentStageData.Rating != 0)
            ShowRecordStage();
        else m_record.SetActive(false);

        CheckQuest();

        if (IsStageLocked())
        {
            m_lockHUD.SetActive(true);
            m_selectBtn.SetActive(false);


        }
    }

    void ShowRecordStage()
    {
        var data = StageManager.s_Instance.CurrentStageData;
        m_record.SetActive(true);

        for (int i = 0; i < 3; i++)
        {
            if (i < data.Rating)
            {

                m_starFilled[i].SetActive(true);
                m_starFilled[i].transform.position = m_starPos[i].position;

                m_star[i].SetActive(false);
            }
            else if (i >= data.Rating)
            {
                m_starFilled[i].SetActive(false);
                m_star[i].SetActive(true);
                m_star[i].transform.position = m_starPos[i].position;
            }
        }
        m_timeText.text = Helper.FloatToTimeSpan(data.BestTime);
        m_deadText.text = data.BestDead.ToString();

    }

    void CheckQuest()
    {

        var questCompleted = PlayerManager.s_Instance.DataPlayer.RewardCollecteds;
        if (questCompleted != null && questCompleted.Count > 0)
        {
            foreach (var item in questCompleted)
            {
                if (item == m_stages[m_currentSelection].StageIndex)
                {
                    m_questAvailableHUD.SetActive(false);
                    m_questCompletedHUD.SetActive(false);
                    m_questCollectedHUD.SetActive(true);
                    return;
                }
            }
        }

        var questUnCollected = PlayerManager.s_Instance.DataPlayer.RewardUnCollecteds;
        if (questUnCollected != null && questUnCollected.Count > 0)
        {
            foreach (var item in questUnCollected)
            {
                if (item == m_stages[m_currentSelection].StageIndex)
                {
                    m_questAvailableHUD.SetActive(false);
                    m_questCollectedHUD.SetActive(false);
                    m_questCompletedHUD.SetActive(true);
                    return;
                }

            }
        }

        ShowQuest(m_stages[m_currentSelection]);

    }

    void ShowQuest(Stage stage)
    {
        if (stage.Quests.Length == 0) return;
        var quest = stage.Quests[0];

        m_questCollectedHUD.SetActive(false);
        m_questCompletedHUD.SetActive(false);
        m_questAvailableHUD.SetActive(true);

        m_descQuestText.text = quest.Description;
        m_rewardQuestImg.sprite = quest.CharacterImageReward;
    }

}
