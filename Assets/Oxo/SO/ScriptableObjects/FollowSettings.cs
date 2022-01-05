using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class FollowSettings : ScriptableObject
{
    public Plane followPlane;
    public float followLerp;
    public Vector3 offset;

}
