using System.Collections.Generic;
using System;

namespace Player
{
    [Serializable]
    public struct PlayerData
    {
        public string CurrentCharacterSelection;
        public List<StageData> StageRecords;
    }
}

