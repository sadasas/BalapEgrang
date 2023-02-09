using UnityEngine;
using Race;
using Player;

[CreateAssetMenu(menuName = "Quest", fileName = "Quest")]
public class Quest : ScriptableObject
{
    public DataRaceType Type;
    public string Description;

    public int Rate;
    public int Rank;
    public int Dead;
    public float Time;


    [Header("Reward Setting")]
    public PlayerType CharacterReward;
    public Sprite CharacterImageReward;

}


