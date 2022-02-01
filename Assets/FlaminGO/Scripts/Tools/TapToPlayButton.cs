using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TapToPlayButton : MonoBehaviour
{
    Image image;

    void OnEnable()
    {
        image = GetComponent<Image>();

        image.raycastTarget = false;
        StartCoroutine(OpenInteraction());
    }

    IEnumerator OpenInteraction()
    {
        yield return new WaitForSeconds(.3f);
        image.raycastTarget = true;
    }
}
