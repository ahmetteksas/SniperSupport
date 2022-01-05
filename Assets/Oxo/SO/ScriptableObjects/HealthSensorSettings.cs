using ScriptableObjectArchitecture;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class HealthSensorSettings : ScriptableObject
{
    public bool isEvolveOpen;
    public float startCount;
    public List<string> stageList;
    public List<int> gearValueList;

    public StringGameEvent PlayerStageChange = default(StringGameEvent);

}
