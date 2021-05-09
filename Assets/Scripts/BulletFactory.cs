using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletFactory : IFactory<Bullet>
{
    public Bullet Create()
    {
        var bullet = Resources.Load<Bullet>("Prefabs/Bullet");

        return Object.Instantiate(bullet);
    }
}
