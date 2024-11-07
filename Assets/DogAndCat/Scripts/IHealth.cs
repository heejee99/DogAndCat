using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHealth
{
    float hpBarAmount { get; } 

    void TakeDamage(float damage);

}
