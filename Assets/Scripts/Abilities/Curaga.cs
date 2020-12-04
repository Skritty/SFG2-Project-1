using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Curaga : Ability
{
    [SerializeField]
    private int _amount = 20;
    public override void Use(Transform origin, Transform target)
    {
        if (target == null) return;
        target.GetComponent<IDamageable>()?.TakeDamage(-_amount);
        Debug.Log("HEALING HEAL! - Nia\nThanks! - " + target.name);
    }
}
