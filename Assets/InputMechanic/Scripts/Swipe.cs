using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Oxo GAMES/Mechanics/Swipe", 0)]

public class Swipe : MonoBehaviour
{
    bool canMultiSwipe;
    private float swipeDetectDistance;
    private bool isSwipeDone;
    private int currentIndex;
    private float sensitivity = 0.008f;

    Vector2 firstMousePosition;
    Vector2 lastMousePosition;
    Vector2 deltaMousePosition;
    Vector2 movementVector;
    [Header(" ")]
    [Header("Your Swipe Setting")]

    public SwipeSetting settings;

    #region Actions

    public event Action OnSwipeRight;
    public event Action OnSwipeLeft;
    public event Action OnSwipeUp;
    public event Action OnSwipeDown;

    #endregion
    private void Awake()
    {
        if (settings != null)
        {
            sensitivity = settings.sensivity;
            canMultiSwipe = settings.canMultiSwipe;
            swipeDetectDistance = settings.swipeDetectDistance;
        }
        else
        {
            Debug.Log("You need swerve settings for use this.");
        }
    }

    void Update()
    {
        #region MobileInputCheck
        if (Input.GetMouseButtonDown(0)) FingerDown();
        if (Input.GetMouseButton(0)) FingerDrag();
        if (Input.GetMouseButtonUp(0)) FingerUp();
        #endregion
    }

    private void FingerUp()
    {

    }

    private void FingerDown()
    {
        firstMousePosition = Input.mousePosition;
        isSwipeDone = false;
    }

    private void FingerDrag()
    {
        lastMousePosition = Input.mousePosition;

        deltaMousePosition = lastMousePosition - firstMousePosition;

        movementVector = deltaMousePosition * sensitivity;
        if (isSwipeDone)
            return;

        if (Mathf.Abs(deltaMousePosition.x) > Mathf.Abs(deltaMousePosition.y))
        {
            if (Mathf.Abs(deltaMousePosition.x) > swipeDetectDistance)
            {
                if (deltaMousePosition.x > 0)
                {
                    print("SwipeRight");
                    SwipeRight();
                }
                else
                {
                    print("SwipeLeft");
                    SwipeLeft();
                }

                if (canMultiSwipe)
                    firstMousePosition = lastMousePosition;
                else
                    isSwipeDone = true;
            }
        }
        else
        {
            if (Mathf.Abs(deltaMousePosition.y) > swipeDetectDistance /** (Screen.width / Screen.height)*/)
            {
                if (deltaMousePosition.y > 0)
                {
                    print("SwipeUp");
                    SwipeUp();
                }
                else
                {
                    print("SwipeDown");
                    SwipeDown();
                }

                if (canMultiSwipe)
                    firstMousePosition = lastMousePosition;
                else
                    isSwipeDone = true;
            }
        }
    }

    private void SwipeUp()
    {
        OnSwipeUp?.Invoke();
    }

    private void SwipeRight()
    {
        OnSwipeRight?.Invoke();
    }

    private void SwipeDown()
    {
        OnSwipeDown?.Invoke();
    }

    private void SwipeLeft()
    {
        OnSwipeRight?.Invoke();
    }

}
