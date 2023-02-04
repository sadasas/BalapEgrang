using System;

[Serializable]
public struct StageData
{
    public SceneType StageIndex;
    public float BestTime;
    public int BestDead;
    public int Rating;

    public override string ToString()
    {
        return $"i:{StageIndex} t:{BestTime} d:{BestDead} r:{Rating}";
    }


}

