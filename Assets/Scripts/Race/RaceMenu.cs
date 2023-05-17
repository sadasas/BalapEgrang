using UnityEngine;

public class RaceMenu : MonoBehaviour
{

    void OnEnable()
    {
        Time.timeScale = 0;
        transform.SetAsLastSibling();
    }

    void OnDisable()
    {
        Time.timeScale = 1;
    }
    public void NextStage() => StageManager.s_Instance.NextStage();

    public void Home() => GameManager.s_Instance.LoadScene(SceneType.MAIN_MENU);

    public void RestartStage() => StageManager.s_Instance.RestartStage();


}
