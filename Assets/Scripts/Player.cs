using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    public float Hp;
    public float Speed;
    public float RotationSpeed;
    [Range(min: 0, max: 1)]
    public float FireRate;

    protected void Update()
    {
        base.Update();
    }
}
