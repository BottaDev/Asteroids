using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletFlyweightPoint
{
    public static BulletFlyweight normal = new BulletFlyweight
    {
        speed = 10,
        TimeToDestroy = 3
    };
}

public class BulletFlyweight
{
    public float speed;
    public float TimeToDestroy;
}
