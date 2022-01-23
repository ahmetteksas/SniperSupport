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
    private GameObject childSoldier;
    public float radius;
    public float damage = .4f;
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
    void ExplosionDamage(Vector3 center,float radius)
    {
        Collider[] hitColliders = Physics.OverlapSphere(center, radius);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.TryGetComponent(out SoldierController soldierController))
            {
                childSoldier = soldierController.gameObject;
                soldierController.TakeHit(damage);
                //soldierController.isDead = true;
                //soldierController.healthBar.fillAmount -= 1.0f;
                //soldierController.healthBar.fillAmount -= .5f;
                //soldierController.gameObject.SetActive(false);
                //StartCoroutine(DecreaseHealth(soldierController.healthBar));
                StartCoroutine(ExplodeCar(soldierController.gameObject));
            }
            //hitCollider.gameObject.GetComponent<SoldierController>().healthBar.fillAmount -= .2f;
        }
    }
    IEnumerator DecreaseHealth(Image _im)
    {
        yield return new WaitForSeconds(.2f);
        _im.fillAmount -= damage;
    }
    IEnumerator ExplodeCar(GameObject _go)
    {
        yield return new WaitForSeconds(.4f);
        if (_go)
        {
            _go.transform.parent = null;
        }
        yield return new WaitForSeconds(.1f);
        explodeCar.SetActive(true);
        destroyedState.SetActive(true);
        destroyedState.transform.parent = null;
        yield return new WaitForSeconds(2.5f);
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
