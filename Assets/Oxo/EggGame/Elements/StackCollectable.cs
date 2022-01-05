using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using UnityEngine;
using ScriptableObjectArchitecture;
using System.Linq;
using MoreMountains.NiceVibrations;
using DG.Tweening;
using UnityEngine.UI;
using System;

public class StackCollectable : MonoBehaviour
{
    [SerializeField]
    GameEvent stunEvent = default(GameEvent);

    [SerializeField]
    bool isCollected;

    public List<GameObjectCollection> releatedColelctionList;

    public GameObject[] caps;
    private int capsIndex;
    public GameObject glass;

    public Renderer myRenderer;
    public Renderer myRenderer2;
    public Renderer myRenderer3;
    public Renderer myRenderer4;

    public GameObject crackedEgg;

    FollowWithLerp fwl;

    public FloatReference relatedFloatRight;
    public FloatReference relatedFloatLeft;

    public static bool crashedObstacle;
    private bool crashed;
    bool isStunned;
    bool crashtogether;




    private void Awake()
    {
        fwl = GetComponent<FollowWithLerp>();
        capsIndex = Random.Range(0, 5);
        relatedFloatRight.Value = 0;
        relatedFloatLeft.Value = 0;
        
    }
    private void OnApplicationQuit()
    {
        foreach (var item in releatedColelctionList)
        {
            item.Clear();
        }
    }
    private void Start()
    {
        if (isCollected)
        {
            if (transform.position.x < 0)
                releatedColelctionList.FirstOrDefault().Add(gameObject);
            else
                releatedColelctionList.LastOrDefault().Add(gameObject);
        }
    }
    private void OnEnable()
    {
        //if (isCollected)
        //{
        //    if (transform.position.x < 0)
        //        releatedColelctionList.FirstOrDefault().Add(gameObject);
        //    else
        //        releatedColelctionList.LastOrDefault().Add(gameObject);
        //}
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Obstacle" || crashed/**//*&&isCollected*/)
        {

            
            crashedObstacle = true;

            Destroy(other.gameObject);

            Lost();

        }

        if (other.gameObject.CompareTag("Cap"))
        {
           GameObject capNew =  caps[capsIndex] ;
            capNew.SetActive(true);
            //capNew.transform.DOMoveZ(55f, 1f);
            capNew.transform.DOMoveZ(capNew.transform.position.z +4.6f, .4f);
            //capNew.transform.DOMoveZ(transform.position.z, 1f);
            //capNew.transform.localPosition = Vector3.Lerp(capNew.transform.localPosition, new Vector3 ( capNew.transform.localPosition.x, capNew.transform.localPosition.y, capNew.transform.localPosition.z+40f), .5f);
            //caps[capsIndex].SetActive(true);
            Debug.Log("cap collected");
        }
        if (other.gameObject.CompareTag("Glass"))
        {
            glass.SetActive(true);
            //glass.transform.DOMoveZ(glass.transform.position.z + 5.25f, .4f);
            //glass.transform.DOMoveY(glass.transform.position.y - .35f, .4f);
        }
    }
   
    private void OnCollisionEnter(Collision other)
    {
        if (!isCollected)
            Collect(other.gameObject);


        if (other.gameObject.tag == "Obstacle" || crashed/*&&isCollected*/)
        {
            if (MirrorMovement.crashed2 == false)
            {
              
                crashedObstacle = true;

                Destroy(other.gameObject);
                Lost();
            }
            //StartCoroutine(CrashedTrue());

        }

        if (other.collider.CompareTag("Collectable") && !crashtogether)
        {

           
            bool done1 = false;
            bool done2 = false;


            foreach (var item in releatedColelctionList)
            {
                if (item.FirstOrDefault() == gameObject)
                {
                    done1 = true;
                }
                if (item.FirstOrDefault() == other.gameObject)
                {
                    done2 = true;
                }

                if (done1 && done2)
                {
                    if (MirrorMovement.crashed2 == false)
                    {

                        Debug.Log("hitted");
                        stunEvent.Raise();
                        Lost();
                        other.collider.GetComponent<StackCollectable>().Lost();
                        crashtogether = true;

                    }

                    //StartCoroutine(Crashed2True());

                }
            }

        }
    }

    private void Update()
    {
        if (/*myRenderer == null && myRenderer2 == null && myRenderer3 == null && myRenderer4 == null &&*/ isCollected)
        {
            myRenderer.enabled = true;
            myRenderer2.enabled = true;
            myRenderer3.enabled = true;
            myRenderer4.enabled = true;
        }
        if (gameObject.transform.position.y <= -.5f)
        {
            Fall();
        }
       
    }

    public void Collect(GameObject other)
    {
        if (other.CompareTag("PlayerLeft"))
        {
            isCollected = true;

            #region SelectFollowTarget
            if (releatedColelctionList.Count > 0)
                fwl.target = releatedColelctionList.FirstOrDefault().LastOrDefault().transform;
            else
                fwl.target = other.transform;
            #endregion

            releatedColelctionList.FirstOrDefault().Add(gameObject);
            relatedFloatLeft.Value++;
            MMVibrationManager.Haptic(HapticTypes.Success);
        }
        if (other.CompareTag("PlayerRight") )
        {

            isCollected = true;

            #region SelectFollowTarget
            if (releatedColelctionList.Count > 0)
                fwl.target = releatedColelctionList.LastOrDefault().LastOrDefault().transform;
            else
                fwl.target = other.transform;
            #endregion
            //Debug.Log("Multiply");
            releatedColelctionList.LastOrDefault().Add(gameObject);

            relatedFloatRight.Value++;
            MMVibrationManager.Haptic(HapticTypes.Success);
        }
        if (other.CompareTag("Player"))
        {
            isCollected = true;

            #region SelectFollowTarget
            if (releatedColelctionList.Count > 0)
                fwl.target = releatedColelctionList.FirstOrDefault().LastOrDefault().transform;
            else
                fwl.target = other.transform;
            #endregion

            releatedColelctionList.FirstOrDefault().Add(gameObject);

            //relatedFloat.Value++;
            MMVibrationManager.Haptic(HapticTypes.Success);
        }
    }

    private void OnDisable()
    {
        if (releatedColelctionList.FirstOrDefault().Contains(gameObject))
        {
            releatedColelctionList.FirstOrDefault().Remove(gameObject);
            relatedFloatLeft.Value--;
        }
        else if (releatedColelctionList.LastOrDefault().Contains(gameObject))
        {
            releatedColelctionList.LastOrDefault().Remove(gameObject);
            relatedFloatRight.Value--;
        }

    }

    public void Lost()
    {
        GameObject go2 = ObjectPool.instance.SpawnFromPool("EggForced", transform.position, Quaternion.Euler(0, 0, 0) /*Quaternion.identity*/);
        Rigidbody go2rb = go2.GetComponent<Rigidbody>();
        BoxCollider go2cl = go2.GetComponent<BoxCollider>();
        float _newZ2 = transform.position.z;
        if (transform.position.x <0f)
        {
            go2.transform.position = new Vector3(transform.position.x-.4f, transform.position.y + 1f, (transform.position.z + 2f));
        }
        if (transform.position.x > 0f)
        {
            go2.transform.position = new Vector3(transform.position.x + .4f, transform.position.y + 1f, (transform.position.z + 2f));
        }

        go2.transform.localEulerAngles = new Vector3(120, 0, 0);
        go2.transform.localScale = new Vector3(.6f, .6f, .6f);
        go2.SetActive(true);
        go2cl.isTrigger = true;
        go2rb.AddForce(Vector3.forward * 4f);
        GameObject go = ObjectPool.instance.SpawnFromPool("CrackedEgg", transform.position, Quaternion.Euler(0,0,0) /*Quaternion.identity*/);
        float _newZ = transform.position.z;

        go.transform.position = new Vector3(transform.position.x, transform.position.y+.1f, (transform.position.z + 2f));
        go.transform.localEulerAngles = new Vector3(0, 0, 0);
        go.SetActive(true);
        crashed = true;
        //StartCoroutine(SeperateEggs());
        //go = gameObject;
        gameObject.SetActive(false);
    }


    public void Fall()
    {
        if (releatedColelctionList.FirstOrDefault().Contains(gameObject))
        {
            releatedColelctionList.FirstOrDefault().Remove(gameObject);
            relatedFloatLeft.Value--;
        }
        else
        {
            releatedColelctionList.LastOrDefault().Remove(gameObject);
            relatedFloatRight.Value--;
        }
        StartCoroutine(FallObj());
    }
    IEnumerator FallObj()
    {
        yield return new WaitForSeconds(.5f);
        gameObject.SetActive(false);
    }
    IEnumerator CrashedTrue()
    {
        yield return new WaitForSeconds(1.5f);
        crashedObstacle = false;
    }
    IEnumerator Crashed2True()
    {

        yield return new WaitForSeconds(2f);
        crashtogether = false;
    }
    //IEnumerator  SeperateEggs()
    //{
    //    eggPart1.isKinematic = false;
    //    eggPart1.AddForce(Vector3.up * 40f);
    //    eggPart2.SetActive(false);
    //    yield return new WaitForSeconds(1.2f);
    //    gameObject.SetActive(false);
        
    //}   
    IEnumerator CrashLast()
    {
        //yield return new WaitForSeconds(1.5f);
        for (int i = 0; i < releatedColelctionList[1].Count; i++)
        {
            yield return new WaitForSeconds(.5f);
            Destroy(releatedColelctionList[i]);
        }
        //Destroy(gameObject);
    }
}
