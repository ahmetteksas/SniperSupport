using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public void Throw()
    {

        StartCoroutine(ThrowEnum());
    }

    IEnumerator ThrowEnum()
    {
        yield return new WaitForSeconds(.1f);
        transform.SetParent(null);
        Rigidbody _rigidbody = gameObject.AddComponent<Rigidbody>();
        _rigidbody.AddForce(transform.forward * 10f);
        MeshCollider _meshCollider = gameObject.AddComponent<MeshCollider>();
        _meshCollider.convex = true;
    }
}
