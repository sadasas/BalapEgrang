using Enemy;
using Player;
using Race;
using System.Collections;
using UI;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        MAINMENU,
        PLAYING,
        PAUSE,
    }

    public static GameState s_GameState = GameState.PLAYING;
    PlayerManager m_playerManager;
    UIManager m_UIManager;
    RaceManager m_RaceManager;
    EnemyManager m_EnemyManager;

    [SerializeField] GameObject m_playerManagerPrefab;
    [SerializeField] GameObject m_UIManagerPrefab;
    [SerializeField] GameObject m_raceManagerPrefab;
    [SerializeField] GameObject m_enemyManagerPrefab;



    /// <summary>
    /// 1. UIManager
    /// 2. playerManager
    /// </summary>
    private void Start()
    {
        m_UIManager = Instantiate(m_UIManagerPrefab).GetComponent<UIManager>();
        m_playerManager = Instantiate(m_playerManagerPrefab).GetComponent<PlayerManager>();
        m_EnemyManager = Instantiate(m_enemyManagerPrefab).GetComponent<EnemyManager>();

        if (s_GameState == GameState.PLAYING) StartCoroutine(StartGame());
    }
    IEnumerator StartGame()
    {
        m_RaceManager = Instantiate(m_raceManagerPrefab).GetComponent<RaceManager>();
        yield return null;
        m_playerManager.SpawnPlayablePlayer();
        m_EnemyManager.SpawnPlayableEnemy();
    }
}
