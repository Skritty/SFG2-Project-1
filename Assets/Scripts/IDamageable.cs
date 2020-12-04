using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IDamageable : MonoBehaviour
{
    public abstract void TakeDamage(int amount);
    public abstract void Kill();
}
