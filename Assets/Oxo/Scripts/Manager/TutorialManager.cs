using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{

    [HideInInspector]
    public static TutorialManager instance;

    public GameObject tutorialSteps;
    List<GameObject> tutorialStepList = new List<GameObject>();

    int totalTutorialStep;
    int currentTutorialStep;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        currentTutorialStep = PlayerPrefs.GetInt("TutorialStep");
        if (tutorialSteps != null)
            totalTutorialStep = tutorialSteps.transform.childCount;

        FillList(tutorialStepList, tutorialSteps);
    }

    public void OpenTutorialAction()
    {
        tutorialStepList[currentTutorialStep].SetActive(true);
    }

    public void ComplateTutorialAction()
    {
        currentTutorialStep++;
        PlayerPrefs.SetInt("TutorialStep", currentTutorialStep);
    }


    void FillList(List<GameObject> list, GameObject parentObject)
    {
        foreach (Transform child in parentObject.transform)
        {
            list.Add(child.gameObject);
        }
    }
}
