using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidFlyweightPoint
{
    public static AsteroidFlyweight normal = new AsteroidFlyweight
    {
        speed = 2,
        points = 10
    };
}

public class AsteroidFlyweight
{
    public float speed;
    public int points;
}
