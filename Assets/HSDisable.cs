using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HSDisable : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable()
    {
        StartCoroutine(DisableObject());
    }
    IEnumerator DisableObject ()
    {
        yield return new WaitForSeconds(.5f);
        gameObject.SetActive(false);
    }
    void Update()
    {
        
    }
}
