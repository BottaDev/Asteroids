using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletFactory : IFactory<EnemyBullet>
{
    public EnemyBullet Create()
    {
        var bullet = ResourceManager.instance.ResourceTable.GetValue("EnemyBullet").GetComponent<EnemyBullet>();

        return Object.Instantiate(bullet);
    }
}