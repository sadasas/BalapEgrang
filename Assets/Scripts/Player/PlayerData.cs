using System.Collections.Generic;
using System;

namespace Player
{
    [Serializable]
    public struct PlayerData
    {
        public int StageUnlocked;
        public string CurrentCharacterSelection;
        public List<StageData> StageRecords;
        public List<PlayerType> CharacterCollections;
        public string[] CharacterNames;
        public List<SceneType> RewardCollecteds;
        public List<SceneType> RewardUnCollecteds;
    }
}

