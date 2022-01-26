using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealFieldDisabler : MonoBehaviour
{
    public float destroyDelay = 1f;

    private void OnEnable()
    {
        StartCoroutine(DestroyObject());
    }

    IEnumerator DestroyObject()
    {
        yield return new WaitForSeconds(destroyDelay);
        gameObject.SetActive(false);
    }
}
