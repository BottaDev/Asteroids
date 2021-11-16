using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletBuilder
{
    private float _speed;
    private float _timeToDestroy;
    
    public EnemyBulletBuilder Configure(float speed, float timeToDestroy) 
    {
        _speed = speed;
        _timeToDestroy = timeToDestroy;
        return this;
    }

    public EnemyBullet Build()
    {
        EnemyBulletFactory factory = new EnemyBulletFactory();
        EnemyBullet bullet = factory.Create();
        bullet.Configure(_speed, _timeToDestroy);
        
        return bullet;
    }
}