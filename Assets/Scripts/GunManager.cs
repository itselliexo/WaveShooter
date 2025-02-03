using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GunManager : MonoBehaviour
{
    [SerializeField] public List<GameObject> gunPrefabs = new List<GameObject>(); //stores a list of gun prefabs
    [SerializeField] public List<Gun> guns = new List<Gun>(); //stores a list of the class "Gun" which is the component which can be found on the gun prefab
    [SerializeField] public GameObject player; //reference to the player
    [SerializeField] public UpgradeManager upgradeManager;
    private int activeGunIndex = 0; //int that tracks the index the list is referencing
    private GameObject currentGunObject; //a game object reference to track the current gun prefab being used

    
    
    private void SwitchGun(int gunIndex)
    {
        if (player != null)
        {
            //checks if the gun index int is invalid and throws a debug statement
            if (gunIndex < 0 || gunIndex >= guns.Count)
            {
                Debug.LogWarning("Invalid gun Index. Cannot switch guns");
            }
            if (!guns[gunIndex].isUnlocked)
            {
                Debug.Log($"{guns[gunIndex].name} is locked and cannot be switched to.");
                return;
            }
            /*if (!guns[gunIndex].enabled)
            {
                Debug.Log($"{guns[gunIndex].name} is locked and cannot be switched to.");
                return;
            }*/
            //loop iterating over the gun class list and setting all of them to inactive before setting the gun at the gun index to active
            foreach (Gun gun in guns)
            {
                gun.SetGunToInactive();
            }
            guns[gunIndex].SetGunToActive();
            //setting the local int vaiable to the arg passed in the method
            activeGunIndex = gunIndex;
            Debug.Log($"Switched to {guns[activeGunIndex].name}");
        }
    }
   
    public void UnlockGun(string gunName)
    {
        Gun gun = guns.Find(g =>  g.name == gunName);
        if (gun != null)
        {
            gun.isUnlocked = true;
            //gun.enabled = true;
           // Debug.Log($"{gunName} unlocked and ready to use!");
        }
        else
        {
            Debug.LogError($"Gun with name {gunName} not found in the list!");
        }
    }
    
    public void AddGun(Gun newGun)
    {
        //if the guns list doesnt already contain the gun passed as an arg, it adds it to the guns list and sets it to inactive
        if (!guns.Contains(newGun))
        {
            guns.Add(newGun);
            newGun.SetGunToInactive();
        }
    }
    
    
    public void RemoveGun(Gun gunToRemove)
    {
        //same as add but to remove
        if (guns.Contains(gunToRemove))
        {
            guns.Remove(gunToRemove);
        }
    }
    
    
    public int GetActiveGunIndex()
    {
        //returns the local current gun index
        return activeGunIndex;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        //gets player reference if none is there
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        UnlockGun("Pistol");
        SwitchGun(0);

        //Gun[] gunScriptsInResources = Resources.LoadAll<Gun>("GunScripts");

        if (guns == null || guns.Count == 0)
        {
            Debug.LogError("No guns initialized. Ensure guns are added to the list.");
            return;
        }
        if (upgradeManager == null)
        {
            upgradeManager = UpgradeManager.FindObjectOfType<UpgradeManager>();
        }
        guns[activeGunIndex].lastFireTime = 0f;
    }

    
    
    // Update is called once per frame
    void Update()
    {
        float currentTime = Time.time;
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchGun(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwitchGun(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SwitchGun(2);
        }

        if (!guns[activeGunIndex].isAutomatic)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0) && guns[activeGunIndex].canFire)
            {
                if (guns[activeGunIndex].lastFireTime >= guns[activeGunIndex].fireRate)
                {
                    guns[activeGunIndex].Fire();
                    guns[activeGunIndex].lastFireTime = 0f;
                }
            }
        }
        else
        {
            if (Input.GetKey(KeyCode.Mouse0) && guns[activeGunIndex].canFire)
            {
                if (currentTime >= guns[activeGunIndex].lastFireTime + guns[activeGunIndex].fireRate)
                {
                    guns[activeGunIndex].Fire();
                    guns[activeGunIndex].lastFireTime = currentTime;
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.R) && guns[activeGunIndex].canReload)
        {
            StartCoroutine(guns[activeGunIndex].Reload());
        }
    }
}
