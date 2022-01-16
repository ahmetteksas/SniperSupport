using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwerveMovement : MonoBehaviour
{
    public float rotSpeed = 3f;

    void Update()
    {
        Shoot();
        //float _rotationX = Mathf.Clamp(transform.rotation.eulerAngles.x, -90.0F, 0.0F);
        // transform.rotation = Quaternion.Euler(rotationX, transform.eulerAngles.y, transform.eulerAngles.z);
        Vector3 rotation = transform.eulerAngles;
        rotation.y = Mathf.Clamp(transform.eulerAngles.y, 210f, 240f);
        rotation.z = Mathf.Clamp(transform.eulerAngles.z, 300f, 350f);
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, rotation.y, rotation.z);
    }
    public void Shoot()
    {
        if (Input.GetMouseButton(0))
        {
            transform.localEulerAngles += rotSpeed * new Vector3(0f, Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        }
    }
}