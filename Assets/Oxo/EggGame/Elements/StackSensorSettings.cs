using ScriptableObjectArchitecture;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class StackSensorSettings : ScriptableObject
{
    public float startCount;

    //public bool isEvolveOpen;

    public List<string> stageList;
    public List<int> gearValueList;

    public StringGameEvent PlayerStageChange = default(StringGameEvent);

}
