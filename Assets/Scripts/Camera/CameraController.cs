
using UnityEngine;

/// <summary>
/// TODO:
/// change player value when player is instantied 
/// </summary>
public class CameraController : MonoBehaviour
{

    [SerializeField] float m_smoothDamp;
    [SerializeField] float m_offsetX;

    public Transform Player { get; set; }


    void LateUpdate()
    {
       if(Player) TrackPlayer();

    }
    void TrackPlayer()
    {
        var pos = new Vector3(transform.position.x, transform.position.y, Player.position.z + m_offsetX);
        transform.position = Vector3.Lerp(transform.position, pos, m_smoothDamp);
    }

}
