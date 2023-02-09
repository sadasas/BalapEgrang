using UnityEngine;
using Player;

public class StageManager : MonoBehaviour
{
    public static StageManager s_Instance;

    Stage m_stageSelected;
    StageData m_currentStageData;
    int m_index;

    [SerializeField] Stage[] m_stages;

    public StageData CurrentStageData { get => m_currentStageData; }
    public Stage StageSelected { get => m_stageSelected; }

    void Awake()
    {
        if (s_Instance) Destroy(gameObject);
        s_Instance = this;
        DontDestroyOnLoad(gameObject);
    }


    void LoadData()
    {
        m_currentStageData = PlayerManager.s_Instance.GetStageRecord(m_stageSelected.StageIndex);

    }

    void SaveData()
    {
        PlayerManager.s_Instance.UpdateStageRecord(m_currentStageData);
    }
    public void SelectStage(Stage stage, int index)
    {
        m_index = index;
        m_stageSelected = stage;
        LoadData();
    }


    #region  RACE API

    public int CalculateRating(float time)
    {
        if (time < m_stageSelected.RateA)
        {
            return 3;
        }
        else if (time < m_stageSelected.RateB)
        {
            return 2;
        }
        else return 1;
    }

    public void CheckForNewRecord(float time, int rank, int dead)
    {

        var rate = CalculateRating(time);
        if (m_currentStageData.Rating == 0)
        {

            m_currentStageData.Rating = rate;
            m_currentStageData.BestDead = dead;
            m_currentStageData.BestTime = time;
            m_currentStageData.StageIndex = m_stageSelected.StageIndex;
            SaveData();
        }
        else
        {
            if (m_currentStageData.BestTime > time)
            {
                m_currentStageData.Rating = rate;
                m_currentStageData.BestDead = dead;
                m_currentStageData.BestTime = time;
                m_currentStageData.StageIndex = m_stageSelected.StageIndex;
                SaveData();
            }
        }

    }

    public void CheckReward(int dead, float time, int rank)
    {
        if (StageSelected.Quests.Length == 0) return;

        var rewardCollected = PlayerManager.s_Instance.DataPlayer.RewardCollecteds;

        if (rewardCollected != null && rewardCollected.Count > 0)
        {
            foreach (var stage in rewardCollected)
            {
                if (stage == StageSelected.StageIndex) return;
            }

        }
        var quest = StageSelected.Quests[0];

        if (quest.Type == Race.DataRaceType.RATE)
        {

            var rate = CalculateRating(time);
            if (rate == quest.Rate)
            {
                PlayerManager.s_Instance.AddReward(StageSelected.StageIndex);
            }
        }

        else if (quest.Type == Race.DataRaceType.RESPAWNED)
        {
            if (dead == quest.Dead)
            {
                PlayerManager.s_Instance.AddReward(StageSelected.StageIndex);
            }

        }
    }

    #endregion



    public bool CheckNextStage()
    {
        if (m_index + 1 < m_stages.Length) return true;
        return false;
    }
    public void NextStage()
    {
        m_index++;
        m_stageSelected = m_stages[m_index];
        LoadData();
        GameManager.s_Instance.LoadScene(m_stageSelected.StageIndex);
    }

    public void RestartStage()
    {
        GameManager.s_Instance.LoadScene(m_stageSelected.StageIndex);
    }

    public Stage[] GetStages()
    {
        return m_stages;
    }


}

