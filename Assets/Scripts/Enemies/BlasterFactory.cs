using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlasterFactory : IFactory<BlasterEnemy>
{
    public BlasterEnemy Create()
    {
        BlasterEnemy blaster = ResourceManager.instance.ResourceTable.GetValue("Blaster").GetComponent<BlasterEnemy>();

        return Object.Instantiate(blaster);
    }
}
