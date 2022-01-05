using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowWithLerp : MonoBehaviour
{
    public FollowSettings settings;
    public Transform target;
    public Color myColor;
    public MeshRenderer myMesh;
    public MeshRenderer myMesh2;
    public SkinnedMeshRenderer myMesh3;
    private GameObject right;
    private GameObject left;
    private float t = 0;
    private float lerpTime = .8f;
    private float k = 0;
    private bool colored;
    public float startPosX;
    private float maxPosX = 2.5f;
    private void Awake()
    {
        if (settings == null)
            Debug.LogError("You need a settings for use this.");
        left = GameObject.FindGameObjectWithTag("PlayerLeft");
        right = GameObject.FindGameObjectWithTag("PlayerRight");

    }

    void Start()
    {
        //if (target != null)
        //    settings.offset = target.position - transform.position;
        //startTime = Time.time;
        startPosX = transform.position.x;
        
    }

    public void SetTarget(GameObject _target)
    {
        if (_target != null)
            target = _target.transform;
        
    }

    void FixedUpdate()
    {

        if (target &&this !=null) // XZ Plane follow
        {
            //if (MirrorMovement.crashed2 == false)
            //{
                Vector3 targetPosition = new Vector3(target.position.x + settings.offset.x, transform.position.y, target.position.z + settings.offset.z);
                transform.position = Vector3.Lerp(transform.position, targetPosition, settings.followLerp);
                //myMesh3.SetBlendShapeWeight(0, 100f - Mathf.Abs((transform.position.x - (0.35f * startPosX / Mathf.Abs(startPosX))) / maxPosX * 250f));
                float lerpValue = 100f - Mathf.Abs((transform.position.x - (0.35f * startPosX / Mathf.Abs(startPosX))) / maxPosX * 250f/*Mathf.Lerp(0, 100, t)*/);
            //myMesh.material.color = Color.Lerp(myColor, Color.red, lerpValue / 100f);
            //myMesh2.material.color = Color.Lerp(myColor, Color.red, lerpValue / 100f);
            if (MirrorMovement.crashed2 == false)
            {
                myMesh3.SetBlendShapeWeight(0, 100f - Mathf.Abs((transform.position.x - (0.35f * startPosX / Mathf.Abs(startPosX))) / maxPosX * 250f));
                myMesh.material.color = Color.Lerp(myColor, Color.red, lerpValue / 100f);
                myMesh2.material.color = Color.Lerp(myColor, Color.red, lerpValue / 100f);
            }
            if (MirrorMovement.crashed2 == true)
            {
                myMesh3.SetBlendShapeWeight(0, 10f);
                myMesh.material.color = myColor;
                myMesh2.material.color = myColor;
            }
            
                
        //}
        //else
        //{
        //    StartCoroutine(StopMov());
        //    //Vector3 targetPosition = new Vector3(target.position.x + settings.offset.x, transform.position.y, target.position.z + settings.offset.z);
        //    //transform.position = Vector3.Lerp(transform.position, new Vector3(0,targetPosition.y,targetPosition.z) /*targetPosition*/, settings.followLerp-.06f);
        //}
    }
    }
    IEnumerator StopMov()
    {
        Vector3 targetPosition = new Vector3(target.position.x + settings.offset.x, transform.position.y, target.position.z + settings.offset.z);
        transform.position = Vector3.Lerp(transform.position, new Vector3(0, targetPosition.y, targetPosition.z) /*targetPosition*/, settings.followLerp - .06f);
        yield return new WaitForSeconds(1f);
        StartCoroutine(StartMov());
        
    }
    IEnumerator StartMov()
    {
        yield return new WaitForSeconds(.1f);
        Vector3 targetPosition = new Vector3(target.position.x + settings.offset.x, transform.position.y, target.position.z + settings.offset.z);
        transform.position = Vector3.Lerp(transform.position, new Vector3(targetPosition.x, targetPosition.y, transform.position.z /*targetPosition.z*/) /*targetPosition*/, settings.followLerp );
    }
}
