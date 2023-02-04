using UnityEngine;
using Player;

public class StageManager : MonoBehaviour
{
    public static StageManager s_Instance;

    Stage m_stageSelected;
    StageData m_currentStageData;

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
    public void SelectStage(Stage stage)
    {
        m_stageSelected = stage;
        LoadData();
    }

    public void CheckForNewRecord(float time, int rank, int dead)
    {
        if (m_currentStageData.Rating == 0)
        {
            Debug.Log("new record");
            m_currentStageData.BestDead = dead;
            m_currentStageData.BestTime = time;
            m_currentStageData.Rating = rank;
            m_currentStageData.StageIndex = m_stageSelected.StageIndex;
        }
        else
        {
            if (m_currentStageData.BestTime > time)
            {
                Debug.Log("new record");
                m_currentStageData.BestDead = dead;
                m_currentStageData.BestTime = time;
                m_currentStageData.Rating = rank;
                m_currentStageData.StageIndex = m_stageSelected.StageIndex;
            }
        }

        SaveData();
    }


}

