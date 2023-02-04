using UnityEngine;

[CreateAssetMenu(menuName = "Stage", fileName = "Stage")]
public class Stage : ScriptableObject
{
    public string Name;
    public SceneType StageIndex;
    public string Description;
    public GameObject Prefab;
}

