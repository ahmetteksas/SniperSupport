using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartPanelController : MonoBehaviour
{
    private ButtonHandler buttonHandler;
    private void OnEnable()
    {
        buttonHandler = ButtonHandler.instance;
        Time.timeScale = 0;
    }
}
