using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorItem : MonoBehaviour
{
    Camera mainCamera;

    [SerializeField] private float screenBoundOffset = 0.9f;

    private Vector3 screenCentre;
    private Vector3 screenBounds;

    [SerializeField] Transform targetTransform;


    private void Awake()
    {
        mainCamera = Camera.main;
        screenCentre = new Vector3(Screen.width, Screen.height, 0) / 2;
        screenBounds = screenCentre * screenBoundOffset;
        if (targetTransform == null)
        {
            Animator _animator = transform.parent.parent.GetComponentInChildren<Animator>();
            targetTransform = _animator.transform;
        }
    }

    void Update()
    {
        Vector3 screenPosition = mainCamera.WorldToScreenPoint(targetTransform.position);

        if (targetTransform.TryGetComponent(out Renderer _renderer))
            if (_renderer.isVisible)
            {
                gameObject.SetActive(false);
                return;
            }


        bool isTargetVisible = IsTargetVisible(screenPosition);

        if (isTargetVisible)
        {
            transform.DOScale(Vector3.zero, .1f);
            return;
        }
        else
        {
            transform.DOScale(Vector3.one * .3f, .2f);
        }

        //float distanceFromCamera = target.NeedDistanceText ? target.GetDistanceFromCamera(mainCamera.transform.position) : float.MinValue;// Gets the target distance from the camera.
        //Indicator indicator = null;

        //if (target.NeedBoxIndicator)
        //{
        //    screenPosition.z = 0;
        //    indicator = GetIndicator(ref target.indicator, IndicatorType.BOX); // Gets the box indicator from the pool.
        //}
        //else
        //if (target.NeedArrowIndicator && !isTargetVisible)
        //{
        float angle = float.MinValue;
        GetArrowIndicatorPositionAndAngle(ref screenPosition, ref angle, screenCentre, screenBounds);

        //indicator = GetIndicator(ref target.indicator, IndicatorType.ARROW); // Gets the arrow indicator from the pool.

        transform.rotation = Quaternion.Euler(0, 0, angle * Mathf.Rad2Deg); // Sets the rotation for the arrow indicator.
        //}
        //if (indicator)
        //{
        //indicator.SetImageColor(target.TargetColor);// Sets the image color of the indicator.
        //indicator.SetDistanceText(distanceFromCamera); //Set the distance text for the indicator.
        transform.position = screenPosition; //Sets the position of the indicator on the screen.
                                             //indicator.SetTextRotation(Quaternion.identity); // Sets the rotation of the distance text of the indicator.
                                             //}
    }


    public static bool IsTargetVisible(Vector3 screenPosition)
    {
        bool isTargetVisible = screenPosition.z > 0 && screenPosition.x > -2 && screenPosition.x < Screen.width && screenPosition.y > -2 && screenPosition.y < Screen.height;
        return isTargetVisible;
    }


    public static void GetArrowIndicatorPositionAndAngle(ref Vector3 screenPosition, ref float angle, Vector3 screenCentre, Vector3 screenBounds)
    {
        // Our screenPosition's origin is screen's bottom-left corner.
        // But we have to get the arrow's screenPosition and rotation with respect to screenCentre.
        screenPosition -= screenCentre;

        // When the targets are behind the camera their projections on the screen (WorldToScreenPoint) are inverted,
        // so just invert them.
        if (screenPosition.z < 0)
        {
            screenPosition *= -1;
        }

        // Angle between the x-axis (bottom of screen) and a vector starting at zero(bottom-left corner of screen) and terminating at screenPosition.
        angle = Mathf.Atan2(screenPosition.y, screenPosition.x);
        // Slope of the line starting from zero and terminating at screenPosition.
        float slope = Mathf.Tan(angle);

        // Two point's line's form is (y2 - y1) = m (x2 - x1) + c, 
        // starting point (x1, y1) is screen botton-left (0, 0),
        // ending point (x2, y2) is one of the screenBounds,
        // m is the slope
        // c is y intercept which will be 0, as line is passing through origin.
        // Final equation will be y = mx.
        if (screenPosition.x > 0)
        {
            // Keep the x screen position to the maximum x bounds and
            // find the y screen position using y = mx.
            screenPosition = new Vector3(screenBounds.x, screenBounds.x * slope, 0);
        }
        else
        {
            screenPosition = new Vector3(-screenBounds.x, -screenBounds.x * slope, 0);
        }
        // Incase the y ScreenPosition exceeds the y screenBounds 
        if (screenPosition.y > screenBounds.y)
        {
            // Keep the y screen position to the maximum y bounds and
            // find the x screen position using x = y/m.
            screenPosition = new Vector3(screenBounds.y / slope, screenBounds.y, 0);
        }
        else if (screenPosition.y < -screenBounds.y)
        {
            screenPosition = new Vector3(-screenBounds.y / slope, -screenBounds.y, 0);
        }
        // Bring the ScreenPosition back to its original reference.
        screenPosition += screenCentre;
    }
}
