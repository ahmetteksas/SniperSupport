using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootParticleOpen : MonoBehaviour
{
    public Transform shootParticleSpawnPos;
    public string particleTag;

    public void Shoot()
    {
        return;
        GameObject shootParticle = ObjectPool.instance.SpawnFromPool(particleTag, shootParticleSpawnPos.position, shootParticleSpawnPos.rotation);
    }
}
