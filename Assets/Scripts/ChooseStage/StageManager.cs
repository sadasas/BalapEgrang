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


    void Start()
    {

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

    public void CheckForNewRecord(float time, int rank, int dead)
    {
        if (m_currentStageData.Rating == 0)
        {
            var rate = CalculateRating(time);
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
                var rate = CalculateRating(time);
                m_currentStageData.Rating = rate;
                m_currentStageData.BestDead = dead;
                m_currentStageData.BestTime = time;
                m_currentStageData.StageIndex = m_stageSelected.StageIndex;
                SaveData();
            }
        }

    }


    public bool CheckNextStage()
    {
        if (m_index + 1 < m_stages.Length) return true;
        return false;
    }
    public void NextStage()
    {
        m_index++;
        m_stageSelected = m_stages[m_index];
        GameManager.s_Instance.LoadScene(m_stageSelected.StageIndex);
    }

    public Stage[] GetStages()
    {
        return m_stages;
    }

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

}

