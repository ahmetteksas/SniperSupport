using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwerveMovement : MonoBehaviour
{
    public float rotSpeed = 3f;


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
        //float _rotationX = Mathf.Clamp(transform.rotation.eulerAngles.x, -90.0F, 0.0F);
        // transform.rotation = Quaternion.Euler(rotationX, transform.eulerAngles.y, transform.eulerAngles.z);
        Vector3 rotation = transform.eulerAngles;
        rotation.y = Mathf.Clamp(transform.eulerAngles.y, yClampMin, yClampMax);
        rotation.z = Mathf.Clamp(transform.eulerAngles.z, zClampMin, zClampMax);

        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, rotation.y, rotation.z);
        float distance = zClampMax - transform.eulerAngles.z;

        //shooter.scopeZoom = defaultScopeZoom * (distance) / (zClampMax - zClampMin);
    }

    public void Shoot()
    {
        if (Input.GetMouseButton(0))
        {
            transform.localEulerAngles += rotSpeed * new Vector3(0f, Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        }
    }
}