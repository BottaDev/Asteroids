using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlasterBuilder
{
    private float _speed;

    public BlasterBuilder SetSpeed(float speed)
    {
        _speed = speed;
        return this;
    }


    public BlasterEnemy Build()
    {
        BlasterFactory factory = new BlasterFactory();
        BlasterEnemy blaster = factory.Create();
        blaster.Configure(_speed);

        return blaster;
    }
}
