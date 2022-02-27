using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHitable
{
    void TakeDamage();

    void TakeDamage(float _damage);
}