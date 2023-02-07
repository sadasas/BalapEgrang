using UnityEngine;
using UnityEngine.UI;

public class NewCharacterHandlerUI : MonoBehaviour
{
    [SerializeField] Image m_characterImage;


    public void SetNewCharacter(Sprite image)
    {
        m_characterImage.sprite = image;
    }

}
