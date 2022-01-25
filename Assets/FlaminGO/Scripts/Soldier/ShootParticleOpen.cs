using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootParticleOpen : MonoBehaviour
{
    public Transform shootParticleSpawnPos;
    public string particleTag;

    public void Shoot()
    {
        GetComponentInParent<SoldierController>().ShootBullet();
        //GameObject shootParticle = ObjectPool.instance.SpawnFromPool(particleTag, shootParticleSpawnPos.position, shootParticleSpawnPos.rotation);
    }
    public void Reload()
    {
        GetComponentInParent<SoldierController>().ReloadBullet();
    }
    public void Aim()
    {
        GetComponentInParent<SoldierController>().AimStart();
    }
}
