using UnityEngine;


public class MainMenu : MonoBehaviour
{

    public void LoadScene()
    {
        GameManager.s_Instance.LoadScene(SceneType.CHOOSE_STAGE);

    }
}
