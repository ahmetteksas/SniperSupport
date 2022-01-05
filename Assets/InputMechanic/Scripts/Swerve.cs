using ScriptableObjectArchitecture;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro; 
[AddComponentMenu("Oxo GAMES/Mechanics/Swerve", 0)]
[RequireComponent(typeof(Rigidbody))]
public class Swerve : MonoBehaviour
{


    public List<GameObjectCollection> releatedColelctionList;

    //Use screen size stabilizer
    //can use max rot system
    //close all sliders when you write sensitivity behaviour
    //use rigidbody mass stabilizer

    public SwerveSetting settings;

    #region Settings Variables

    bool isSensitivityStabilize;

    private LayerMask detectMask;

    bool crashed;

    [Header("Character")]
    //private float movementSpeed = 5;
    //private float topSpeed = 7;
    //private int stabilizeIteration = 10; // high value work like lerp

    #endregion

    #region Input Variables

    private bool isSwerveStoped;
    private Vector2 firstMousePosition;
    private Vector2 lastMousePosition;
    private Vector2 deltaMousePosition;
    private Vector2 movementVector;
    private float tempSensitivity;

    #endregion

    #region Controll Variables

    private Rigidbody rigidbody;
    private Transform controlRoot;
    private Quaternion targetRot;

    private GameObject rotationStabilizer;
    private Vector3 rotationCorePoint;
    private Quaternion rotationStabilizerTargetRot;

    public TextMeshProUGUI points;
    public int pointCounter;

    #endregion

    #region Ray Variables

    private RaycastHit leftHit;
    private RaycastHit rightHit;
    private RaycastHit stabilizerLeftHit;
    private RaycastHit stabilizerRightHit;
    private float firstHitLenght = 0;
    private float currentHitLenght;

    #endregion

    #region Constants

    float stopDetectDistance = 0.5f; // it just Control
    float eulerCalculationExp = 5f; //used for calculate target rot for path
    float pathRotationLerp = .03f; //Path rotation fix lerp value
    float swerveRotationLerp = .06f; //Character root rotation fix lerp value
    float minSwerveSpeed = 55f;
    float maxSwerveSpeed = 2500f;
    float currentPosCalculateExp = 20;
    
    #endregion

    #region Initialize

    private void Awake()
    {
        if (settings == null)
            Debug.LogError("You need a settings for use this.");

        rigidbody = GetComponent<Rigidbody>();
        //Getting swerveSettings on the Swerve Mechanic.

        tempSensitivity = settings.sensitivity;
    }

    private void Start()
    {
        rotationStabilizer = new GameObject();
        rotationStabilizer.name = "RotationStabilizer";
        rotationStabilizer.transform.position = transform.position;
        rotationStabilizer.transform.rotation = transform.rotation;

        /* All rigs adding controllRoot for if you want rotate on the rig not mainParent. So thats stuffs adding childs one parent*/
        GameObject go = new GameObject();
        controlRoot = go.transform;
        controlRoot.gameObject.name = "controlRoot";

        controlRoot.position = transform.position;
        controlRoot.rotation = transform.rotation;

        int childCount = transform.childCount;

        for (int i = 0; i < childCount; i++)
            transform.GetChild(0).SetParent(controlRoot);//LOOK HERE

        controlRoot.SetParent(transform);

        StartCoroutine(StabilizePlatformRotation());
        //points.text = pointCounter.ToString();
    }

    #endregion

    
    void Update()
    {
        #region MobileInputCheck

        if (Input.GetMouseButtonDown(0)) FingerDownAction();
        if (Input.GetMouseButton(0)) FingerDragAction();
        if (Input.GetMouseButtonUp(0)) FingerUpAction();

        #endregion

        StabilizeControlRootRotation(deltaMousePosition.x);

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Finish"))
        {
            crashed = true;
        }
        if (other.gameObject.CompareTag("CollectablePoints"))
        {
            pointCounter += 10;
            Destroy(other.gameObject);
            if (points !=null)
            {
                points.text = pointCounter.ToString();
            }
            
        }
    }
    private void FixedUpdate()
    {
        return;
        // If my finger is not on the screen 
        if (isSwerveStoped)
        {
            /* If mySpeed is low than topSpeed*/
            if (rigidbody.velocity.magnitude < settings.topSpeed)
            {
                rigidbody.AddForce(transform.forward * settings.movementSpeed /*(+ movementSpeedSlider.value)*/);
            }
            //Calling swerveStop because isSwerveStoped= true, my finger is not on the screen.
            SwerveStopped();
        }
    }

    #region FingerActions

    private void FingerDownAction()
    {
        firstMousePosition = Input.mousePosition;
    }

    private void FingerDragAction()
    {
        return;
        #region DragDefaults

        lastMousePosition = Input.mousePosition;

        deltaMousePosition = lastMousePosition - firstMousePosition;

        if (deltaMousePosition.magnitude < stopDetectDistance)
        {
            SwerveStopped();
        }
        else
        {
            isSwerveStoped = false;
        }

        #endregion


        if (deltaMousePosition.x < 0 && isStunned)
        {
            return;
        }

        movementVector = deltaMousePosition * settings.sensitivity /*(+ sensitivitySlider.value)*/;

        if (movementVector.x != 0)
        {
            if (Mathf.Abs(movementVector.x) < minSwerveSpeed)
            {
                movementVector.x *= (minSwerveSpeed / Mathf.Abs(movementVector.x));
            }
        }

        Vector3 currentPos = transform.localPosition;
        float targetPosX = currentPos.x + movementVector.x;

        currentPos = transform.right * movementVector.x * rigidbody.mass /** currentPosCalculateExp*/;

        if (currentPos.x != 0)
            StartCoroutine(StabilizeSwerveForce(currentPos));

        /*Swerve Rotation Calculation*/
        float rot = deltaMousePosition.x * settings.rotationSensitivity/*(+ rotationSensitivitySlider.value)*/;

        if (Math.Abs(rot) > settings.maxSwerveRotation)
            rot = rot / Math.Abs(rot) * settings.maxSwerveRotation;

        targetRot.eulerAngles = new Vector3(0f, rot, 0f);

        firstMousePosition = lastMousePosition;
    }

    private void FingerUpAction()
    {
        SwerveStopped();
    }

    #endregion

    bool forceStabilizeWorking;

    IEnumerator StabilizeSwerveForce(Vector3 _force)
    {
        //if (smoothStop.isOn)
        forceStabilizeWorking = true;


        SetSpeed();

        for (int i = 0; i < settings.stabilizeIteration; i++)
        {
            rigidbody.AddForce(_force / (settings.stabilizeIteration));
            yield return new WaitForFixedUpdate();
        }

        forceStabilizeWorking = false;
    }

    public void SetSpeed()
    {
        rigidbody.velocity = transform.forward * (settings.movementSpeed);
    }

    private void SwerveStopped()
    {
        StartCoroutine(SwerveStopEnum());
    }

    IEnumerator SwerveStopEnum()
    {
        while (forceStabilizeWorking)
        {
            yield return new WaitForFixedUpdate();
        }

        isSwerveStoped = true;

        rigidbody.velocity = Vector3.Lerp(rigidbody.velocity,
            transform.forward * settings.movementSpeed +
            new Vector3(rigidbody.velocity.x * 0f, rigidbody.velocity.y, rigidbody.velocity.z * 0f),
            1f); // can stop with lerp but force iteration count should decreace to use this
        targetRot = Quaternion.identity;
    }

    void StabilizeControlRootRotation(float distance)
    {
        //Checking for use RootControl (parent for rotation)
        if (settings.isRootControl)
        {
            //Which one you are choose ? Torque or Quaternion.Lerp (Physic or Lerp)

            //Torque
            if (settings.isPhysic)
            {
                if ((settings.rotationSensitivity /*+ rotationSensitivitySlider.value*/) != 0 && Mathf.Abs(distance) > 0.5f &&
                    !isSwerveStoped)
                {
                    rigidbody.AddTorque(new Vector3(0, distance * settings.rotationValue /*(+ rotationValueSlider.value)*/, 0));
                }
                else
                {
                    transform.rotation =
                        Quaternion.Slerp(transform.rotation, rotationStabilizer.transform.rotation, 0.05f);
                }
            }
            //If you choose Quaternion Lerp rotate with RootControll = (Parent of rigs) || Is not physics
            else
            {
                if (settings.rotationSensitivity /*(+ rotationSensitivitySlider.value)*/ != 0)
                {
                    controlRoot.transform.localRotation =
                        Quaternion.Lerp(controlRoot.transform.localRotation, targetRot,
                            swerveRotationLerp); // need 1 / rotation stabilize iteratio
                    transform.rotation = rotationStabilizer.transform.rotation;
                }
            }
        }
        //Rotate with (Rotating Main GameObject not rig) not rootControll(Rigs parent) 
        else
        {
            Quaternion targerQuat =
                Quaternion.Euler(targetRot.eulerAngles + rotationStabilizer.transform.rotation.eulerAngles);

            if (settings.rotationSensitivity /*(+ rotationSensitivitySlider.value)*/ != 0)
                transform.localRotation =
                    Quaternion.Lerp(transform.localRotation, targerQuat,
                        swerveRotationLerp); // need 1 / rotation stabilize iteratio
        }
    }

    IEnumerator StabilizePlatformRotation()
    {
        yield return new WaitForSeconds(2f);
        while (true) // need game running control
        {
            #region PlayerRaycast

            RaycastHit hit;

            if (Physics.Raycast(transform.position + Vector3.up, transform.TransformDirection(Vector3.right), out hit,
                Mathf.Infinity, detectMask))
            {
                rightHit = hit;
                Debug.DrawRay(transform.position + Vector3.up,
                    transform.TransformDirection(Vector3.right) * hit.distance, Color.red);
            }
            else
            {
                //Debug.Log("Did not Hit");
            }

            if (Physics.Raycast(transform.position + Vector3.up, transform.TransformDirection(Vector3.left), out hit,
                Mathf.Infinity, detectMask))
            {
                leftHit = hit;
                Debug.DrawRay(transform.position + Vector3.up,
                    transform.TransformDirection(Vector3.left) * hit.distance, Color.red);
                //Debug.Log("Did Hit Left");
            }
            else
            {
                //Debug.Log("Did not Hit");
            }

            #endregion

            rotationCorePoint = (rightHit.point + leftHit.point) / 2f;
            rotationStabilizer.transform.position = rotationCorePoint;

            #region StabilizerRaycast

            RaycastHit hitStabilizer;

            if (Physics.Raycast(rotationStabilizer.transform.position,
                rotationStabilizer.transform.TransformDirection(Vector3.right + Vector3.forward * 2f),
                out hitStabilizer, Mathf.Infinity, detectMask))
            {
                stabilizerRightHit = hitStabilizer;
                Debug.DrawRay(rotationStabilizer.transform.position,
                    rotationStabilizer.transform.TransformDirection(Vector3.right + Vector3.forward * 2f) *
                    hitStabilizer.distance, Color.red);
            }
            else
            {
                //Debug.Log("Did not Hit");
            }

            if (Physics.Raycast(rotationStabilizer.transform.position,
                rotationStabilizer.transform.TransformDirection(Vector3.left + Vector3.forward * 2f), out hitStabilizer,
                Mathf.Infinity, detectMask))
            {
                stabilizerLeftHit = hitStabilizer;
                Debug.DrawRay(rotationStabilizer.transform.position,
                    rotationStabilizer.transform.TransformDirection(Vector3.left + Vector3.forward * 2f) *
                    hitStabilizer.distance, Color.red);
            }
            else
            {
                //Debug.Log("Did not Hit");
            }

            #endregion

            float stabilizerDifference = stabilizerRightHit.distance - stabilizerLeftHit.distance;

            rotationStabilizerTargetRot = rotationStabilizer.transform.rotation;

            rotationStabilizerTargetRot.eulerAngles = rotationStabilizerTargetRot.eulerAngles +
                                                      (Vector3.up * stabilizerDifference * eulerCalculationExp);

            rotationStabilizer.transform.rotation = Quaternion.Slerp(rotationStabilizer.transform.rotation,
                rotationStabilizerTargetRot, pathRotationLerp);

            if (isSensitivityStabilize)
                StabilizeSensitivity();

            yield return null;
        }
    }

    void StabilizeSensitivity()
    {
        if (firstHitLenght == 0)
        {
            firstHitLenght = rightHit.distance + leftHit.distance;
        }
        else
        {
            currentHitLenght = rightHit.distance + leftHit.distance;
            settings.sensitivity = (currentHitLenght / firstHitLenght) * tempSensitivity;
        }
    }

    bool isStunned;

    private void OnCollisionEnter(Collision collision)
    {
        return;
        if (collision.transform.tag.Contains("Player"))
        {
            if (!isStunned)
            {
                isStunned = true;
                if (!crashed)
                {
                    transform.position = new Vector3((transform.position.x / transform.position.x) * -0.35f, transform.position.y, transform.position.z);
                    //collision.transform.position = new Vector3((collision.transform.position.x / collision.transform.position.x) * 0.35f, collision.transform.position.y, collision.transform.position.z);
                    Debug.Log("Hitted");
                    StartCoroutine(SolveStun());
                    HitThem();
                }
                
            }
        }
    }

    void HitThem()
    {
        foreach (var item in releatedColelctionList)
        {
            item.FirstOrDefault().GetComponent<StackCollectable>().Lost();
        }
    }

    IEnumerator SolveStun()
    {
        yield return new WaitForSeconds(3f);
        isStunned = false;
    }
}