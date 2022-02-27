using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MainCameraDisplay : MonoBehaviour
{

    [SerializeField] float sensitivity;
    Transform defaultParent;
    public float movementTime;


    private void Awake()
    {
        //defaultParent = transform.parent;
    }
    void Start()
    {
        transform.DOLocalMove(Vector3.zero, 0f);
        transform.DOLocalRotate(Vector3.zero, 0f);
    }

    Vector3 firstMousePosition;
    Vector3 tempMousePosition;
    Vector3 mouseDelta;

    void Update()
    {
        if (LevelManager.instance.isGameRunning)
            Swerve();
    }
    Vector3 targetEulerAngles;
    public void Swerve()
    {
        if (Input.GetMouseButtonDown(0))
            firstMousePosition = Input.mousePosition;

        if (Input.GetMouseButton(0))
        {

            tempMousePosition = Input.mousePosition;
            mouseDelta = tempMousePosition - firstMousePosition;
            transform.parent.Rotate(sensitivity * new Vector3(-mouseDelta.y * 2f, mouseDelta.x, 0f) * Time.deltaTime);

            //targetEulerAngles = transform.eulerAngles;
            //targetEulerAngles += sensitivity * new Vector3(-mouseDelta.y * 2, mouseDelta.x, -mouseDelta.y) * Time.deltaTime;
            //transform.rotation = Quaternion.Euler(targetEulerAngles);// Quaternion.Lerp(transform.localRotation, Quaternion.Euler(targetEulerAngles), lerpSensitivity);

            firstMousePosition = tempMousePosition;
        }
    }
}
