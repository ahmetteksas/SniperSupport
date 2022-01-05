using ScriptableObjectArchitecture;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvolveSensor : MonoBehaviour
{
    //[SerializeField]
    //GameEvent failGame = default(GameEvent);

    [SerializeField]
    EvolveSensorSettings settings;

    public FloatReference count;

    private int currentGear;

    private void Awake()
    {
        if (settings == null)
            Debug.LogError("You need a settings for use this.");

        //Initialize
        count.Value = settings.startCount;
    }

    public void OnCountChange()
    {
        if (settings.isEvolveOpen)
        {
            currentGear = SelectGear();

            StageChange();
            //Debug.Log(settings.stageList[currentGear]);
        }
    }

    void StageChange()
    {
        settings.PlayerStageChange.Raise(settings.stageList[currentGear]);
    }

    private int SelectGear()
    {
        int _currentGear = 0;

        foreach (int gear in settings.gearValueList)
            if (gear < count.Value)
                _currentGear = settings.gearValueList.IndexOf(gear) + 1;

        return _currentGear;
    }
}
