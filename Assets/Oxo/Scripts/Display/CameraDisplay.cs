using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class CameraDisplay : MonoBehaviour
{
    [SerializeField]
    Transform runnerCamPos;
    [SerializeField]
    Transform mazeCamPos;
    [SerializeField]
    Transform platformerCamPos;
    [SerializeField]
    Transform fpsCamPos;
    [SerializeField]

    public float cameraTransitionDelay;

    public void GoToRunnerCamPos() 
    {
        transform.SetParent(runnerCamPos);
        transform.DOMove(runnerCamPos.position, cameraTransitionDelay);
        transform.DOLocalRotate(Vector3.zero, cameraTransitionDelay);
    }
    public void GoToMazePos()
    {
        transform.SetParent(mazeCamPos);
        transform.DOLocalMove(Vector3.zero, cameraTransitionDelay);
        transform.DOLocalRotate(Vector3.zero, cameraTransitionDelay);
    }
    public void GoToPlatformerCamPos()
    {
        transform.SetParent(platformerCamPos);
        transform.DOLocalMove(Vector3.zero, cameraTransitionDelay);
        transform.DOLocalRotate(Vector3.zero, cameraTransitionDelay);
    }
    public void GoToFpsCamPos()
    {
        transform.SetParent(fpsCamPos);
        transform.DOLocalMove(Vector3.zero, cameraTransitionDelay);
        transform.DOLocalRotate(Vector3.zero, cameraTransitionDelay);
    }
}
