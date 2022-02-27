using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HSDisable : MonoBehaviour
{
    private void OnEnable()
    {
        StartCoroutine(DisableObject());
    }
    IEnumerator DisableObject ()
    {
        yield return new WaitForSeconds(.5f);
        gameObject.SetActive(false);
    }
}
