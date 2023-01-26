using UI;
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
                var cteHandler = UIManager.s_this.GetHUD(HUDType.CTE).GetComponent<CTEHUDHandler>();
                cteHandler.Obstacle = this.gameObject;
                cteHandler.SetPlayer(other.gameObject);
                cteHandler.PlayCTE();


            }
        }

    }

}
