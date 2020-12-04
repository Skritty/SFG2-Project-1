using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(SphereCollider))]
public class FireballProjectile : MonoBehaviour
{
    [SerializeField] float velocity = 5;
    [SerializeField] float acceleration = 0;
    [SerializeField] float gravity = 1;
    [SerializeField] float earlyBlastTime = 2;
    [SerializeField] int damage = 30;

    Rigidbody rb;
    SphereCollider sc;
    bool exploding;
    List<IDamageable> targets = new List<IDamageable>();

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        sc = GetComponent<SphereCollider>();
        rb.velocity = transform.forward * velocity;
        rb.AddForce(Vector3.up * -gravity, ForceMode.Acceleration);
        StartCoroutine(TimeOut());
    }

    private IEnumerator TimeOut()
    {
        yield return new WaitForSeconds(earlyBlastTime);
        if (!exploding) Explode();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(!exploding && collision.gameObject.layer != 8) Explode();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<IDamageable>() && !targets.Contains(other.GetComponent<IDamageable>()))
        {
            targets.Add(other.GetComponent<IDamageable>());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<IDamageable>())
        {
            targets.Remove(other.GetComponent<IDamageable>());
        }
    }

    private void Explode()
    {
        exploding = true;
        foreach(IDamageable h in targets)
        {
            h.TakeDamage(damage);
        }
        //Kaboom
        Destroy(gameObject);
    }
}
