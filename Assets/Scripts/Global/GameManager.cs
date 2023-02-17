using Enemy;
using Player;
using Race;
using System.Collections;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;


public enum SceneType
{
    MAIN_MENU,
    CHOOSE_CHARACTER,
    CHOOSE_STAGE,
    STAGE_1,
    STAGE_2
}
public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        MAINMENU,
        PLAYING,
        PAUSE,
    }

    public static GameState s_GameState = GameState.PLAYING;
    public static GameManager s_Instance;
    public PlayerManager m_playerManager;
    UIManager m_UIManager;
    RaceManager m_RaceManager;
    EnemyManager m_EnemyManager;
    StageManager m_stageManager;

    [SerializeField] GameObject m_playerManagerPrefab;
    [SerializeField] GameObject m_stageManagerPrefab;
    [SerializeField] GameObject m_UIManagerPrefab;
    [SerializeField] GameObject m_raceManagerPrefab;
    [SerializeField] GameObject m_enemyManagerPrefab;

    void Awake()
    {
        if (s_Instance != null) Destroy(gameObject);
        else
            s_Instance = this;
    }

    /// <summary>
    /// 1. UIManager
    /// 2. playerManager
    /// </summary>
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;

        InitPlayerManager();

    }
    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        switch (scene.name)
        {
            case "Stage1":
            case "Stage2":
                StartGame();
                break;
            case "ChooseStage":
                InitStageManager();
                break;
            default:
                break;
        }
    }

    public void LoadScene(SceneType type)
    {
        var sceneName = type switch
        {
            SceneType.CHOOSE_CHARACTER => "ChooseCharacter",
            SceneType.MAIN_MENU => "MainMenu",
            SceneType.CHOOSE_STAGE => "ChooseStage",
            SceneType.STAGE_1 => "Stage1",
            SceneType.STAGE_2 => "Stage2",
        };

        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }

    void InitStageManager()
    {
        m_stageManager = Instantiate(m_stageManagerPrefab).GetComponent<StageManager>();
    }

    void InitPlayerManager()
    {

        m_playerManager = Instantiate(m_playerManagerPrefab).GetComponent<PlayerManager>();
    }

    void StartGame()
    {
        m_UIManager = Instantiate(m_UIManagerPrefab).GetComponent<UIManager>();
        m_EnemyManager = Instantiate(m_enemyManagerPrefab).GetComponent<EnemyManager>();

        if (s_GameState == GameState.PLAYING) StartCoroutine(StartingGame());

    }
    IEnumerator StartingGame()
    {
        yield return null;
        m_playerManager.SpawnPlayablePlayer();
        m_EnemyManager.SpawnPlayableEnemy();
    }

}
