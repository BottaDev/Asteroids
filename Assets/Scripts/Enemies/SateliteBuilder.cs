using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SateliteBuilder 
{
    private float _speed;

    public SateliteBuilder SetSpeed(float speed)
    {
        _speed = speed;
        return this;
    }

    public SateliteEnemy Build()
    {
        SateliteFactory factory = new SateliteFactory();
        SateliteEnemy satelite = factory.Create();
        satelite.Configure(_speed);

        return satelite;
    }
}