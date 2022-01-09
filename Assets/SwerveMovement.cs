using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwerveMovement : MonoBehaviour
{
    float rotSpeed = 3f;
    private void Start()
    {
        
    }
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            transform.localEulerAngles += rotSpeed * new Vector3(Input.GetAxis("Mouse X"), -Input.GetAxis("Mouse Y"), 0);
        }
    }
}
