using UnityEngine;
using Enemy;



[CreateAssetMenu(menuName = "Stage", fileName = "Stage")]
public class Stage : ScriptableObject
{
    public string Name;
    public SceneType StageIndex;
    public string Description;
    public Sprite Image;
    public AILevel Level;

    [Header("Rating Setting")]
    public float RateA;
    public float RateB;
    public float RateC;


    [Header("CTE Obstacle Setting")]
    public float TimeCTEObstacle;
    public float IncrementCTEObstacle;

    [Header("Quest Setting")]
    public Quest[] Quests;

}

