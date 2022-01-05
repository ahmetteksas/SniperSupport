using ScriptableObjectArchitecture;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSensor : MonoBehaviour
{
    [SerializeField]
    HealthSensorSettings settings;

    public FloatReference playerHealth;

    [SerializeField]
    GameEvent failGame = default(GameEvent);


    private int currentGear;

    private void Awake()
    {
        if (settings == null)
            Debug.LogError("You need a settings for use this.");

        //Initialize
        playerHealth.Value = settings.startCount;
    }

    public void OnChangeHealth()
    {
        if (settings.isEvolveOpen)
        {
            currentGear = SelectGear();

            if (currentGear == 0)/*DEMO*/
                failGame.Raise();

            StageChange();
            //Debug.Log(settings.stageList[currentGear]);
        }
    }

    void StageChange()
    {
        settings.PlayerStageChange?.Raise(settings.stageList[currentGear]);
    }

    private int SelectGear()
    {
        int _currentGear = 0;

        foreach (int gear in settings.gearValueList)
            if (gear < playerHealth.Value)
                _currentGear = settings.gearValueList.IndexOf(gear) + 1;

        return _currentGear;
    }
}
