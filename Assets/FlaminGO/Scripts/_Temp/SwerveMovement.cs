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

    Vector3 firstMousePosition;
    Vector3 tempMousePosition;
    Vector3 mouseDelta;

    void Update()
    {
        if (transform.parent == null)
        {
            GameObject go = GameObject.FindWithTag("Player");

            if (!go)
                return;

            transform.SetParent(go.transform);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
        }


        if (LevelManager.instance.isGameRunning)
            Swerve();
    }
    Vector3 targetEulerAngles;
    public void Swerve()
    {
        if (Input.GetMouseButtonDown(0))
            firstMousePosition = Input.mousePosition;

        if (Input.GetMouseButton(0))
        {
            transform.localRotation = Quaternion.Euler(targetEulerAngles);// Quaternion.Lerp(transform.localRotation, Quaternion.Euler(targetEulerAngles), lerpSensitivity);

            tempMousePosition = Input.mousePosition;
            mouseDelta = tempMousePosition - firstMousePosition;

            targetEulerAngles = transform.localEulerAngles;

            targetEulerAngles += sensitivity * new Vector3(-mouseDelta.x, -mouseDelta.y * 2, mouseDelta.y * 4.5f / sensitivity) * Time.deltaTime;

            firstMousePosition = tempMousePosition;
        }
    }
}