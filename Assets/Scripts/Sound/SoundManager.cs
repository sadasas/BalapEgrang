
using UnityEngine;

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
        BTNCLICK,
        PLAYERFALL,
        NEWCHARACTER,

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


        public bool IsBGMMute;
        public bool IsSFXMute;

        private void Awake()
        {
            if (s_Instance != null) Destroy(gameObject);
            else s_Instance = this;
        }


        private void Start()
        {
            // LoadPlayerSetting();

            if (!IsBGMMute) PlayBGM(BGMType.MAINMENU);
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
                SFXType.BTNCLICK => m_btnClickSFX,
                SFXType.PLAYERFALL => m_playerFallSFX,
                // SFXType.NEWCHARACTER => m_newCharacterSFX,
                _ => null
            };

            m_SFXAudio.clip = audioClip;
            m_SFXAudio.Play();
        }


    }
}
