using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnClickButton : MonoBehaviour
{
    public Image black;
    private float clicked = 0;
    public bool shoot;
    void Start()
    {

    }
    public void BlackClose()
    {
        if (black.gameObject.activeInHierarchy)
        {
            black.gameObject.SetActive(false);
        }
        //clicked++;
    }

    void Update()
    {
        //if (clicked % 2 == 1)
        //{
        //    black.fillAmount += .1f;
        //    //black.SetActive(true);
        //    //black.SetActive(true);
        //}
        //if (clicked % 2 == 0)
        //{
        //    black.fillAmount -= .1f;
        //    //black.SetActive(false);
        //}
    }
}
