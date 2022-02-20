using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwerveMovement : MonoBehaviour
{
    public float rotSpeed = 3f;


    public float xClampMin = 0f;
    public float xClampMax = 38f;
    public float yClampMin = 230f;
    public float yClampMax = 267f;
    public float zClampMin = 230f;
    public float zClampMax = 267f;

    Shooter shooter;
    float defaultScopeZoom;
    private void Start()
    {
        shooter = GetComponentInChildren<Shooter>();
        defaultScopeZoom = shooter.scopeZoom;
    }

    void Update()
    {
        Shoot();
        //Vector3 rotation = transform.eulerAngles;
        //rotation.x = Mathf.Clamp(transform.eulerAngles.x, xClampMin, xClampMax);
        //rotation.y = Mathf.Clamp(transform.eulerAngles.y, yClampMin, yClampMax);
        //rotation.z = Mathf.Clamp(transform.eulerAngles.z, zClampMin, zClampMax);

        //transform.rotation = Quaternion.Euler(rotation.x, rotation.y, rotation.z);
    }

    public void Shoot()
    {
        if (Input.GetMouseButton(0))
        {
            transform.localEulerAngles += rotSpeed * new Vector3(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X")*2, Input.GetAxis("Mouse Y"))*Time.deltaTime;
        }
    }
}