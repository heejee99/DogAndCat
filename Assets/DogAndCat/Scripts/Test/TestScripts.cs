using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScripts : MonoBehaviour
{
    public float damage = 1f;
    public void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.TryGetComponent<Cat>(out Cat cat))
        {
            print("АјАн");
            cat.TakeDamage(damage);
        }
    }
}
