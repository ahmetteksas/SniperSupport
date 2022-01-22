using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerScript : MonoBehaviour
{
    public float depth;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        return;
        if (gameObject.name == "Character")
        {
            Vector3 mousePos = Input.mousePosition;
            Vector3 wantedPos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, depth));

            transform.LookAt(wantedPos);
        }
        else
        {
            Vector3 mousePos = Input.mousePosition;
            Vector3 wantedPos = Camera.main.ScreenToWorldPoint(new Vector3(-mousePos.x, mousePos.y, depth));

            transform.LookAt(wantedPos);
        }
    }
}
