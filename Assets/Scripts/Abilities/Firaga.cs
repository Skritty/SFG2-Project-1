using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Firaga : Ability
{
    [SerializeField]
    int rank = 3;
    [SerializeField]
    GameObject fireball;
    public override void Use(Transform origin, Transform target)
    {
        if (fireball == null) return;
        GameObject projectile = Instantiate(fireball, origin.position, origin.rotation);
        if (target)
        {
            projectile.transform.LookAt(target);
        }
        Debug.Log("I cast fireball at "+rank+" level!\n"+target.name+": o_o");
    }
}
