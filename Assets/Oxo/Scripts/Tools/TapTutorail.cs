using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapTutorail : MonoBehaviour
{

    public GameObject clickHandImage;
    public GameObject basicHandImage;
    public GameObject origin;
    public bool alwaysFollow;
    public bool resetMouse;
    public float offset;
    // Update is called once per frame

    Vector2 firstMousePos;
    Vector2 lastMousePos;
    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            basicHandImage.SetActive(false);
            clickHandImage.SetActive(true);
            firstMousePos = Input.mousePosition;
        }

        if (Input.GetMouseButton(0))
        {

            lastMousePos = Input.mousePosition;

            //float dist = Vector2.Distance(origin.transform.position, Input.mousePosition);

            //float yLock = Mathf.Clamp(Input.mousePosition.y, (origin.transform.position.y - offset), (origin.transform.position.y + offset));
            //float xLock = Mathf.Clamp(Input.mousePosition.x, (origin.transform.position.x - offset), (origin.transform.position.x + offset));
            Vector2 mouse = lastMousePos-firstMousePos;
            mouse = Vector2.ClampMagnitude(mouse, offset);
            basicHandImage.GetComponent<RectTransform>().anchoredPosition = /*origin.GetComponent<RectTransform>().anchoredPosition +*/ mouse;
            clickHandImage.GetComponent<RectTransform>().anchoredPosition = /*origin.GetComponent<RectTransform>().anchoredPosition +*/ mouse;

        }

        if (Input.GetMouseButtonUp(0))
        {

            basicHandImage.SetActive(true);
            clickHandImage.SetActive(false);

            if (alwaysFollow || !resetMouse)
                return;

           
            basicHandImage.transform.position = origin.transform.position;
            clickHandImage.transform.position = origin.transform.position;
        }

        if (alwaysFollow)
        {

            Vector2 mouse = lastMousePos - firstMousePos;
            mouse = Vector2.ClampMagnitude(mouse, offset);
            basicHandImage.GetComponent<RectTransform>().anchoredPosition = /*origin.GetComponent<RectTransform>().anchoredPosition +*/ mouse;
            clickHandImage.GetComponent<RectTransform>().anchoredPosition = /*origin.GetComponent<RectTransform>().anchoredPosition +*/ mouse;

            //Vector3 mouse = Input.mousePosition;
            //mouse = Vector3.ClampMagnitude(mouse, offset);
            //basicHandImage.GetComponent<RectTransform>().anchoredPosition = mouse;
            //clickHandImage.GetComponent<RectTransform>().anchoredPosition = mouse;

        }


    }
}
