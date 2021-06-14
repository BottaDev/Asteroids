using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidFactory : IFactory<Asteroid>
{
    public Asteroid Create()
    {
        int random = Random.Range(0, 3);
        
        Asteroid asteroid = null;

        if (random == 0)
            asteroid = ResourceManager.instance.ResourceTable.GetValue("SmallAsteroid").GetComponent<Asteroid>();
        else if (random == 1)
            asteroid = ResourceManager.instance.ResourceTable.GetValue("MediumAsteroid").GetComponent<Asteroid>();
        else if (random == 2)
            asteroid = ResourceManager.instance.ResourceTable.GetValue("LargeAsteroid").GetComponent<Asteroid>();
        
        return Object.Instantiate(asteroid);
    }
    
}
