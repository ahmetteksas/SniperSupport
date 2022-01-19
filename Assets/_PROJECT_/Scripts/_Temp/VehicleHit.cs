using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VehicleHit : MonoBehaviour
{
    //public Image healthBar;
    public float health;
    public GameObject explodeCar;
    private bool explode;
    public GameObject destroyedState;
    public GameObject childSoldier;
    void Start()
    {
        //childSoldier = GameObject.GetComponentInChildren<SoldierController>();
    }

    // Update is called once per frame
    void Update()
    {
        //healthBar.fillAmount = health/100;
        if (health == 0)
        {
            if (!explode)
            {
                explode = true;
                //StartCoroutine(ExplodeCar());
            }
        }
    }
    void ExplosionDamage(Vector3 center, float radius)
    {
        Collider[] hitColliders = Physics.OverlapSphere(center, radius);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.TryGetComponent(out SoldierController soldierController))
            {
                soldierController.healthBar.fillAmount -= 1f;
                soldierController.gameObject.SetActive(false);
            }
            //hitCollider.gameObject.GetComponent<SoldierController>().healthBar.fillAmount -= .2f;
            StartCoroutine(ExplodeCar());
        }
    }
    IEnumerator ExplodeCar()
    {
        if (childSoldier)
        {
            childSoldier.transform.parent = null;
        }
        explodeCar.SetActive(true);
        destroyedState.SetActive(true);
        destroyedState.transform.parent = null;
        yield return new WaitForSeconds(1.5f);
        gameObject.SetActive(false);
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("BulletPlayer"))
        {
            ExplosionDamage(transform.position, 5f);
            health -= 50f;
        }
    }
}
