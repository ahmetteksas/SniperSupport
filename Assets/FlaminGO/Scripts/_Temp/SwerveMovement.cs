using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwerveMovement : MonoBehaviour
{
    public float sensitivity = 3f;

    [SerializeField] float lerpSensitivity;

    [SerializeField]
    float xClampMin, xClampMax,
           yClampMin, yClampMax,
           zClampMin, zClampMax;

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(.1f);

        if (transform.parent == null)
        {
            GameObject go = GameObject.FindWithTag("Player");

            transform.SetParent(go.transform);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
        }
    }
  
}