using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class AimLineManager : MonoBehaviour
{
    public static AimLineManager instance;
    public Transform startPoint;
    public Transform peakPoint;
    public Transform endPoint;
    private Transform target;
    public LineRenderer _lineRenderer;
    public float vertexCount;
    public float point2YPosition;
    public List<Vector3> pointList = new List<Vector3>();
    public GameObject spherePrefab;
    public List<GameObject> sphereList;
    private float gravity;
    private float flightTime;
    private float height;

    private Vector3 initialVelocity;
    
    private Rigidbody _rigidBody;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }
    private void Start()
    {
        for (int i = 0; i < vertexCount; i++)
        {
            GameObject sphere = Instantiate(spherePrefab);
            sphere.SetActive(false);
            sphereList.Add(sphere);
        }
    }
    public void SetLine()
    {
        pointList.Clear();
        peakPoint.transform.position =
            new Vector3((startPoint.transform.position.x + endPoint.transform.position.x) / 2, point2YPosition,
                (startPoint.transform.position.z + endPoint.transform.position.z) / 2);
        for (float ratio = 0; ratio <= 1; ratio += 1 / vertexCount)
        {
            var tangent1 = Vector3.Lerp(startPoint.position, peakPoint.position, ratio);
            var tangent2 = Vector3.Lerp(peakPoint.position, endPoint.position, ratio);
            var curve = Vector3.Lerp(tangent1, tangent2, ratio);
            // force = peakPoint.position - startPoint.position;
            pointList.Add(curve);
        }
        _lineRenderer.positionCount = pointList.Count;
        _lineRenderer.SetPositions(pointList.ToArray());
        for (int i = 0; i < vertexCount; i++)
        {
            sphereList[i].SetActive(true);
            sphereList[i].transform.position = pointList[i];
        }
    }
    public void HideLine()
    {
        sphereList.ForEach(x => x.SetActive(false));
        _lineRenderer.positionCount = 0;
    }
    public void ResetEndPoint()
    {
        return;
        sphereList.ForEach(x => x.SetActive(false));
        endPoint.position = startPoint.position;
    }
    //public void Shoot()
    //{
    //    /// You need to call on the player.
    //    AimLineManager.instance.HideLine();
    //    target = AimLineManager.instance.endPoint;
    //    Physics.gravity = Vector3.up * gravity;
    //    float displacementY = target.position.y - transform.position.y;
    //    Vector3 displacementXZ = new Vector3(target.position.x - transform.position.x, 0,
    //        target.position.z - transform.position.z);
    //    float time = Mathf.Sqrt(-2 * height / gravity) + Mathf.Sqrt(2 * (displacementY - height) / gravity);
    //    Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * height);
    //    Vector3 velocityXZ = displacementXZ / time;
    //    initialVelocity = velocityXZ + velocityY * -Mathf.Sign(gravity);
    //    flightTime = time;
    //    _rigidBody.velocity = initialVelocity;
    //}
}