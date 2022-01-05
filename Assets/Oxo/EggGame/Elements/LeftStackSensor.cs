using ScriptableObjectArchitecture;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LeftStackSensor : MonoBehaviour
{
    [SerializeField]
    StackSensorSettings settings;

    //public FloatReference effect;

    public GameObjectCollection relatedList;
    //public FloatReference currentCount;
    public GameObject player;

    private int currentGear;

    private void Awake()
    {
        if (settings == null)
            Debug.LogError("You need a settings for use this.");

        //Initialize
        //stackCollectable.Value = settings.startCount;
    }

    public void OnStackChange()
    {
        currentGear = SelectGear();
        Debug.Log("leftsensor");
        //if (currentGear == 0)/DEMO/
        //    failGame.Raise();

        StageChange();
        ReArrangeFollowers();
        //Debug.Log(settings.stageList[currentGear]);
    }

    void StageChange()
    {
        return;
        settings.PlayerStageChange?.Raise(settings.stageList[currentGear]);
    }

    private int SelectGear()
    {
        int _currentGear = 0;

        foreach (int gear in settings.gearValueList)
            if (gear < relatedList.Count)
                _currentGear = settings.gearValueList.IndexOf(gear) + 1;

        return _currentGear;
    }

    public void ReArrangeFollowers()
    {
        List<GameObject> _releatedList = relatedList.ToList();
        _releatedList = _releatedList.OrderBy(x => x.transform.position.z).ToList();
        foreach (var stack in _releatedList)
        {
            if (stack == _releatedList.FirstOrDefault())
            {
                stack.GetComponent<FollowWithLerp>().SetTarget(player);
            }
            else
            {
                stack.GetComponent<FollowWithLerp>().SetTarget(_releatedList[_releatedList.IndexOf(stack) - 1]);
            }
        }
    }
    
}
