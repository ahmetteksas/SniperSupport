using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;

public class BulletDamage : MonoBehaviour
{
    [SerializeField] FloatVariable enemyHealth;
    [SerializeField] FloatVariable allyHealth;
    public float bulletForce;
    void Start()
    {
        //GetComponent<Rigidbody>().AddForce(gameObject.transform.parent.transform.forward * bulletForce);
        StartCoroutine(DestroyBullet());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            enemyHealth.Value -= 10;
            gameObject.SetActive(false);
        }
        if (other.gameObject.CompareTag("Ally"))
        {
            allyHealth.Value += 10;
            gameObject.SetActive(false);
        }
    }
    IEnumerator DestroyBullet ()
    {
        yield return new WaitForSeconds(6f);
        this.gameObject.SetActive(false);
    }
}
