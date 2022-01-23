using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Dreamteck.Splines;
using UnityEngine;

public class ShootTheCrowdMechanic : MonoBehaviour
{

    public GameObject ThrowPrefab;
    public SplineComputer _splineComputer;
    public SplineFollower playerFollower;
    public SplineFollower endPos;
    private Vector3 firstMousePos;
    private Vector3 lastMousePOS;
    public SplinePositioner[] LinePointCount;
    private Vector3 delta;
    public LineRenderer LineRenderer;
    public float heightMultiplier;
    public AnimationCurve heightCurve;
    
    
    
    
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            firstMousePos = Input.mousePosition;
            playerFollower.follow = false;
            endPos.SetPercent(_splineComputer.Project(transform.position).percent);
            endPos.follow = true;
        }
        if (Input.GetMouseButton(0))
        {
            lastMousePOS = Input.mousePosition;
            Vector3[] positions=new Vector3[LinePointCount.Length];
            float length = _splineComputer.CalculateLength(_splineComputer.Project(transform.position).percent,_splineComputer.Project(endPos.transform.position).percent);
            float maxdistance = _splineComputer.CalculateLength();
            float distanceBetween =length / (float)LinePointCount.Length;
            float distance = 0;
            for (int i = 0; i < LinePointCount.Length; i++)
            {
                distance += distanceBetween;
                float temp = (distance) / maxdistance;
   
                LinePointCount[i].SetPercent(_splineComputer.Project(transform.position).percent+temp);
                positions[i] = LinePointCount[i].transform.position;
            }
            length =  heightCurve[heightCurve.length-1].time;
            distanceBetween = length / positions.Length;
            distance = 0;
            for (int i = 0; i < positions.Length; i++)
            {
                distance += distanceBetween;
                positions[i] = positions[i] + Vector3.up * heightCurve.Evaluate(distance)*heightMultiplier;
            }
            List<Vector3> newpoints= new List<Vector3>();
            newpoints.Add(transform.position);
            for (int i = 0; i < positions.Length; i++)
            {
                newpoints.Add(positions[i]);
            }
            //newpoints.Add(endPos.transform.position);
            LineRenderer.positionCount = newpoints.Count;
            LineRenderer.SetPositions(newpoints.ToArray());
        }
        if (Input.GetMouseButtonUp(0))
        {
            StartCoroutine(Throw());
            
        }

    }


    public IEnumerator Throw()
    {
        endPos.follow = false;
        
        GameObject newObj = Instantiate(ThrowPrefab);
        newObj.transform.position = playerFollower.transform.position;
        List<Vector3> newpos = new List<Vector3>();
        for (int i = 0; i < LineRenderer.positionCount; i++)
        {
            newpos.Add(LineRenderer.GetPosition(i));
        }
        newObj.GetComponent<ThrowableDisplay>().positions=newpos;
        StartCoroutine(newObj.GetComponent<ThrowableDisplay>().Throw());
                    
        yield return new WaitForSeconds(1f);
        playerFollower.follow = true;
        
    }
}
