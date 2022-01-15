using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VehicleHit : MonoBehaviour
{
    public Image healthBar;
    public float health;
    public GameObject explodeCar;
    private bool explode;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.fillAmount = health/100;
        if (health == 0)
        {
            if (!explode)
            {
                explode = true;
                StartCoroutine(ExplodeCar());
            }
        }
    }
    IEnumerator ExplodeCar()
    {
        explodeCar.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        gameObject.SetActive(false);
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            health -= 50f;
        }
    }
}
