using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FpsCounter : MonoBehaviour
{
    public float timer, refresh, avgFramerate;
    string display = "{0} FPS";
    private Text m_Text;

    public float minFps;
    public float maxFps;

    public bool debug;
    private void Start()
    {
        m_Text = GetComponent<Text>();
    }


    private void Update()
    {
        //Change smoothDeltaTime to deltaTime or fixedDeltaTime to see the difference
        float timelapse = Time.smoothDeltaTime;
        timer = timer <= 0 ? refresh : timer -= timelapse;

        if (timer <= 0) avgFramerate = (int)(1f / timelapse);

        minFps = avgFramerate < minFps ? Mathf.Abs(avgFramerate) : minFps;
        maxFps = avgFramerate > maxFps ? avgFramerate : maxFps;
        m_Text.text = string.Format(display, avgFramerate.ToString());
    }
    private void OnDisable()
    {
        if (debug)
        {
            Debug.Log("Min FPS: " + minFps);
            Debug.Log("Max FPS: " + maxFps);
        }
    }
}
