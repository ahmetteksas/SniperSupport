using System;
using System.Collections;
using System.Collections.Generic;
using Dreamteck.Splines;
using UnityEngine;

public class SplinePlayerController : MonoBehaviour
{
    public Animator anmtr;
    public float movementSpeed;
    public float sensivity = 0.01f;
    Vector2 firstMousePosition;
    Vector2 lastMousePosition;
    Vector2 deltaMousePosition;
    Vector2 movementVector;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            firstMousePosition = Input.mousePosition;
        }

        if (Input.GetMouseButton(0))
        {
            lastMousePosition = Input.mousePosition;
            deltaMousePosition = lastMousePosition - firstMousePosition;
            movementVector = deltaMousePosition * sensivity;
            Vector3 currentPos = transform.localPosition;
            
            float posX = Mathf.Clamp(transform.GetComponent<SplineFollower>().motion.offset.x + movementVector.x, -5f, 5f);
            transform.GetComponent<SplineFollower>().motion.offset = new Vector3(posX, 0, 0);

            firstMousePosition = lastMousePosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
            lastMousePosition = Input.mousePosition;
        }
    }

    public bool isHandGrabbing;
    public Transform leftHandTarget;
    public Transform rightHandTarget;
    
    private void OnAnimatorIK()
    {

        if (isHandGrabbing)
        {
            anmtr.SetIKPositionWeight(AvatarIKGoal.RightHand,1f);
            anmtr.SetIKPositionWeight(AvatarIKGoal.LeftHand,1f);

            anmtr.SetIKPosition(AvatarIKGoal.RightHand,rightHandTarget.transform.position);
            anmtr.SetIKPosition(AvatarIKGoal.LeftHand,leftHandTarget.transform.position);
            
        }
    }
}