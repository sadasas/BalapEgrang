using UnityEngine;

namespace Player
{
    [CreateAssetMenu(fileName = "PlayerType", menuName = "Player/Type")]
    public class PlayerType : ScriptableObject
    {


        [Header("Description Setting")]
        public string Name;
        public GameObject characterDisplay;
        public GameObject characterPlayable;

        [Header("Movement Setting")]
        public float Speed;
        public float Acceleration;
        public float TurnSpeed;
        public float TurnRange;
        public float DistanceMove;

        [Header("Damage Setting")]
        public float RespawnPosDis;

        [Header("Ability Setting")]
        public int MaxPushVal;
        public int AbilityTime;

        [Header("UI Setting")]
        public Color BgUI;
        public Color BgCharacter;



    }
}

