using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwerveMovement : MonoBehaviour
{
    float rotSpeed = 3f;

    void Update()
    {
        Shoot();
        //float _rotationX = Mathf.Clamp(transform.rotation.eulerAngles.x, -90.0F, 0.0F);
        // transform.rotation = Quaternion.Euler(rotationX, transform.eulerAngles.y, transform.eulerAngles.z);
        Quaternion rotation = transform.rotation;
        rotation.y = Mathf.Clamp(transform.eulerAngles.y, 180f, 260f);
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, rotation.y, transform.rotation.eulerAngles.z);
    }
    public void Shoot()
    {
        if (Input.GetMouseButton(0))
        {
            transform.localEulerAngles += rotSpeed * new Vector3(0f, Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        }
    }
}