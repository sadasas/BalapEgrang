
using BalapEgrang.Sound;
using UnityEngine;

public class ClickButton : MonoBehaviour
{
    public void PlaySfx()
    {
        SoundManager.s_Instance.PlaySFX(SFXType.BTNCLICK);
    }

}

