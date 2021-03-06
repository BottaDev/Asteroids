using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletFactory : IFactory<Bullet>
{
    public Bullet Create()
    {
        var bullet = ResourceManager.instance.ResourceTable.GetValue("Bullet").GetComponent<Bullet>();

        return Object.Instantiate(bullet);
    }
}
