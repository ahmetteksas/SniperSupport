using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MainCameraDisplay : MonoBehaviour
{
    public float movementTime;
    void Start()
    {
        transform.DOLocalMove(Vector3.zero, movementTime);
        transform.DOLocalRotate(Vector3.zero, movementTime);
    }

    void Update()
    {

    }
}
