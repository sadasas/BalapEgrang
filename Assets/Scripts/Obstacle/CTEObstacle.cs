using Enemy;
using Player;
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
                UIManager.s_Instance.ForceHUD(HUDType.CTE);
                var player = other.GetComponent<PlayerController>();
                player.MovementBehaviour.ForceStopMovement();
                player.MovementBehaviour.Idle();
                player.Reposition();
                var cteHandler = UIManager.s_Instance.GetHUD(HUDType.CTE).GetComponent<CTEHUDHandler>();
                cteHandler.Obstacle = this.gameObject;
                cteHandler.SetPlayer(other.gameObject);
                cteHandler.PlayCTE();


            }
            else if (other.CompareTag("Enemy"))
            {
                var cteHandler = other.GetComponent<AIController>();
                cteHandler.OnCTETriggered(gameObject);
            }
        }

    }

}
