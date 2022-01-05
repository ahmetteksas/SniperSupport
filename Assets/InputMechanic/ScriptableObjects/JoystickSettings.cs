using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Joystick Setting", menuName = "OXO Games/Mechanics/Joystick Setting", order = 0)]
public class JoystickSettings : ScriptableObject
{
    [Header("Mechanic")]
    public bool isRootControl;
    public float rotationSensitivity;
    //public float maxRotationOffset;
    //public float stopDetectDistance;

    public LayerMask detectMask;

    [Header("Character")]
    public float movementSpeed;
    public float topSpeed;
}
