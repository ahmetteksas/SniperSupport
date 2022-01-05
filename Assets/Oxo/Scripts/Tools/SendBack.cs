using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendBack : MonoBehaviour
{

    public Vector3 basePos;

    private void OnTriggerEnter(Collider other)
    {
        other.transform.position = basePos;
        other.transform.rotation = Quaternion.identity;
    }
}
