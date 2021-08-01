using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//MyA1-P3
public class BombBuilder
{
    public float chainTime;
    public float radius;
    
    public BombBuilder Configure(float time, float radius) 
    {
        chainTime = time;
        this.radius = radius;
        return this;
    }
    
    public Bomb Build()
    {
        BombFactory factory = new BombFactory();
        Bomb bomb = factory.Create();
        bomb.chainTime = chainTime;
        bomb.radius = radius;

        return bomb;
    }
}
