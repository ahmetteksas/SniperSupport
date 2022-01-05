using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class LevelSliderFiller : MonoBehaviour
{
    private Image FillableImage;

    private void Start()
    {
        FillableImage = CanvasManager.instance.FillImage;
    }

    private void LateUpdate()
    {
        FillableImage.fillAmount = transform.position.z / LevelManager.instance.finishPlatform.transform.position.z;
    }
}
