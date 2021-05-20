using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBuilder
{
    private float _speed;
    
    public BulletBuilder SetSpeed(float speed) 
    {
        _speed = speed;
        return this;
    }

    public Bullet Build()
    {
        BulletFactory factory = new BulletFactory();
        Bullet bullet = factory.Create();
        bullet.Configure(_speed);
        
        return bullet;
    }
}
