using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerDisplay : MonoBehaviour
{
    public MechanicType mechanic;

    public float movementSpeed;
    public float sensivity = 0.01f;

    bool isDead;

    public GameObject cross;

    public Swipe swipe;

    //[Header("Joystick Constants")]


    [Header("Swipe Constants")]
    [SerializeField]
    bool isMultiSwipe;
    [SerializeField]
    float swipeDetectDistance;
    bool isSwipeDone;
    public int currentIndex;
    [Serializable]
    public class Position
    {
        public int index;
        public Vector2 position;
    }
    public List<Position> Positions = new List<Position>();
    [Header("Tap To Aim Constants")]
    [SerializeField]
    string targetTag;
    [SerializeField]
    LayerMask layerMask;

    #region Private

    AnimatorDisplay _animatorDisplay;

    #endregion

    private void Awake()
    {
        _animatorDisplay = GetComponent<AnimatorDisplay>();

        cross = cross == null ? cross = gameObject : null;
    }

    void Update()
    {
        //if (!GameManager.instance.isGameRunning)
        //    return;

        #region MobileInputCheck
        if (Input.GetMouseButtonDown(0)) FingerDown();
        if (Input.GetMouseButton(0)) FingerDrag();
        if (Input.GetMouseButtonUp(0)) FinerUp();
        #endregion
    }

    #region MobileInputFunctions

    Vector2 firstMousePosition;
    Vector2 lastMousePosition;
    Vector2 deltaMousePosition;
    Vector2 movementVector;

    void FingerDown()
    {
        firstMousePosition = Input.mousePosition;

        if (mechanic == MechanicType.Joystick)
        {
            if (cross != null) //Character Control
            {
                _animatorDisplay.ForwardMovement(movementSpeed);
            }
            else //Aim Control
            {

            }
        }
        else if (mechanic == MechanicType.Swipe)
        {
            isSwipeDone = false;
        }
        else if (mechanic == MechanicType.TapTiming)
        {
            TapAction();
        }
        else if (mechanic == MechanicType.TapToAim)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
            {
                if (hit.collider.tag == targetTag)
                {
                    print("Target Hitted");
                    Shoot(hit.point);
                    //Transform objectHit = hit.transform;
                }
                // Do something with the object that was hit by the raycast.
            }
        }
    }

    void FingerDrag()
    {
        lastMousePosition = Input.mousePosition;

        deltaMousePosition = lastMousePosition - firstMousePosition;

        movementVector = deltaMousePosition * sensivity;

        if (mechanic == MechanicType.Swerve)
        {
            Vector3 currentPos = transform.position;

            float posX = Mathf.Clamp(currentPos.x + movementVector.x, -2.5f, 2.5f); // look here

            transform.position = new Vector3(posX, currentPos.y, currentPos.z);

            firstMousePosition = lastMousePosition;
        }
        else if (mechanic == MechanicType.Joystick)
        {
            if (cross != null) //Character Control
            {
                movementVector = movementVector.normalized;
                Vector3 movement = Quaternion.Euler(0,
                    Camera.main.transform.eulerAngles.y, 0) * new Vector3(movementVector.x * sensivity * Time.deltaTime, 0, movementVector.y * sensivity * Time.deltaTime);
                transform.LookAt(transform.position + movement);
            }
            else //Aim Control
            {

            }
        }
        else if (mechanic == MechanicType.Swipe)
        {
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

                    if (isMultiSwipe)
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

                    if (isMultiSwipe)
                        firstMousePosition = lastMousePosition;
                    else
                        isSwipeDone = true;
                }
            }
        }
        else if (mechanic == MechanicType.TapHold)
        {
            HoldAction();
        }
    }

    void FinerUp()
    {
        if (mechanic == MechanicType.Joystick)
        {
            _animatorDisplay.ForwardMovement(0);
        }
    }

    #endregion

    #region SwipeFunctions

    void SwipeRight()
    {
        ++currentIndex;
        // TakeThePosition();
        _animatorDisplay.StartDodge(0);
    }

    void SwipeLeft()
    {
        --currentIndex;
        // TakeThePosition();
        _animatorDisplay.StartDodge(.5f);
    }

    void SwipeUp()
    {
        _animatorDisplay.StartDodge(1f);

    }

    void SwipeDown()
    {
        _animatorDisplay.StartSlide(1f);
    }


    #endregion

    void TapAction()
    {
        print("Trig Tap Action");
    }

    void HoldAction()
    {
        print("Holding");
    }

    void Shoot(Vector3 target)
    {
        print("go" + target);
    }

    void OnGameStart()
    {
        if (mechanic == MechanicType.Swerve)
        {
            _animatorDisplay.ForwardMovement(movementSpeed);
        }
        else if (mechanic == MechanicType.Joystick)
        {

        }
    }

    public enum MechanicType { Joystick, Swipe, Swerve, TapTiming, TapHold, TapDrag, TapToAim, FPS_Shooter }

}
