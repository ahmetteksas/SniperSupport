using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootParticleOpen : MonoBehaviour
{
    public Transform shootParticleSpawnPos;
    public string particleTag;

    public void Shoot()
    {
        if (LevelManager.instance.isGameRunning)
        {
            GetComponentInParent<SoldierController>().ShootBullet();
        }
    }
    public void Reload()
    {
        if (LevelManager.instance.isGameRunning)
        {
            GetComponentInParent<SoldierController>().ReloadBullet();
        }
    }
    public void Aim()
    {
        if (LevelManager.instance.isGameRunning)
        {
            GetComponentInParent<SoldierController>().AimStart();
        }
    }
}
