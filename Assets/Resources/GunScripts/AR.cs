using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AR : Gun
{
    // Start is called before the first frame update
    void Start()
    {
        isAutomatic = true;
        if (firePoint == null)
        {
            firePoint = transform.Find("FirePoint");
            if (firePoint == null)
            {
                Debug.LogWarning("no firepoint found");
            }
        }
    }
    public override void Fire()
    {
        if (Input.GetKey(KeyCode.Mouse0) && readyToFire)
        { 
            base.Fire();
        }
    }
    protected override Color GetRayColour()
    {
        return Color.green;
    }
    protected new void Awake()
    {
        base.Awake();

        //SetGunToInactive();

        //this.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {

        if (lastFireTime > fireRate)
        {
            readyToFire = true;
        }
        else
        {
            readyToFire = false;
        }
    }
}
