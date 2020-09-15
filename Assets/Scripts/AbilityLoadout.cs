using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityLoadout : MonoBehaviour
{
    public Ability EquippedAbility { get; private set; }

    public void EquipAbility(Ability ability)
    {
        EquippedAbility = ability;
    }

    public void UseEquppiedAbility(Transform origin, Transform target)
    {
        EquippedAbility?.Use(origin, target);
    }

    public void RemoveCurrentAbilityObject()
    {
        foreach(Transform obj in transform)
        {
            Destroy(obj);
        }
    }

    public void CreateNewAbilityObject(Ability ability)
    {
        EquippedAbility = Instantiate(ability, transform.position, Quaternion.identity);
        EquippedAbility.transform.SetParent(transform);
    }
}
