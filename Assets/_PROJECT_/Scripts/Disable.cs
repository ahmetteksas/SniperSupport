using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Disable : MonoBehaviour
{

    private void OnEnable()
    {
        gameObject.transform.localScale = Vector3.zero;
        gameObject.transform.DOScale(Vector3.one * .2f, .4f);
        //StartCoroutine(DisableObject());
    }
    IEnumerator DisableObject()
    {
        yield return new WaitForSeconds(3f);
        gameObject.SetActive(false);
    }
}
