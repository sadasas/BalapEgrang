using UnityEngine;
using UnityEngine.UI;
using BalapEgrang.Sound;

public class SoundSetting : MonoBehaviour
{

    [SerializeField] Image m_musicImg;
    [SerializeField] Image m_sfxImg;
    [SerializeField] Sprite m_sfxOnSprite;
    [SerializeField] Sprite m_sfxOffSprite;
    [SerializeField] Sprite m_bgmOnSprite;
    [SerializeField] Sprite m_bgmOffSprite;

    void Start()
    {
        m_musicImg.sprite = SoundManager.s_Instance.IsBGMMute ? m_bgmOffSprite : m_bgmOnSprite;
    }

    public void ToggleMusic()
    {
        SoundManager.s_Instance.IsBGMMute = !SoundManager.s_Instance.IsBGMMute;
        m_musicImg.sprite = SoundManager.s_Instance.IsBGMMute ? m_bgmOffSprite : m_bgmOnSprite;
        if (SoundManager.s_Instance.IsBGMMute) SoundManager.s_Instance.StopBGM();
        else SoundManager.s_Instance.PlayBGM(BGMType.MAINMENU);
    }

    public void ToggleSFX()
    {
        SoundManager.s_Instance.IsSFXMute = !SoundManager.s_Instance.IsSFXMute;
        m_sfxImg.sprite = SoundManager.s_Instance.IsSFXMute ? m_sfxOffSprite : m_sfxOnSprite;
    }

}
