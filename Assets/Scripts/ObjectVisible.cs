using UnityEngine;
using System.Collections;

public class ObjectVisible : MonoBehaviour
{
    [SerializeField] CameraController m_camera;
    [SerializeField] float m_tresholdStopCamera;

    void OnBecameVisible()
    {
        StartCoroutine(StopingCameraMove());
    }

    IEnumerator StopingCameraMove()
    {
        yield return new WaitForSeconds(m_tresholdStopCamera);
        m_camera.IsTrackAllowed = false;
    }

}
