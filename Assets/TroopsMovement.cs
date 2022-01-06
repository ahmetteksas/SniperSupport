using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TroopsMovement : MonoBehaviour
{
    public Transform dest;
    NavMeshAgent nMesh;
    public float troopsFirstSpeed;
    void Start()
    {
        nMesh = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        nMesh.speed = troopsFirstSpeed;
        nMesh.destination = dest.position;
    }
}
