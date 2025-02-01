using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotGun : Gun
{
    [SerializeField] private int pelletCount;
    public override void Fire()
    {
            canReload = ammoReserve > 0;
            if (canFire & currentAmmoCount > 0)
            {
                if (audioSource != null)
                {
                    audioSource.Play();
                }

                for (int i = 0; i < pelletCount; i++)
                {
                    Vector3 randomDirection = base.GetRandomDirection();

                    Debug.DrawRay(firePoint.position, randomDirection * maxRange, GetRayColour(), 1f);

                    if (Physics.Raycast(firePoint.position, randomDirection, out RaycastHit hit, maxRange))
                    {
                        IDamageable damageable = hit.collider.GetComponent<IDamageable>();
                        if (damageable == null)
                        {
                            damageable = hit.collider.GetComponentInParent<IDamageable>();
                            if (damageable == null)
                            {
                                Debug.Log("Missed");
                            }
                            if (damageable != null)
                            {
                                damageable.Damage(damage);
                                if (hitSource != null)
                                {
                                    hitSource.Play();
                                }
                            }
                        }
                    }
                }
                currentAmmoCount--;
                if (currentAmmoCount <= 0)
                {
                    canFire = false;
                    if (reloadWhenEmpty && canReload)
                    {
                        StartCoroutine(base.Reload());
                    }
                }
            }
        }
    protected override Color GetRayColour()
    {
        return Color.yellow;
    }
    protected new void Awake()
    {
        base.Awake();

        SetGunToInactive();

        this.enabled = false;
    }
    private void Update()
    {
        lastFireTime += Time.deltaTime;

        if (lastFireTime > fireRate)
        {
            readyToFire = true;
        }
        else
        {
            readyToFire = false;
        }
        if (ammoReserve > 0)
        {
            canReload = true;
        }
    }
}
