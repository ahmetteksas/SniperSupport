using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hammer : MonoBehaviour
{

    public float rotFl = 10f;
    private float t = 0f;

    void Update()
    {
        t = Mathf.PingPong(Time.time * rotFl, 1f);

        if (transform.position.x > 0)
            transform.rotation = Quaternion.Slerp(Quaternion.Euler(270, -90, 0), Quaternion.Euler(180, -90, 0), t);
        else
            transform.rotation = Quaternion.Slerp(Quaternion.Euler(270, 90, 0), Quaternion.Euler(180, 90, 0), t);
    }
}
