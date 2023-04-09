using UnityEngine;

namespace Race
{
    public class FinishHandler : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            var racer = other.GetComponent<IRacer>();
            if (racer != null)
            {
                RaceManager.s_Instance.RacerFinished(racer);
            }

        }

    }

}
