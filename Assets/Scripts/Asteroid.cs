using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Animations;

public class Asteroid : MonoBehaviour
{
    public float speed;

    public static Asteroid TurnOn(Asteroid asteroid)
    {
        asteroid.gameObject.SetActive(true);
        
        return asteroid;
    }
    
    public static Asteroid TurnOff(Asteroid asteroid)
    {
        asteroid.gameObject.SetActive(false);
        
        return asteroid;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Player bullet
        if (other.gameObject.layer == 10)
            TurnOff(this);
    }
}
