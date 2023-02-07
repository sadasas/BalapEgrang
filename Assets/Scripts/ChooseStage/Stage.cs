using UnityEngine;
using Player;
using Enemy;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Stage", fileName = "Stage")]
public class Stage : ScriptableObject
{
    public string Name;
    public SceneType StageIndex;
    public string Description;
    public GameObject Prefab;
    public AILevel Level;

    [Header("Rating Setting")]
    public float RateA;
    public float RateB;
    public float RateC;


    [Header("Reward Setting")]
    public int RateReward;
    public PlayerType CharacterReward;
    public Sprite CharacterImageReward;
}

