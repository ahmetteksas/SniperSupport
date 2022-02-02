using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TapToPlayButton : MonoBehaviour
{
    Image image;

    private void Awake()
    {
        image = GetComponent<Image>();

        image.raycastTarget = false;
        StartCoroutine(OpenInteraction());
    }
    //void OnEnable()
    //{
       
    //}

    IEnumerator OpenInteraction()
    {
        yield return new WaitForSeconds(.7f);
        image.raycastTarget = true;
    }
}
