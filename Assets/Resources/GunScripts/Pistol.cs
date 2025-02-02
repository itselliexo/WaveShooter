using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : Gun
{
    public override void Fire()
    {
        base.Fire();
    }
    protected override Color GetRayColour()
    {
        return Color.red;
    }
    private void Start()
    {
        isAutomatic = false;
    }
    private void Update()
    {
        lastFireTime += Time.deltaTime;

        if (Time.time >= lastFireTime + fireRate)
        {
            readyToFire = true;
        }
        else
        {
            readyToFire = false;
        }
    }
}
