using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwerveMovement : MonoBehaviour
{
    public float sensitivity = 3f;

    Shooter shooter;
    float defaultScopeZoom;

    private void Start()
    {
        shooter = GetComponentInChildren<Shooter>();
        defaultScopeZoom = shooter.scopeZoom;




    }

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

        if (ObjectPool.instance.isGameRunning)
            Swerve();
        //Vector3 rotation = transform.eulerAngles;
        //rotation.x = Mathf.Clamp(transform.eulerAngles.x, xClampMin, xClampMax);
        //rotation.y = Mathf.Clamp(transform.eulerAngles.y, yClampMin, yClampMax);
        //rotation.z = Mathf.Clamp(transform.eulerAngles.z, zClampMin, zClampMax);

        //transform.rotation = Quaternion.Euler(rotation.x, rotation.y, rotation.z);
    }


    Vector3 firstMousePosition;
    Vector3 tempMousePosition;
    Vector3 lastMousePosition;
    Vector3 mouseDelta;
    public void Swerve()
    {
        //if (!shooter.cross.activeInHierarchy)
        //    return;

        if (Input.GetMouseButtonDown(0))
        {
            firstMousePosition = Input.mousePosition;
        }

        if (Input.GetMouseButton(0))
        {
            tempMousePosition = Input.mousePosition;
            mouseDelta = tempMousePosition - firstMousePosition;

            transform.localEulerAngles += sensitivity * new Vector3(-mouseDelta.x, -mouseDelta.y * 2, mouseDelta.y * 2f) * Time.deltaTime;

            firstMousePosition = tempMousePosition;
        }
    }
}