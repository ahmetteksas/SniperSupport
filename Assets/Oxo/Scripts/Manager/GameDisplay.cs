using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameDisplay : MonoBehaviour
{
    public RunnerGame settings;

    float maxDistance;
    public GameObject playerobj;
    public GameObject endLine;
    public Image levelBar;

    private void Awake()
    {
        if (settings == null)
            Debug.LogError("You need a settings for use this.");

        Application.targetFrameRate = 60;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }
    private void Start()
    {
        maxDistance = getDistance();
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            SceneManager.LoadScene(0);
        }
        if (playerobj.transform.position.z <= maxDistance && playerobj.transform.position.z <= endLine.transform.position.z)
        {
            float distance = 1 - (getDistance() / maxDistance);
            setProgress(distance);
        }
    }
    float getDistance()
    {
        return Vector3.Distance(playerobj.transform.position, endLine.transform.position);
    }
    void setProgress(float p)
    {
        levelBar.fillAmount = p;
    }
    public void StartGame()
    {
        settings.isGameRunning = true;
    }

    public void FinishTheGame()
    {
        settings.isGameRunning = false;
    }
}