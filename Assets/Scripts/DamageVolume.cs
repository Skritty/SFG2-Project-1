using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class DamageVolume : MonoBehaviour
{
    [SerializeField]
    int amount = 1;
    [SerializeField]
    float cooldown;
    float cd;

    private void OnTriggerEnter(Collider other)
    {
        if(cd <= 0)
        if (other.GetComponent<IDamageable>())
        {
            other.GetComponent<IDamageable>().TakeDamage(1);
            cd = cooldown;
        }
    }

    private void FixedUpdate()
    {
        cd -= Time.fixedDeltaTime;
    }
}
