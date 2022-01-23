using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootParticleOpen : MonoBehaviour
{
    public Transform shootParticleSpawnPos;

    public void OpenShoot()
    {
        GameObject shootParticle = ObjectPool.instance.SpawnFromPool("BulletShootParticle", shootParticleSpawnPos.position, shootParticleSpawnPos.rotation);
    }
}
