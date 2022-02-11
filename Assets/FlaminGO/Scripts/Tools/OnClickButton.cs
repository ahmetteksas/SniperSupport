using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class OnClickButton : MonoBehaviour
{
    public Image black;
    private float clicked = 0;
    public bool shoot;
    public static bool isMouseOver;
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

        if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject() /*EventSystem.current.IsPointerOverGameObject()*/)
        {
            isMouseOver = true;
        }
        else
        {
            //Debug.Log(isMouseOver);
            isMouseOver = false;
        }
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
    private void OnMouseOver()
    {
        Debug.Log(name + "selected");
    }
}
