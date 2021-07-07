using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpBuilder
{
    private float _timeToDestroy;
    
    public PowerUpBuilder SetTime(float time)
    {
        _timeToDestroy = time;
        return this;
    }
    
    public PowerUp Build()
    {
        PowerUpFactory factory = new PowerUpFactory();
        PowerUp powerUp = factory.Create();
        powerUp.Configure(_timeToDestroy);
        
        return powerUp;
    }
}
