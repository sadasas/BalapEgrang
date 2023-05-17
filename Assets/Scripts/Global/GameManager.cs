using Enemy;
using Player;
using BalapEgrang.Sound;
using Race;
using System.Collections;
using UI;
using UnityEngine;
using UnityEngine.UI;
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
    PlayerManager m_playerManager;
    UIManager m_UIManager;
    RaceManager m_RaceManager;
    EnemyManager m_EnemyManager;
    StageManager m_stageManager;
    TutorialManager m_tutorialManager;
    SoundManager m_soundManager;

    [SerializeField] GameObject m_playerManagerPrefab;
    [SerializeField] GameObject m_stageManagerPrefab;
    [SerializeField] GameObject m_UIManagerPrefab;
    [SerializeField] GameObject m_raceManagerPrefab;
    [SerializeField] GameObject m_enemyManagerPrefab;
    [SerializeField] GameObject m_tutorialManagerPrefab;
    [SerializeField] GameObject m_soundManagerPrefab;

    void Awake()
    {
        if (s_Instance != null) Destroy(gameObject);
        else
            s_Instance = this;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            PlayerPrefs.SetInt("Tutorial", 0);
        }
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
        InitUIManager();
        InitSoundManager();

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
            case "Tutorial":
                SetupTutorial();
                break;
            default:
                break;
        }
    }


    public void LoadScene(SceneType type)
    {
        var sceneName = "";
        // if (type == SceneType.STAGE_1)
        // {
        //     var tutorial = PlayerPrefs.GetInt("Tutorial");
        //     if (tutorial == 0)
        //         sceneName = "Tutorial";
        // }

        if (sceneName == "")
        {
            sceneName = type switch
            {
                SceneType.CHOOSE_CHARACTER => "ChooseCharacter",
                SceneType.MAIN_MENU => "MainMenu",
                SceneType.CHOOSE_STAGE => "ChooseStage",
                SceneType.STAGE_1 => "Stage1",
                SceneType.STAGE_2 => "Stage2",
                _ => ""
            };

        }
        StartCoroutine(ProcessLoadScene(sceneName));
    }


    IEnumerator ProcessLoadScene(string sceneName)
    {
        var m_loadSceneHUD = m_UIManager.GetHUD(HUDType.LOADING_SCENE);
        var m_loadSlider = m_loadSceneHUD.transform.GetChild(0).GetChild(0).GetComponent<Slider>();
        m_loadSceneHUD.SetActive(true);

        AsyncOperation asyncLoadScene = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        while (!asyncLoadScene.isDone)
        {
            m_loadSlider.value = asyncLoadScene.progress;

            yield return null;

        }
        yield return null;
        m_loadSceneHUD.SetActive(false);
    }



    private void SetupTutorial()
    {
        m_tutorialManager = Instantiate(m_tutorialManagerPrefab).GetComponent<TutorialManager>();

    }

    void InitStageManager()
    {
        m_stageManager = Instantiate(m_stageManagerPrefab).GetComponent<StageManager>();
    }

    void InitPlayerManager()
    {

        m_playerManager = Instantiate(m_playerManagerPrefab).GetComponent<PlayerManager>();
    }


    private void InitSoundManager()
    {
        m_soundManager = Instantiate(m_soundManagerPrefab).GetComponent<SoundManager>();
    }


    void InitUIManager()
    {

        m_UIManager = Instantiate(m_UIManagerPrefab).GetComponent<UIManager>();
    }

    void StartGame()
    {
        m_EnemyManager = Instantiate(m_enemyManagerPrefab).GetComponent<EnemyManager>();

        if (s_GameState == GameState.PLAYING) StartCoroutine(StartingGame());

    }
    IEnumerator StartingGame()
    {
        yield return null;
        m_playerManager.SetupPlayerForRace();
        m_EnemyManager.SpawnPlayableEnemy();
    }

}
