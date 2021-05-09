using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidFactory : IFactory<Asteroid>
{
    public Asteroid Create()
    {
        Asteroid asteroid = null;
        
        int random = Random.Range(0, 3);
        
        if(random == 0)
            asteroid = Resources.Load<Asteroid>("Prefabs/Asteroid (Small)");
        else if (random == 1)
            asteroid = Resources.Load<Asteroid>("Prefabs/Asteroid (Medium)");
        else if (random == 2)
            asteroid = Resources.Load<Asteroid>("Prefabs/Asteroid (Large)");

        return Object.Instantiate(asteroid);
    }
}
