using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Swerve Setting", menuName = "OXO Games/Mechanics/Swerve Setting", order = 0)]
public class SwerveSetting : ScriptableObject
{
    #region Variables
    [Header("Mechanic")]
    public bool isRootControl;
    public bool isPhysic;
    public bool isSensitivityStabilize;
    public float sensitivity;
    public float rotationSensitivity;
    public float maxSwerveRotation;
    public float rotationValue;
    public int stabilizeIteration; 
    #endregion

    //public float maxRotationOffset;
    //public float stopDetectDistance;

    public LayerMask detectMask;

    [Header("Character")]
    public float movementSpeed;
    public float topSpeed;
}
