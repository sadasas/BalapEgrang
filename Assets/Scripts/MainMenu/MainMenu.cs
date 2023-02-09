using UnityEngine;


public class MainMenu : MonoBehaviour
{

    public void Play()
    {
        GameManager.s_Instance.LoadScene(SceneType.CHOOSE_STAGE);

    }
}
