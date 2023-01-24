using UnityEngine;

namespace Obstacle
{

    public class CTEObstacle : MonoBehaviour
    {

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                UIManager.s_this.ForceHUD(HUDType.CTE);
                var cteHandler = UIManager.s_this.GetHUD(HUDType.CTE).GetComponent<CTEHandler>();
                cteHandler.Obstacle = this.gameObject;
                cteHandler.Player = other.gameObject;
                cteHandler.PlayCTE();


            }
        }

    }

}
