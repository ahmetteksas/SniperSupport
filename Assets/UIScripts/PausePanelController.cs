using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PausePanelController : MonoBehaviour
{
    private void OnEnable()
    {
        Time.timeScale = 0;
    }
}
