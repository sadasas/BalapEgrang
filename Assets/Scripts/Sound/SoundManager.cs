
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BalapEgrang.Sound
{
    public enum BGMType
    {
        MAINMENU = 0,
        RACE

    }

    public enum AmbienceType
    {
        KITCHEN
    }
    public enum SFXType
    {
        BTN_CLICK,
        PLAYER_FALL,
        NEW_CHARACTER,
        POWER_UP,
        WIN,
        COUNTDOWN
    }

    public class SoundManager : MonoBehaviour
    {
        public static SoundManager s_Instance;

        [SerializeField] AudioSource m_BGMAudio;
        [SerializeField] AudioSource m_SFXAudio;

        [Header("BGM List")]
        [SerializeField] AudioClip m_inGameBGM;
        [SerializeField] AudioClip m_raceBGM;

        [Header("SFX List")]
        [SerializeField] AudioClip m_btnClickSFX;
        [SerializeField] AudioClip m_playerRunSFX;
        [SerializeField] AudioClip m_playerFallSFX;
        [SerializeField] AudioClip m_NewCharacterSFX;
        [SerializeField] AudioClip m_PowerUpSFX;
        [SerializeField] AudioClip m_winSFX;
        [SerializeField] AudioClip m_countDownSFX;


        public bool IsBGMMute;
        public bool IsSFXMute;

        private void Awake()
        {
            if (s_Instance != null) Destroy(gameObject);
            else
            {
                s_Instance = this;
                DontDestroyOnLoad(gameObject);

            }
        }


        private void Start()
        {
            LoadPlayerSetting();
            SceneManager.sceneLoaded += OnSceneLoaded;

            PlayBGM(BGMType.MAINMENU);

        }


        public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            switch (scene.name)
            {
                case "MainMenu":
                    PlayBGM(BGMType.MAINMENU);
                    break;
                case "Stage1":
                case "Stage2":
                    PlayBGM(BGMType.RACE);
                    break;
                default:
                    break;
            }
        }

        void LoadPlayerSetting()
        {
            IsBGMMute = PlayerPrefs.GetInt("BGM") == 1 ? true : false;
            IsSFXMute = PlayerPrefs.GetInt("SFX") == 1 ? true : false;
        }

        public void PlayBGM(BGMType type)
        {
            if (IsBGMMute) return;
            var audioClip = (type) switch
            {
                BGMType.MAINMENU => m_inGameBGM,
                BGMType.RACE => m_raceBGM,
                _ => null
            };
            m_BGMAudio.clip = audioClip;
            m_BGMAudio.Play();
        }


        public void StopBGM()
        {

            m_BGMAudio.Pause();
        }

        public void PlaySFX(SFXType type)
        {
            if (IsSFXMute) return;
            var audioClip = (type) switch
            {
                SFXType.BTN_CLICK => m_btnClickSFX,
                SFXType.PLAYER_FALL => m_playerFallSFX,
                SFXType.NEW_CHARACTER => m_NewCharacterSFX,
                SFXType.POWER_UP => m_PowerUpSFX,
                SFXType.WIN => m_winSFX,
                SFXType.COUNTDOWN => m_countDownSFX,
                _ => null
            };

            m_SFXAudio.clip = audioClip;
            m_SFXAudio.Play();
        }


    }
}
