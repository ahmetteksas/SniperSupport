using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DoPath : MonoBehaviour
{
    private NavMeshAgent nMesh;
    public Transform vehicleTarget;
    private void Awake()
    {
        nMesh = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        nMesh.destination = vehicleTarget.position;
        nMesh.speed = 1f;
    }
}
