using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MainCameraDisplay : MonoBehaviour
{

    Transform defaultParent;
    public float movementTime;


    private void Awake()
    {
        //defaultParent = transform.parent;
    }
    void Start()
    {
        transform.DOLocalMove(Vector3.zero, 0f);
        transform.DOLocalRotate(Vector3.zero, 0f);
    }

    //public void SetParentDefault()
    //{
    //    transform.SetParent(null);
    //    transform.SetParent(defaultParent);
    //    transform.localPosition = Vector3.zero;
    //    transform.localRotation = Quaternion.identity;
    //}
    //public void SetParentNull()
    //{
    //    transform.SetParent(transform.parent.parent);
    //    //transform.localPosition = Vector3.zero;
    //    //transform.localRotation = Quaternion.identity;
    //}
}
