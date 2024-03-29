
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public bool IsTrackAllowed = true;

    [SerializeField] float m_smoothDamp;
    [SerializeField] float m_offsetX;


    public Transform Player { get; set; }

    void LateUpdate()
    {
        if (Player && IsTrackAllowed) TrackPlayer();

    }
    void TrackPlayer()
    {
        var pos = new Vector3(transform.position.x, transform.position.y, Player.position.z + m_offsetX);
        transform.position = Vector3.Lerp(transform.position, pos, m_smoothDamp);
    }

}
