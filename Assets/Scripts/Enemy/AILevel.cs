using UnityEngine;

namespace Enemy
{
    [CreateAssetMenu(fileName = "Enemy", menuName = "Enemy")]
    public class AILevel : ScriptableObject
    {
        public int DecisionVarian;
        public float DecisionMinCost;
        public float DecisionMaxCost;

        public float Speed;

    }
}
