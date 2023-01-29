using UnityEngine;

namespace Enemy
{
    [CreateAssetMenu(fileName ="Enemy",menuName ="Enemy")]
    public class EnemyLevel :ScriptableObject
    {
        public int DecisionVarian;
        public float DecisionMinCost;
        public float DecisionMaxCost;

    }
}