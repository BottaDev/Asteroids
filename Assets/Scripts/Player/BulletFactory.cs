using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletFactory : IFactory<Bullet>
{
    public Bullet Create()
    {
        var bullet = LevelManager.instance.Manager.ResourceTable.GetValue("Bullet").GetComponent<Bullet>();

        return Object.Instantiate(bullet);
    }
}
