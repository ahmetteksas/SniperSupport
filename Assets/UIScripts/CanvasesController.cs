using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasesController : MonoBehaviour
{
    private static CanvasesController canvasesController = null;

    private void Awake()
    {
        if(canvasesController == null)
        {
            canvasesController = this;
            DontDestroyOnLoad(canvasesController);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }
}
