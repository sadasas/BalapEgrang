using UnityEngine;

public class RaceMenu : MonoBehaviour
{
    public void NextStage() => StageManager.s_Instance.NextStage();

    public void Home() => GameManager.s_Instance.LoadScene(SceneType.MAIN_MENU);

    public void RestartStage() => StageManager.s_Instance.RestartStage();


}
