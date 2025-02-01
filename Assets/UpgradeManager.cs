using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public enum UpgradeType
{
    Unlock,
    FireRateModifier
}
public class Upgrade
{
    public string name;
    public int pointValue;
    public bool unlocked;
    public UpgradeType upgradeType;

    public float fireRateModifier = 1f;
    public bool unlocksNewGun = false;

    public string targetGunName;
}
public class UpgradeManager : MonoBehaviour
{
    [SerializeField] public List<Upgrade> upgradeList = new List<Upgrade>();
    [SerializeField] GameObject pistol;
    [SerializeField] GameObject shotgun;
    [SerializeField] WaveManager waveManager;
    [SerializeField] GunManager gunManager;
    [SerializeField] float rampingFactor;
    public float points = 0;

    private void Start()
    {
        if (waveManager == null)
        {
            waveManager = FindObjectOfType<WaveManager>();
        }
        if (gunManager == null)
        {
            gunManager = FindObjectOfType<GunManager>();
        }
        upgradeList.Add(new Upgrade {
            name = "Pistol",
            pointValue = 0,
            unlocked = false,
            upgradeType = UpgradeType.Unlock,
            targetGunName = "Pistol"
        });

        upgradeList.Add(new Upgrade {
            name = "Pistol - faster firing",
            pointValue = 2, unlocked = false,
            fireRateModifier = 0.6f,
            upgradeType = UpgradeType.FireRateModifier,
            targetGunName = "Pistol"
        });

        upgradeList.Add(new Upgrade {
            name = "ShotGun",
            pointValue = 4,
            unlocked = false,
            upgradeType = UpgradeType.Unlock,
            targetGunName = "ShotGun"
        });

        upgradeList.Add(new Upgrade {
            name = "Shotgun - faster firing",
            pointValue = 6,
            unlocked = false,
            fireRateModifier = 0.6f,
            upgradeType = UpgradeType.FireRateModifier,
            targetGunName = "ShotGun"
        });

        pistol = GameObject.FindGameObjectWithTag("Pistol");
        shotgun = GameObject.FindGameObjectWithTag("Shotgun");
        CheckUpgrades();
    }
    public void UnlockUpgrade(int upgradeIndex)
    {
        if (upgradeIndex >= 0 && upgradeIndex < upgradeList.Count && !upgradeList[upgradeIndex].unlocked)
        {
            Upgrade upgrade = upgradeList[upgradeIndex];
            upgrade.unlocked = true;

            Debug.Log($"Unlocked {upgradeList[upgradeIndex].name}!");

            if (upgrade.upgradeType == UpgradeType.Unlock)
            {
                gunManager.UnlockGun(upgrade.name);
            }
            else if (upgrade.upgradeType == UpgradeType.FireRateModifier)
            {
                Gun matchingGun = gunManager.guns.Find(g => g.name == upgrade.targetGunName);
                if (matchingGun != null)
                {
                    matchingGun.fireRate *= upgrade.fireRateModifier;
                    Debug.Log($"{matchingGun.name} fire rate modified by {upgrade.fireRateModifier}.");
                }
                else
                {
                    Debug.LogWarning($"Gun {upgrade.name} not found in GunManager.");
                }
            }
        }
    }
    public void CheckUpgrades()
    {
        for (int i = 0; i < upgradeList.Count; i++)
        {
            Upgrade upgrade = upgradeList[i];
            if (!upgrade.unlocked && points >= upgrade.pointValue)
            {
                UnlockUpgrade(i);
                Debug.Log($"upgrade {i} unlocked");
            }
        }
    }
    private void Update()
    {
        if (!waveManager.isWaveReady)
        points -= 0.1f * Time.deltaTime * (points * rampingFactor);
        points = Mathf.Max(points, 0);
    }
}
