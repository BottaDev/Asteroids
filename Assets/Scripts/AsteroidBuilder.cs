using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidBuilder
{
    private float _speed;
    
    public AsteroidBuilder SetSpeed(float speed) 
    {
        _speed = speed;
        return this;
    }

    public Asteroid Build()
    {
        AsteroidFactory factory = new AsteroidFactory();
        Asteroid asteroid = factory.Create();
        asteroid.Configure(_speed);
        
        return asteroid;
    }
}
