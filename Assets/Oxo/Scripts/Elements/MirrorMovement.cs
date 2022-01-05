using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using ScriptableObjectArchitecture;
using DG.Tweening;
public class MirrorMovement : MonoBehaviour
{
    public List<GameObjectCollection> releatedColelctionList;
    public List<GameObjectCollection> rightList;

    public GameObject fail;

    private float dragOrigin;
    private float leftThreshold = -2.5f;
    private float rightThreshold = 2.5f;
    //public bool isLeftHand;
    private Rigidbody rigidbody;
    public float movement;

    public GameObject leftPlayer;
    public GameObject confetti1;
    public GameObject confetti2;
    public GameObject confetti3;

    bool isStunned;

    public float stunDelay;
    public float minCloseOffset;
    public static bool crashed2;
    private bool danced;

    private void Awake()
    {
        //rigidbody = GetComponent<Rigidbody>();
    }
    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();

        

    }
    private void Update()
    {
        //if (isLeftHand)
        //    HandleLeftMovement();
        //else
        if (!crashed2 && !danced)
        {
            HandleRightMovement();
        }
        else if (crashed2 && !danced)
        {
            StraightMovement();
        }
       
        foreach (var item in releatedColelctionList)
        {
            if (item.Count == 0 && !crashed2)
            {
                //fail.SetActive(true);
            }
        }

    }
    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "Finish" /*&& !crashed*//*&&isCollected*/)
        {
            //ObjectPool.instance.isGameRunning = false;
            crashed2 = true;


            //Destroy(other.gameObject);
            //foreach (var item in releatedColelctionList.FirstOrDefault())
            //{
            //    if (!item.gameObject.activeInHierarchy)
            //        return;
            //    releatedColelctionList[1].Add(item);
            //    releatedColelctionList[0].Remove(item);
            //}
            //releatedColelctionList.LastOrDefault(). = releatedColelctionList.FirstOrDefault().OrderByDescending(x => x.transform.position.z);
            //ReArrangeFollowers();

        }
        if (other.gameObject.tag == "Dance")
        {
            danced = true;
            List<FollowWithLerp> go = new List<FollowWithLerp>();
            List<StackCollectable> st = new List<StackCollectable>();

            st = FindObjectsOfType<StackCollectable>().ToList();
            go = FindObjectsOfType<FollowWithLerp>().ToList();
            foreach (var item in go)
            {
                Destroy(item);
            }
            StartCoroutine(OrderEggs());
            confetti1.SetActive(true);
            confetti2.SetActive(true);
            confetti3.SetActive(true);
            //foreach (var item2 in st)
            //{
            //    Destroy(item2);
            //}

            //go.Add(followWithLerp);
            //crashed2 = false;
            //Debug.Log("Danced");
        }
        else
        {
            //crashed2 = false;
        }
    }
    //void ReArrangeFollowers ()
    //{
    //    foreach (var item in releatedColelctionList.LastOrDefault())
    //    {
    //        if (releatedColelctionList.LastOrDefault().IndexOf(item)==0)
    //        {
    //            item.GetComponent<FollowWithLerp>().SetTarget(gameObject);
    //        }
    //        else
    //        {
    //            item.GetComponent<FollowWithLerp>().SetTarget
    //                (releatedColelctionList.LastOrDefault()[releatedColelctionList.LastOrDefault().IndexOf(item)-1]);
    //        }
            
    //    }
    //    //ObjectPool.instance.isGameRunning = true;
    //}
    private void FixedUpdate()
    {

      
        rigidbody.velocity = Vector3.forward * movement;
        //rigidbody.velocity = Vector3.down * movement/4;
        if (crashed2)
        {
            leftPlayer.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - .6f);
        }
        else
        {
            leftPlayer.transform.position = new Vector3(-transform.position.x, transform.position.y, transform.position.z);
        }

        
    }
    private void StraightMovement()
    {
        if (Input.GetMouseButtonDown(0))
        {
            dragOrigin = Input.mousePosition.x;
        }

        if (Input.GetMouseButton(0))
        {
            float moveAmount = (Input.mousePosition.x - dragOrigin);
            transform.localPosition = new Vector3(transform.localPosition.x + moveAmount / 110, transform.localPosition.y, transform.localPosition.z);

            if (transform.localPosition.x >= rightThreshold)
            {
                transform.localPosition = new Vector3(rightThreshold, transform.localPosition.y, transform.localPosition.z);
            }
            else if (transform.localPosition.x <= leftThreshold)
            {
                if (isStunned)
                {
                    transform.localPosition = new Vector3(-2.3f, transform.localPosition.y, transform.localPosition.z);
                }
                else
                {
                    transform.localPosition = new Vector3(leftThreshold, transform.localPosition.y, transform.localPosition.z);
                }
            }

            dragOrigin = Input.mousePosition.x;
        }
    }
    private void HandleRightMovement()
    {
        if (Input.GetMouseButtonDown(0))
        {
            dragOrigin = Input.mousePosition.x;
        }

        if (Input.GetMouseButton(0))
        {
            float moveAmount = (Input.mousePosition.x - dragOrigin);


            if (isStunned)
            {
                if (transform.localPosition.x < minCloseOffset)
                {
                    transform.localPosition = new Vector3(minCloseOffset, transform.localPosition.y, transform.localPosition.z);
                }

                //if (moveAmount < 0 && transform.localPosition.x <= minCloseOffset)
                //{
                //    Debug.Log("move amount" + moveAmount + " lpX : " + transform.localPosition.x);
                //    return;
                //}
            }

            transform.localPosition = new Vector3(transform.localPosition.x + moveAmount / 110, transform.localPosition.y, transform.localPosition.z);

            if (transform.localPosition.x >= rightThreshold)
            {
                transform.localPosition = new Vector3(rightThreshold, transform.localPosition.y, transform.localPosition.z);
            }
            else if (transform.localPosition.x <= .28f)
            {
                if (isStunned)
                {
                    transform.localPosition = new Vector3(.3f, transform.localPosition.y, transform.localPosition.z);
                }
                else
                {
                    transform.localPosition = new Vector3(.28f, transform.localPosition.y, transform.localPosition.z);
                }
            }

            dragOrigin = Input.mousePosition.x;
        }
    }

    public void PlayerStunned()
    {
        isStunned = true;
        transform.localPosition = new Vector3(minCloseOffset, transform.localPosition.y, transform.localPosition.z);
        StartCoroutine(SolveStun());
    }

    public void StartGame()
    {
        rigidbody.isKinematic = false;
    }

    IEnumerator SolveStun()
    {
        yield return new WaitForSeconds(stunDelay);
        isStunned = false;
    }
    IEnumerator OrderEggs()
    {
        rigidbody.isKinematic = true;
        yield return new WaitForSeconds(.2f);

        foreach (var item in rightList.FirstOrDefault())
        {
            Collider _co = item.GetComponent<Collider>();
            _co.isTrigger = true;
            Rigidbody _rb = item.GetComponent<Rigidbody>();
            _rb.isKinematic = true;
            item.transform.DOMove(new Vector3(Random.Range(-3f, 3f), 0, 275.5f), 1.5f); /*(item.transform.position, new Vector3( Random.Range(-3f, 3f), 0, 68f),.2f);*/
            StartCoroutine(DanceMovement(item));

        }
    }
    IEnumerator DanceMovement(GameObject item)
    {
        while (true)
        {
            yield return item.transform.DORotate( new Vector3(0, 180, 30f), .8f).WaitForCompletion();
            yield return item.transform.DORotate(new Vector3(0, 180, -30f), .8f).WaitForCompletion();
        }
    }
}
