using System;
using Player;
using UnityEngine;

namespace Utility
{

    public class Helper
    {

        public static string FloatToTimeSpan(float second)
        {

            TimeSpan time = TimeSpan.FromSeconds(second);

            return time.ToString("hh':'mm':'ss");
        }
        public static PlayerType GetPlayerType(string name)
        {
            var so = Resources.Load<PlayerType>($"ScriptableObject/Player/{name}");
            return so;
        }
    }
}
