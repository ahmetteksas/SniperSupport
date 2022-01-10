using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DoPath : MonoBehaviour
{
    private NavMeshAgent nMesh;
    public Transform vehicleTarget;
    void Start()
    {
        nMesh = GetComponent<NavMeshAgent>();
        nMesh.destination = vehicleTarget.position;
        nMesh.speed = 2f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
