using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwerveMovement : MonoBehaviour
{
    public float rotSpeed = 3f;

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
            transform.SetParent(go.transform);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
        }

        Shoot();
        //Vector3 rotation = transform.eulerAngles;
        //rotation.x = Mathf.Clamp(transform.eulerAngles.x, xClampMin, xClampMax);
        //rotation.y = Mathf.Clamp(transform.eulerAngles.y, yClampMin, yClampMax);
        //rotation.z = Mathf.Clamp(transform.eulerAngles.z, zClampMin, zClampMax);

        //transform.rotation = Quaternion.Euler(rotation.x, rotation.y, rotation.z);
    }

    public void Shoot()
    {
        if (!shooter.cross.activeInHierarchy)
            return;

        if (Input.GetMouseButton(0))
        {
            transform.localEulerAngles += rotSpeed * new Vector3(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X") * 2, Input.GetAxis("Mouse Y")) * Time.deltaTime;
        }
    }
}