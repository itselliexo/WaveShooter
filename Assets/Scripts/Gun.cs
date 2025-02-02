using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Gun : MonoBehaviour
{
    [SerializeField] protected Gun gun;
    [SerializeField] protected Transform firePoint;
    [SerializeField] protected float maxRange = 100f;
    [SerializeField] protected int damage;
    [SerializeField] public int clipSize;
    [SerializeField] protected int currentAmmoCount;
    [SerializeField] public int ammoReserve;
    [SerializeField] public float fireRate;
    [SerializeField] protected float reloadTime;
    [SerializeField] protected float spreadAngle;
    [SerializeField] protected AudioSource audioSource;
    [SerializeField] protected AudioSource hitSource;
    [SerializeField] protected Transform cameraTransform;

    public float lastFireTime = 0f;
    [SerializeField] public bool canFire;
    [SerializeField] public bool canReload;
    [SerializeField] protected bool isActive = false;
    [SerializeField] protected bool reloadWhenEmpty = true;
    [SerializeField] protected bool readyToFire;
    [SerializeField] public bool isUnlocked = false;
    [SerializeField] public bool isAutomatic;
    [SerializeField] public UpgradeManager upgradeManager;

    public void Awake()
    {
        currentAmmoCount = clipSize;
        canFire = true;
        if (cameraTransform == null)
        {
            cameraTransform = Camera.main.transform;
        }
        if (upgradeManager == null)
        {
            upgradeManager = UpgradeManager.FindObjectOfType<UpgradeManager>();
        }
    }
    private void Start()
    {
        currentAmmoCount = clipSize;
        canFire = true;
        
        if (cameraTransform == null)
        {
            cameraTransform = Camera.main.transform;
        }

        if (upgradeManager == null)
        {
            upgradeManager = FindObjectOfType<UpgradeManager>();
        }
        if (firePoint == null)
        {
            firePoint = transform.Find("FirePoint");
            if (firePoint == null)
            {
                Debug.LogWarning("no firepoint found");
            }
        }
    }
    public virtual void Fire()
    {
  
            if (ammoReserve > 0)
            {
                canReload = true;
            }
            if (canFire & currentAmmoCount > 0)
            {
                if (audioSource != null)
                {
                    audioSource.Play();
                }
                Vector3 randomDirection = GetRandomDirection();

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
                currentAmmoCount--;
                if (currentAmmoCount <= 0)
                {
                    canFire = false;
                    if (reloadWhenEmpty && canReload)
                    {
                        StartCoroutine(Reload());
                    }
                }
            }
        }
    protected virtual Color GetRayColour()
    {
        return Color.white;
    }
    protected Vector3 GetRandomDirection()
    {
        Vector2 randomCirclePoint = Random.insideUnitCircle * Mathf.Tan(Mathf.Deg2Rad * spreadAngle);

        Vector3 spreadOffset = new Vector3(randomCirclePoint.x, randomCirclePoint.y, 1).normalized;

        return cameraTransform.TransformDirection(spreadOffset);
    }
    public IEnumerator Reload()
    {
        if (ammoReserve <= 0)
        {
            yield break;
        }
        canFire = false;
        yield return new WaitForSeconds(reloadTime);

        int ammoNeeded = clipSize - currentAmmoCount;
        int ammoToReload = Mathf.Min(ammoNeeded, ammoReserve);

        currentAmmoCount += ammoToReload;
        ammoReserve -= ammoToReload;
        ammoReserve = Mathf.Clamp(ammoReserve, 0, ammoReserve);
        canFire = true;
    }
    public void SetGunToActive()
    {
        isActive = true;
        enabled = true;
        gameObject.SetActive(true);
    }
    public void SetGunToInactive()
    {
        isActive = false;
        enabled = false;
        gameObject.SetActive(false);
    }
}
