using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableDisplay : MonoBehaviour
{
    public List<Vector3> positions;

    public int currentindex;

    public float force;
    private Rigidbody _rigidbody;
    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
       // StartCoroutine(Throw());
    }

 

    public IEnumerator Throw()
    {
        print("sdsf");
        print(positions.Count);
        while (currentindex<positions.Count-1)
        {
            yield return new WaitForSeconds(0f);
            print(currentindex);
            print(Vector3.Distance(transform.position,positions[currentindex]));
            //_rigidbody.MovePosition(positions[currentindex]);
            _rigidbody.velocity=(positions[currentindex]-transform.position).normalized*force;
            if (Vector3.Distance(transform.position,positions[currentindex])<0.1f)
            {
                currentindex++;
            }
        }

        GetComponent<Collider>().enabled = true;
        print("end");
    }
}
