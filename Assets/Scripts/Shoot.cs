using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    [SerializeField] Transform firePoint;
    [SerializeField] float maxRange = 100f;
    //[SerializeField] LayerMask hitLayer;
    [SerializeField] int damage;
    float lastFireTime = 0f;
    [SerializeField] float fireRate;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioSource hitSource;
    [SerializeField] private Transform cameraTransform;
   
   void Fire()
    {
        if (audioSource != null)
        {
            audioSource.Play();
        }
        Debug.DrawRay(firePoint.position, cameraTransform.forward * maxRange, Color.red, 1f);

        RaycastHit hit;

        if (Physics.Raycast(firePoint.position, cameraTransform.forward, out hit, maxRange /*hitLayer*/ ))
        {

            IDamageable damageable = hit.collider.GetComponent<IDamageable>();
            if (damageable == null )
            {
                damageable = hit.collider.GetComponentInParent<IDamageable>();
                if(damageable == null )
                {
                    Debug.Log("Missed");
                }
            }
            
            if (damageable != null)
            {
                damageable.Damage(damage);
                hitSource.Play();
            }
        }
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        lastFireTime += Time.deltaTime;
        
        if (Input.GetKeyDown(KeyCode.Mouse0) && lastFireTime >= fireRate)
        {
          lastFireTime = 0f;
          Fire();
        }
    }
}
