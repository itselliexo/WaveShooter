using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    [SerializeField] GunManager gunManager;
public void IncreaseAmmo()
    {
        int activeGunIndex = gunManager.GetActiveGunIndex();
        gunManager.guns[activeGunIndex].ammoReserve += gunManager.guns[activeGunIndex].clipSize;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            IncreaseAmmo();
            Destroy(gameObject);
        }
    }
    private void Awake()
    {
        if (gunManager == null)
        {
            gunManager = FindObjectOfType<GunManager>();
            if (gunManager == null)
            {
                Debug.Log("No gun manager found");
            }
        }
    }
}

