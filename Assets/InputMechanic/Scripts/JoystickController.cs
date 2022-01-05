using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class JoystickController : MonoBehaviour
{
    public VariableJoystick variableJoystick;

    private bool isTouch;
    public float speed;
    private Rigidbody rb;

    #region Sliders

    public Toggle smoothStop;
    public Slider sensitivitySlider;
    public Slider rotationSensitivitySlider;
    public Slider lerpIterationCountSlider;
    public Slider movementSpeedSlider;
    public Slider rotationValueSlider;

    #endregion

    #region Settings Variables

    public JoystickSettings settings;
    
    //private float maxRotationOffset;
    bool isSensitivityStabilize;
    bool isRootControl;
    private bool isPhysic;
    private float stopDetectDistance = 1; // it just Control
    private float sensitivity = 0.008f;
    private float rotationSensitivity = 7; // zero mean no rotation control
    private float maxSwerveRotation = 30;
    private float rotationValue = 0.2f; // zero mean no rotation control

    private LayerMask detectMask;

    [Header("Character")] private float movementSpeed = 5;
    private float topSpeed = 7;
    private int stabilizeIteration = 10; // high value work like lerp

    #endregion

    

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        if (settings != null)
        {
            isRootControl = settings.isRootControl;
            movementSpeed = settings.movementSpeed;
            topSpeed = settings.topSpeed;
            rotationSensitivity = settings.rotationSensitivity;
            detectMask = settings.detectMask;
            speed = settings.movementSpeed;
            
            // sensitivitySlider = CanvasManager.instance.sensitivitySlider;
            // rotationSensitivitySlider = CanvasManager.instance.rotationSensitivitySlider;
            // lerpIterationCountSlider = CanvasManager.instance.lerpIterationCountSlider;
            // movementSpeedSlider = CanvasManager.instance.movementSpeedSlider;
            // rotationValueSlider = CanvasManager.instance.rotationValueSlider;
            // smoothStop = CanvasManager.instance.smoothStop;
        }
    }

    void Start()
    {
    }

    private void Update()
    {
        // if (!GameManager.instance.isGameRunning)
        //     return;

        #region MobileInputCheck

        if (Input.GetMouseButtonDown(0)) FingerDownAction();
        if (Input.GetMouseButton(0)) FingerDragAction();
        if (Input.GetMouseButtonUp(0)) FingerUpAction();

        #endregion
    }

    public void FixedUpdate()
    {
        if (isTouch)
        {
            Vector3 direction = Vector3.forward * variableJoystick.Vertical +
                                Vector3.right * variableJoystick.Horizontal;
            rb.AddForce(direction * speed * Time.fixedDeltaTime, ForceMode.VelocityChange);

            if (Mathf.Abs(
                    Mathf.Atan2(variableJoystick.Horizontal, variableJoystick.Vertical) * 180 / Mathf.PI) -
                transform.eulerAngles.y > 90)
                rb.velocity = rb.velocity = Vector3.Lerp(rb.velocity, Vector3.zero, 0.05f);

            transform.DORotate(new Vector3(0,
                    Mathf.Atan2(variableJoystick.Horizontal, variableJoystick.Vertical) * 180 / Mathf.PI, 0),
                1 / rotationSensitivity);

            // transform.eulerAngles = new Vector3(0,
            //     Mathf.Atan2(variableJoystick.Horizontal, variableJoystick.Vertical) * 180 / Mathf.PI, 0);
        }
        else
        {
            rb.velocity = Vector3.Lerp(rb.velocity, Vector3.zero, 0.05f);
        }
    }

    private void FingerDownAction()
    {
        isTouch = true;
    }

    private void FingerDragAction()
    {
    }

    private void FingerUpAction()
    {
        isTouch = false;
    }
}