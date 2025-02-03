using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    [SerializeField] int damage;
    [SerializeField] AudioSource hitSource;
    [SerializeField] float nextHitTime;
    [SerializeField] float hitInterval = 0.7f;
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (Time.time >= nextHitTime)
        {
            if (other.CompareTag("Player"))
            {
                IDamageable damageable = other.GetComponent<IDamageable>();
                if (damageable == null)
                {
                    damageable = other.GetComponentInParent<IDamageable>();
                    if (damageable == null)
                    {
                        Debug.Log("?");
                    }
                }
                if (damageable != null)
                {
                    damageable.Damage(damage);
                    nextHitTime = Time.time + hitInterval;
                    if (hitSource != null)
                    {
                        hitSource.Play();
                    }
                }
            }
        }
    }
}
