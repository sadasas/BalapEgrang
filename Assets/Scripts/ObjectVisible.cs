using UnityEngine;
using System.Collections;

public class ObjectVisible : MonoBehaviour
{
    [SerializeField] CameraController m_camera;

    void OnBecameVisible()
    {
        StartCoroutine(StopingCameraMove());
    }

    IEnumerator StopingCameraMove()
    {
        yield return new WaitForSeconds(2);
        m_camera.IsTrackAllowed = false;
    }

}
