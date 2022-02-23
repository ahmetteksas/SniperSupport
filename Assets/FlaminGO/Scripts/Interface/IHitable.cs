using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHitable
{
    //public float health { get; }
    void TakeDamage();
    void TakeDamage(float _damage);
}