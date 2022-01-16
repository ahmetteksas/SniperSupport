using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnClickButton : MonoBehaviour
{
    public GameObject black;
    private float clicked = 0;
    public bool shoot;
    void Start()
    {

    }
    public void BlackClose()
    {
        clicked++;
    }

    void Update()
    {
        if (clicked % 2 == 1)
        {
            black.SetActive(true);
            //black.SetActive(true);
        }
        if (clicked % 2 == 0)
        {
            black.SetActive(false);
        }
    }
}
