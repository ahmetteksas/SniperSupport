using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PingPong : MonoBehaviour
{
    public float speed = 5f;
    public float length = 3f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x,  Mathf.Lerp(transform.position.y, 1f, Mathf.PingPong(Time.time, 1)), transform.position.z);
    }
}
