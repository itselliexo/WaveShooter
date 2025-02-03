using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour, IDamageable
{
    [SerializeField] int currentHealth;
    [SerializeField] int maxHealth;
    [SerializeField] int healthRegen;
    [SerializeField] float regenDelay;
    [SerializeField] GameObject ammoDrop;
    [SerializeField] WaveManager waveManager;
    [SerializeField] UpgradeManager upgradeManager;
    float healthClock;

    private float timeSinceLastDamage;

    private void Awake()
    {
        currentHealth = maxHealth;
        waveManager = FindObjectOfType<WaveManager>();
        upgradeManager = FindObjectOfType<UpgradeManager>();
    }
    public void Damage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        timeSinceLastDamage = 0f;
    }
    void HealthRegen()
    {
        if (timeSinceLastDamage >= regenDelay)
        {
            currentHealth += healthRegen;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        }
    }
    public void HandleDeath()
    {
        if (currentHealth <= 0f)
        {
            if (CompareTag("Enemy"))
            {
                int dropRate = Random.Range(1, 100);
                if (dropRate < 50 / (waveManager.currentWave * 0.3))
                {
                    if (ammoDrop != null)
                    {
                        Instantiate(ammoDrop, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z), Quaternion.identity);
                    }
                }
                waveManager.enemiesRemaining--;
                upgradeManager.points += 1f;
                upgradeManager.CheckUpgrades();
                Destroy(gameObject);
            }
            if (CompareTag("Player"))
            {
                Debug.Log("You died");
                Destroy(gameObject);
            }
        }
    }
    private void Update()
    {
        timeSinceLastDamage += Time.deltaTime;
        healthClock += Time.deltaTime;
        if (healthClock >= 1f)
        {
            HealthRegen();
            healthClock = 0f;
        }
        HandleDeath();
    }
}
